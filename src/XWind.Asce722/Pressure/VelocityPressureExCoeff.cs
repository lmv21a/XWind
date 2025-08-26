
using XWind.Asce722.Constants;
using XWind.Core.Enums;

namespace XWind.Asce722.Pressure
{
    public class VelocityPressureExCoeff
    {
        public TerrainExposureConstant TerrainConstants { get; set; } = new TerrainExposureConstant();

        /// <summary>
        /// Generate (z, Kz) samples from z = zStart to zEnd inclusive with step zStep.
        /// </summary>
        public List<(double z, double Kz)> KzSeries(ExposureCategory category, double zStart, double zEnd, double zStep)
        {
            if (zStart <= 0) throw new ArgumentOutOfRangeException(nameof(zStart), "zStart must be > 0.");
            if (zEnd <= 0 || zEnd < zStart) throw new ArgumentOutOfRangeException(nameof(zEnd), "zEnd must be >= zStart and > 0.");
            if (zStep <= 0) throw new ArgumentOutOfRangeException(nameof(zStep), "zStep must be > 0.");

            var alpha = TerrainConstants.GetAlpha(category);
            var zg = TerrainConstants.GetZg(category);

            var points = new List<(double z, double Kz)>();
            for (double z = zStart; z <= zEnd + 1e-9; z += zStep)
            {
                var kz = GetKz(z, zg, alpha); // existing scalar helper
                points.Add((z, kz));
            }
            return points;
        }
        // === Scalar helpers (useful for tests or discrete calculations) ===

        /// <summary>ASCE 7‑22 Table 26.10‑1 rules for Kz.</summary>
        public static double GetKz(double z, double zg, double alpha)
        {
            if (z <= 0) throw new ArgumentOutOfRangeException(nameof(z), "z must be > 0.");
            if (zg <= 0) throw new ArgumentOutOfRangeException(nameof(zg), "zg must be > 0.");
            if (alpha <= 0) throw new ArgumentOutOfRangeException(nameof(alpha), "alpha must be > 0.");

            if (z < 15.0)
                return 2.41 * Math.Pow(15.0 / zg, 2.0 / alpha);   // use 15 ft
            if (z <= zg)
                return 2.41 * Math.Pow(z / zg, 2.0 / alpha);
            if (z <= 3280.0)
                return 2.41;

            throw new ArgumentOutOfRangeException(nameof(z), "z must be ≤ 3280 ft.");
        }

        /// <summary>Kh at mean roof height h (same rules as Kz).</summary>
        public static double GetKh(double h, double zg, double alpha) => GetKz(h, zg, alpha);

        internal object KzSeries(ExposureCategory exposure, double zMax, double zStep)
        {
            throw new NotImplementedException();
        }
    }
}
