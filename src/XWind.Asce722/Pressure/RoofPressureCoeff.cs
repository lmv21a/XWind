using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.Interpolation;
using XWind.Asce722.Constants; // expects RoofPressureConstants.*

namespace XWind.Asce722.Pressure
{
    /// <summary>
    /// Interpolates roof pressure coefficients for the Directional Procedure (ASCE 7-22 Ch. 27).
    /// Sources: figure-based Cp tables for Windward, Leeward, and Parallel-to-ridge winds.
    /// </summary>
    public class RoofPressureCoeff
    {
        /// <summary>
        /// Windward roof coefficients (Cp1, Cp2) for wind normal to ridge.
        /// Uses bilinear interpolation in h/L and roof angle; inputs are clamped to table bounds.
        /// </summary>
        /// <param name="lengthL">Plan length L (&gt; 0). Used to compute h/L.</param>
        /// <param name="heightH">Mean roof height h (&gt; 0). Used to compute h/L.</param>
        /// <param name="angleDeg">Wind/roof angle in degrees (matches Cp table columns).</param>
        /// <returns>
        /// Tuple of Cp values (Cp1, Cp2) for the windward roof zones corresponding to the given h/L and angle.
        /// </returns>
        public static (double Cp1, double Cp2) GetWindwardRoofCp(double lengthL, double heightH, double angleDeg)
        {
            if (lengthL <= 0) throw new ArgumentException("Length L must be > 0.", nameof(lengthL));
            if (heightH <= 0) throw new ArgumentException("Height h must be > 0.", nameof(heightH));

            double hL = heightH / lengthL;

            // Anchors from constants
            double[] hAnchors = RoofPressureConstants.WindwardCp.Keys.OrderBy(x => x).ToArray();
            if (hAnchors.Length < 2) throw new InvalidOperationException("WindwardCp requires at least two h/L anchors.");

            double[] angleCols = RoofPressureConstants.WindwardCp[hAnchors[0]].Keys.OrderBy(x => x).ToArray();
            if (angleCols.Length < 1) throw new InvalidOperationException("WindwardCp requires at least one angle column.");

            // Cap inputs
            double h = Clamp(hL, hAnchors.First(), hAnchors.Last());
            double a = Clamp(angleDeg, angleCols.First(), angleCols.Last());

            // Exact angle: 1D interpolation in h/L
            if (TryGetExactAngle(angleCols, a, out double exactAngle))
            {
                return InterpolateAlongH(exactAngle, hAnchors, h);
            }

            // Bilinear: interpolate along h/L at each bounding angle, then across angle
            GetAngleBracket(a, angleCols, out double a0, out double a1, out _);

            var p0 = InterpolateAlongH(a0, hAnchors, h); // (Cp1,Cp2) at a0
            var p1 = InterpolateAlongH(a1, hAnchors, h); // (Cp1,Cp2) at a1

            var sCp1 = LinearSpline.InterpolateSorted(new[] { a0, a1 }, new[] { p0.Cp1, p1.Cp1 });
            var sCp2 = LinearSpline.InterpolateSorted(new[] { a0, a1 }, new[] { p0.Cp2, p1.Cp2 });

            return (sCp1.Interpolate(a), sCp2.Interpolate(a));
        }

        /// <summary>
        /// Windward roof coefficients (Cp1, Cp2) with optional area reduction.
        /// Reduction applies only when Cp == −1.30 (per constants breakpoints).
        /// </summary>
        /// <param name="lengthL">Plan length L (&gt; 0). Used to compute h/L.</param>
        /// <param name="heightH">Mean roof height h (&gt; 0). Used to compute h/L.</param>
        /// <param name="angleDeg">Wind/roof angle in degrees.</param>
        /// <param name="planArea">Projected roof plan area (ft² or m²). If &gt; 0, may reduce Cp = −1.30.</param>
        /// <returns>
        /// Tuple (Cp1, Cp2) after applying area reduction (only if Cp == −1.30); otherwise original values.
        /// </returns>
        public static (double Cp1, double Cp2) GetWindwardRoofCp(double lengthL, double heightH, double angleDeg, double planArea)
        {
            var (c1, c2) = GetWindwardRoofCp(lengthL, heightH, angleDeg);
            c1 = MaybeReduceNeg1p30_Constants(c1, planArea, out _);
            c2 = MaybeReduceNeg1p30_Constants(c2, planArea, out _);
            return (c1, c2);
        }

        /// <summary>
        /// Leeward roof coefficient (Cp) for wind normal to ridge.
        /// Bilinear interpolation in h/L and angle; inputs are clamped to table bounds.
        /// </summary>
        /// <param name="hOverL">Ratio h/L (unitless). Values outside table range are clamped.</param>
        /// <param name="angleDeg">Wind/roof angle in degrees (matches Cp table columns).</param>
        /// <returns>
        /// Interpolated leeward Cp for the specified h/L and angle.
        /// </returns>
        public static double GetLeewardCp(double hOverL, double angleDeg)
        {
            // Anchors from constants
            var hAnchors = RoofPressureConstants.LeewardCp.Keys.OrderBy(x => x).ToArray();
            var angleAnchors = RoofPressureConstants.LeewardCp[hAnchors[0]].Keys.OrderBy(x => x).ToArray();

            // Cap inputs
            double h = Clamp(hOverL, hAnchors.First(), hAnchors.Last());
            double a = Clamp(angleDeg, angleAnchors.First(), angleAnchors.Last());

            // Exact angle column?
            if (TryGetExactAngle(angleAnchors, a, out double exactAngle))
            {
                var cpVals = new double[hAnchors.Length];
                for (int i = 0; i < hAnchors.Length; i++)
                    cpVals[i] = RoofPressureConstants.LeewardCp[hAnchors[i]][exactAngle];

                var s = LinearSpline.InterpolateSorted(hAnchors, cpVals);
                return s.Interpolate(h);
            }

            // Bilinear: along h for each bounding angle, then across angle
            GetAngleBracket(a, angleAnchors, out double a0, out double a1, out _);

            double[] cpAtA0 = new double[hAnchors.Length];
            double[] cpAtA1 = new double[hAnchors.Length];
            for (int i = 0; i < hAnchors.Length; i++)
            {
                cpAtA0[i] = RoofPressureConstants.LeewardCp[hAnchors[i]][a0];
                cpAtA1[i] = RoofPressureConstants.LeewardCp[hAnchors[i]][a1];
            }

            var s0 = LinearSpline.InterpolateSorted(hAnchors, cpAtA0);
            var s1 = LinearSpline.InterpolateSorted(hAnchors, cpAtA1);

            double c0 = s0.Interpolate(h);
            double c1 = s1.Interpolate(h);

            var sA = LinearSpline.InterpolateSorted(new[] { a0, a1 }, new[] { c0, c1 });
            return sA.Interpolate(a);
        }

        /// <summary>
        /// Leeward roof coefficient (Cp) with optional area reduction.
        /// Reduction applies only when Cp == −1.30 (per constants breakpoints).
        /// </summary>
        /// <param name="hOverL">Ratio h/L (unitless). Values outside table range are clamped.</param>
        /// <param name="angleDeg">Wind/roof angle in degrees.</param>
        /// <param name="planArea">Projected roof plan area (ft² or m²). If &gt; 0, may reduce Cp = −1.30.</param>
        /// <returns>
        /// Leeward Cp after applying area reduction (only if Cp == −1.30); otherwise original Cp.
        /// </returns>
        public static double GetLeewardCp(double hOverL, double angleDeg, double planArea)
        {
            double c = GetLeewardCp(hOverL, angleDeg);
            return MaybeReduceNeg1p30_Constants(c, planArea, out _);
        }

        /// <summary>
        /// Cp values by zone for wind parallel to ridge (no area reduction).
        /// Cp1 is piecewise in h/L (interpolated for 0.5 &lt; h/L &lt; 1.0); Cp2 is constant from constants.
        /// </summary>
        /// <param name="hOverL">Ratio h/L (unitless). Used to pick or interpolate zone Cp1.</param>
        /// <returns>
        /// List of tuples per zone: (Zone label, Cp1, Cp2, ReductionFactorCp1 = null).
        /// </returns>
        public static List<(string Zone, double Cp1, double Cp2, double? ReductionFactorCp1)>
            GetParallelToRidge(double hOverL)
            => GetParallelToRidge(hOverL, planArea: null);

        /// <summary>
        /// Cp values by zone for wind parallel to ridge with optional area reduction on Cp1.
        /// Reduction applies only when Cp1 == −1.30 (per constants breakpoints).
        /// </summary>
        /// <param name="hOverL">Ratio h/L (unitless). Used to pick or interpolate zone Cp1.</param>
        /// <param name="planArea">Projected roof plan area (ft² or m²). If &gt; 0, may reduce Cp1 = −1.30.</param>
        /// <returns>
        /// List of tuples per zone: (Zone label, Cp1 (possibly reduced), Cp2, ReductionFactorCp1 if applied; otherwise null).
        /// </returns>
        public static List<(string Zone, double Cp1, double Cp2, double? ReductionFactorCp1)>
            GetParallelToRidge(double hOverL, double? planArea)
        {
            var results = new List<(string Zone, double Cp1, double Cp2, double? ReductionFactorCp1)>();
            var zones = RoofPressureConstants.ParallelToRidgeZones;

            if (hOverL <= 0.50)
            {
                foreach (var z in zones)
                {
                    double cp1 = z.Cp1_LE_05;
                    double? r = MaybeReduceNeg1p30_Constants(cp1, planArea);
                    if (r.HasValue) cp1 *= r.Value;

                    results.Add((z.Zone, cp1, z.Cp2_All, r));
                }
            }
            else if (hOverL >= 1.00)
            {
                foreach (var z in zones)
                {
                    double cp1 = z.Cp1_GE_10;
                    double? r = MaybeReduceNeg1p30_Constants(cp1, planArea);
                    if (r.HasValue) cp1 *= r.Value;

                    results.Add((z.Zone, cp1, z.Cp2_All, r));
                }
            }
            else
            {
                // 0.5 < h/L < 1.0 → interpolate Cp1 with MathNet
                double[] xs = { 0.50, 1.00 };

                foreach (var z in zones)
                {
                    double[] ys = { z.Cp1_LE_05, z.Cp1_GE_10 };
                    var s = LinearSpline.InterpolateSorted(xs, ys);
                    double cp1Interp = s.Interpolate(hOverL);

                    double? r = MaybeReduceNeg1p30_Constants(cp1Interp, planArea);
                    if (r.HasValue) cp1Interp *= r.Value;

                    results.Add((z.Zone, cp1Interp, z.Cp2_All, r));
                }
            }

            return results;
        }

        // ===== Internals =====

        /// <summary>Interpolates (Cp1, Cp2) along h/L for a fixed angle column.</summary>
        private static (double Cp1, double Cp2) InterpolateAlongH(double angleCol, double[] hAnchors, double h)
        {
            var cp1 = new double[hAnchors.Length];
            var cp2 = new double[hAnchors.Length];

            for (int i = 0; i < hAnchors.Length; i++)
            {
                double hKey = hAnchors[i];
                var row = RoofPressureConstants.WindwardCp[hKey];
                if (!row.TryGetValue(angleCol, out var pair))
                    throw new KeyNotFoundException($"No Cp pair for h/L={hKey} at angle={angleCol}°");

                cp1[i] = pair.Cp1;
                cp2[i] = pair.Cp2;
            }

            var s1 = LinearSpline.InterpolateSorted(hAnchors, cp1);
            var s2 = LinearSpline.InterpolateSorted(hAnchors, cp2);

            return (s1.Interpolate(h), s2.Interpolate(h));
        }

        /// <summary>Finds bounding angle columns around 'a' and the interpolation fraction t ∈ [0,1].</summary>
        private static void GetAngleBracket(double a, double[] angleCols, out double a0, out double a1, out double t)
        {
            a0 = angleCols[0];
            a1 = angleCols[^1];
            t = 0.0;

            for (int i = 0; i < angleCols.Length - 1; i++)
            {
                double left = angleCols[i];
                double right = angleCols[i + 1];
                if (a >= left && a <= right)
                {
                    a0 = left;
                    a1 = right;
                    t = (a - left) / (right - left);
                    return;
                }
            }

            // Shouldn't reach here due to clamping; snap to nearest edge if we do
            a0 = a1 = a <= angleCols[0] ? angleCols[0] : angleCols[^1];
            t = 0.0;
        }

        /// <summary>True if 'a' matches an angle column within tolerance; returns the exact column value.</summary>
        private static bool TryGetExactAngle(double[] angleCols, double a, out double exact)
        {
            foreach (var col in angleCols)
            {
                if (Math.Abs(a - col) < 1e-9) { exact = col; return true; }
            }
            exact = 0;
            return false;
        }

        /// <summary>
        /// If cp == −1.30 and planArea is valid, returns reduced Cp and outputs the factor R via <paramref name="rApplied"/>.
        /// Otherwise returns cp unchanged and R=1.
        /// </summary>
        /// <param name="cp">Original Cp value.</param>
        /// <param name="planArea">Plan area for reduction check (null or ≤0 disables reduction).</param>
        /// <param name="rApplied">Reduction factor actually applied (1.0 if no reduction).</param>
        /// <returns>Reduced Cp when eligible; otherwise the original Cp.</returns>
        private static double MaybeReduceNeg1p30_Constants(double cp, double? planArea, out double rApplied)
        {
            if (planArea.HasValue && planArea.Value > 0 && Math.Abs(cp - (-1.30)) < 1e-9)
            {
                double A = planArea.Value;
                var A1 = RoofPressureConstants.AreaReduction.A1; var R1 = RoofPressureConstants.AreaReduction.R1;
                var A2 = RoofPressureConstants.AreaReduction.A2; var R2 = RoofPressureConstants.AreaReduction.R2;
                var A3 = RoofPressureConstants.AreaReduction.A3; var R3 = RoofPressureConstants.AreaReduction.R3;

                double R;
                if (A <= A1) R = R1;
                else if (A >= A3) R = R3;
                else if (A <= A2) R = Lerp(A, A1, R1, A2, R2); // 100..250 → 1.00..0.90
                else R = Lerp(A, A2, R2, A3, R3);              // 250..1000 → 0.90..0.80

                rApplied = R;
                return cp * R;
            }

            rApplied = 1.0;
            return cp;
        }

        /// <summary>
        /// Returns reduction factor R(A) when cp == −1.30 and area is valid; otherwise null.
        /// </summary>
        /// <param name="cp">Original Cp value.</param>
        /// <param name="planArea">Plan area for reduction check.</param>
        /// <returns>R in (0,1] if reduction applies; otherwise null.</returns>
        private static double? MaybeReduceNeg1p30_Constants(double cp, double? planArea)
        {
            if (!planArea.HasValue || planArea.Value <= 0) return null;
            if (Math.Abs(cp - (-1.30)) > 1e-9) return null;

            double A = planArea.Value;
            var A1 = RoofPressureConstants.AreaReduction.A1; var R1 = RoofPressureConstants.AreaReduction.R1;
            var A2 = RoofPressureConstants.AreaReduction.A2; var R2 = RoofPressureConstants.AreaReduction.R2;
            var A3 = RoofPressureConstants.AreaReduction.A3; var R3 = RoofPressureConstants.AreaReduction.R3;

            if (A <= A1) return R1;
            if (A >= A3) return R3;
            if (A <= A2) return Lerp(A, A1, R1, A2, R2); // 100..250 → 1.00..0.90
            return Lerp(A, A2, R2, A3, R3);              // 250..1000 → 0.90..0.80
        }

        private static double Clamp(double v, double lo, double hi) => v < lo ? lo : (v > hi ? hi : v);

        private static double Lerp(double x, double x0, double y0, double x1, double y1)
        {
            if (Math.Abs(x1 - x0) < 1e-12) return y0;
            double t = (x - x0) / (x1 - x0);
            return y0 + t * (y1 - y0);
        }
    }
}
