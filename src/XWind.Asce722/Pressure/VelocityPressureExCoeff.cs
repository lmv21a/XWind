using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XWind.Asce722.Constants;
using XWind.Core.Enums;

namespace XWind.Asce722.Pressure
{
    public class VelocityPressureExCoeff
    {
        public ExposureCategory Category { get; set; }
        /// <summary>Gets or sets the z = Height above ground level, ft</summary>
        public double Z { get; set; }
        /// <summary>h = Mean roof height of a building or height of other structure, except that eave height shall be used for roof angle θ less than or equal to 10 degrees, in ft</summary>
        public double H { get; set; }
        private double Zg { get; set; }
        private double Alpha { get; set; }
        private double ZSteps { get; set; }

        public TerrainExposureConstant TerrainConstants { get; set; } = new TerrainExposureConstant();

        /// <summary>Get velocity pressure coefficient, Kz.</summary>
        /// <param name="Category">The category.</param>
        /// <param name="z">The height above ground level z.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.ArgumentException">Exposure type for velocity exposure coefficient not supported</exception>
        public List<double> Kz(ExposureCategory category, double z, double zSteps)
        {
            if (z <= 0)
                throw new ArgumentOutOfRangeException(nameof(z), "z must be > 0.");
            if (zSteps <= 0)
                throw new ArgumentOutOfRangeException(nameof(zSteps), "zSteps must be > 0.");

            var alpha = TerrainConstants.GetAlpha(category);
            var zg = TerrainConstants.GetZg(category);
            List<double> kzValues = new List<double>();

            for (double i = 0; i <= z; i += zSteps)
            {
                kzValues.Add(GetKz(i, zg, alpha));
            }
            return kzValues;
        }


        /// <summary>Contains formulas to obtain velocity pressure exposure coefficient values from z (Kh and/or Kz), Zg, and Alpha.</summary>
        /// <param name="z">The height above ground level z.</param>
        /// <param name="Zg">Nominal height of the atmospheric boundary layer, Zg</param>
        /// <param name="Alpha">α = Mean hourly wind-speed power law exponent</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.ArgumentException">Check z, Zg, and/or Alpha Values</exception>
        public static double GetKz(double z, double zg, double alpha)
        {
            if (z <= 0) throw new ArgumentOutOfRangeException(nameof(z), "z must be > 0.");
            if (zg <= 0) throw new ArgumentOutOfRangeException(nameof(zg), "zg must be > 0.");
            if (alpha <= 0) throw new ArgumentOutOfRangeException(nameof(alpha), "alpha must be > 0.");

            // Per ASCE 7-22 Table 26.10-1 notes:
            // For z < 15 ft:   Kz = 2.41 * (15/zg)^(2/alpha)
            // For 15 <= z <= zg: Kz = 2.41 * (z/zg)^(2/alpha)
            // For zg < z <= 3280 ft: Kz = 2.41
            if (z < 15.0)
                return 2.41 * Math.Pow(15.0 / zg, 2.0 / alpha);

            if (z <= zg)
                return 2.41 * Math.Pow(z / zg, 2.0 / alpha);

            if (z <= 3280.0)
                return 2.41;

            throw new ArgumentOutOfRangeException(nameof(z), "z must be ≤ 3280 ft per ASCE 7-22.");
        }


        /// <summary>Contains formulas to obtain velocity pressure exposure coefficient values from h (Kh and/or Kz), Zg, and Alpha</summary>
        /// <param name="h">The h.</param>
        /// <param name="zg">The zg.</param>
        /// <param name="alpha">The alpha.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <exception cref="System.ArgumentOutOfRangeException">h - h must be &gt; 0.
        /// or
        /// zg - zg must be &gt; 0.
        /// or
        /// alpha - alpha must be &gt; 0.
        /// or
        /// h - h must be ≤ 3280 ft per ASCE 7-22.</exception>
        public static double GetKh(double h, double zg, double alpha)
        {
            if (h <= 0) throw new ArgumentOutOfRangeException(nameof(h), "h must be > 0.");
            if (zg <= 0) throw new ArgumentOutOfRangeException(nameof(zg), "zg must be > 0.");
            if (alpha <= 0) throw new ArgumentOutOfRangeException(nameof(alpha), "alpha must be > 0.");

            // Per ASCE 7-22 Table 26.10-1 notes:
            // For h < 15 ft:   Kz = 2.41 * (15/zg)^(2/alpha)
            // For 15 <= h <= zg: Kz = 2.41 * (h/zg)^(2/alpha)
            // For zg < h <= 3280 ft: Kz = 2.41
            if (h < 15.0)
                return 2.41 * Math.Pow(15.0 / zg, 2.0 / alpha);

            if (h <= zg)
                return 2.41 * Math.Pow(h / zg, 2.0 / alpha);

            if (h <= 3280.0)
                return 2.41;

            throw new ArgumentOutOfRangeException(nameof(h), "h must be ≤ 3280 ft per ASCE 7-22.");
        }
    }
}

