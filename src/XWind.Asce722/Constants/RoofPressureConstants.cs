// XWind.Asce722.Constants.RoofPressureConstants.cs
using System;
using System.Collections.Generic;

namespace XWind.Asce722.Constants
{
    public static class RoofPressureConstants
    {
        // ---------------------- WINDWARD: Normal to ridge ----------------------
        // Structure: h/L → angle → (Cp1, Cp2)
        public static readonly Dictionary<double, Dictionary<double, (double Cp1, double Cp2)>> WindwardCp
            = new Dictionary<double, Dictionary<double, (double Cp1, double Cp2)>>
            {
                [0.25] = new Dictionary<double, (double, double)>
                {
                    [10] = (-0.7, -0.18),
                    [15] = (-0.5, 0.0),
                    [20] = (-0.3, 0.2),
                    [25] = (-0.2, 0.3),
                    [30] = (-0.2, 0.3),
                    [35] = (0.0, 0.4),
                    [45] = (0.4, 0.4),
                    [60] = (0.6, 0.6),
                    [70] = (0.01 * 70, 0.01 * 70), // 60–80° rule baked-in
                    [80] = (0.01 * 80, 0.01 * 80),
                    [90] = (0.8, 0.8)
                },
                [0.5] = new Dictionary<double, (double, double)>
                {
                    [10] = (-0.9, -0.18),
                    [15] = (-0.7, -0.18),
                    [20] = (-0.4, 0.0),
                    [25] = (-0.3, 0.2),
                    [30] = (-0.2, 0.2),
                    [35] = (-0.2, 0.3),
                    [45] = (0.0, 0.4),
                    [60] = (0.6, 0.6),
                    [70] = (0.01 * 70, 0.01 * 70),
                    [80] = (0.01 * 80, 0.01 * 80),
                    [90] = (0.8, 0.8)
                },
                [1.0] = new Dictionary<double, (double, double)>
                {
                    [10] = (-1.3, -0.18),
                    [15] = (-1.0, -0.18),
                    [20] = (-0.7, 0.0),
                    [25] = (-0.5, 0.2),
                    [30] = (-0.3, 0.2),
                    [35] = (-0.2, 0.2),
                    [45] = (0.0, 0.3),
                    [60] = (0.6, 0.6),
                    [70] = (0.01 * 70, 0.01 * 70),
                    [80] = (0.01 * 80, 0.01 * 80),
                    [90] = (0.8, 0.8)
                }
            };

        // ---------------------- LEEWARD: Normal to ridge ----------------------
        // Structure: h/L → angle → Cp
        // Angle anchors are 10°, 15°, and 20° (20° represents ≥20° flat continuation)
        public static readonly Dictionary<double, Dictionary<double, double>> LeewardCp
            = new Dictionary<double, Dictionary<double, double>>
            {
                [0.25] = new Dictionary<double, double> { [10] = -0.30, [15] = -0.50, [20] = -0.60 },
                [0.50] = new Dictionary<double, double> { [10] = -0.50, [15] = -0.50, [20] = -0.60 },
                [1.00] = new Dictionary<double, double> { [10] = -0.70, [15] = -0.60, [20] = -0.60 },
            };

        // ---------------------- PARALLEL TO RIDGE ----------------------
        // Each zone has a Cp1 for h/L ≤ 0.5 and for h/L ≥ 1.0; Cp2 (alternate) is -0.18 everywhere.
        // Interpolate Cp1 linearly when 0.5 < h/L < 1.0.
        public static readonly (string Zone, double Cp1_LE_05, double Cp1_GE_10, double Cp2_All)[] ParallelToRidgeZones =
        {
            ("0 to h/2",  -0.90, -1.30, -0.18),
            ("h/2 to h",  -0.90, -0.70, -0.18),
            ("h to 2h",   -0.50, -0.70, -0.18),
            ("> 2h",      -0.30, -0.70, -0.18),
        };

        // ---------------------- AREA-REDUCTION BREAKPOINTS (optional centralization) ----------------------
        public static class AreaReduction
        {
            public const double A1 = 100.0; public const double R1 = 1.00;
            public const double A2 = 250.0; public const double R2 = 0.90;
            public const double A3 = 1000.0; public const double R3 = 0.80;
        }
    }
}
