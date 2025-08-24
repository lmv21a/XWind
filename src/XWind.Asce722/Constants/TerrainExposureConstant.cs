using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XWind.Core.Enums;

namespace XWind.Asce722.Constants
{
    public class TerrainExposureConstant
    {
        // Dictionary for each parameter
        private readonly Dictionary<ExposureCategory, double> alphaValues = new()
        {
            { ExposureCategory.B, 7.5 },
            { ExposureCategory.C, 9.8 },
            { ExposureCategory.D, 11.5 }
        };

        private readonly Dictionary<ExposureCategory, double> zgValues = new()
        {
            { ExposureCategory.B, 3280 },
            { ExposureCategory.C, 2460 },
            { ExposureCategory.D, 1935 }
        };

        private readonly Dictionary<ExposureCategory, double> alphaHatValues = new()
        {
            { ExposureCategory.B, 1.0/7.5 },
            { ExposureCategory.C, 1.0/9.8 },
            { ExposureCategory.D, 1.0/11.5 }
        };

        readonly Dictionary<ExposureCategory, double> bHatValues = new()
        {
            { ExposureCategory.B, 0.84 },
            { ExposureCategory.C, 1.00 },
            { ExposureCategory.D, 1.09 }
        };

        readonly Dictionary<ExposureCategory, double> aBarValues = new()
        {
            { ExposureCategory.B, 1.0/4.5 },
            { ExposureCategory.C, 1.0/6.4 },
            { ExposureCategory.D, 1.0/8.0 }
        };

        readonly Dictionary<ExposureCategory, double> bBarValues = new()
        {
            { ExposureCategory.B, 0.47 },
            { ExposureCategory.C, 0.66 },
            { ExposureCategory.D, 0.78 }
        };

        readonly Dictionary<ExposureCategory, double> cValues = new()
        {
            { ExposureCategory.B, 0.30 },
            { ExposureCategory.C, 0.20 },
            { ExposureCategory.D, 0.15 }
        };

        readonly Dictionary<ExposureCategory, double> lValues = new()
        {
            { ExposureCategory.B, 320 },
            { ExposureCategory.C, 500 },
            { ExposureCategory.D, 650 }
        };

        readonly Dictionary<ExposureCategory, double> eBarValues = new()
        {
            { ExposureCategory.B, 1.0/3.0 },
            { ExposureCategory.C, 1.0/5.0 },
            { ExposureCategory.D, 1.0/8.0 }
        };

        readonly Dictionary<ExposureCategory, double> zminValues = new()
        {
            { ExposureCategory.B, 30 },
            { ExposureCategory.C, 15 },
            { ExposureCategory.D, 7 }
        };

        // Public methods to access the values
        /// <summary>Gets the alpha, α = Mean hourly wind-speed power law exponent</summary>
        /// <param name="category">The category.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public double GetAlpha(ExposureCategory category) => alphaValues[category];
        /// <summary>Gets the zg = Nominal height of the atmospheric boundary layer used in this standard</summary>
        /// <param name="category">The category.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        /// <summary>
        /// Gets the nominal height of the atmospheric boundary layer, <c>z<sub>g</sub></c>.
        /// </summary>
        /// <param name="category">Exposure category (B, C, or D).</param>
        /// <returns>
        /// <c>z<sub>g</sub></c> [ft] — Nominal height of the atmospheric boundary layer 
        /// as listed in Table 26.11-1 (ASCE 7-22).
        /// </returns>
        public double GetZg(ExposureCategory category) => zgValues[category];

        /// <summary>
        /// Gets the reciprocal of the power law exponent, <c>α̂</c>.
        /// </summary>
        /// <param name="category">Exposure category (B, C, or D).</param>
        /// <returns>
        /// <c>α̂</c> = 1/α — Three-second gust speed power law exponent reciprocal 
        /// from Table 26.11-1.
        /// </returns>
        public double GetAlphaHat(ExposureCategory category) => alphaHatValues[category];

        /// <summary>
        /// Gets the three-second gust speed factor, <c>b̂</c>.
        /// </summary>
        /// <param name="category">Exposure category (B, C, or D).</param>
        /// <returns>
        /// <c>b̂</c> — Factor relating gust wind speed in Equation (26.11-16).
        /// </returns>
        public double GetBHat(ExposureCategory category) => bHatValues[category];

        /// <summary>
        /// Gets the mean hourly wind-speed power law coefficient, <c>ā</c>.
        /// </summary>
        /// <param name="category">Exposure category (B, C, or D).</param>
        /// <returns>
        /// <c>ā</c> = 1/n — Exponent for mean hourly wind-speed power law.
        /// </returns>
        public double GetABar(ExposureCategory category) => aBarValues[category];

        /// <summary>
        /// Gets the mean hourly wind speed factor, <c>b̄</c>.
        /// </summary>
        /// <param name="category">Exposure category (B, C, or D).</param>
        /// <returns>
        /// <c>b̄</c> — Coefficient used in mean hourly wind-speed formulation.
        /// </returns>
        public double GetBBar(ExposureCategory category) => bBarValues[category];

        /// <summary>
        /// Gets the turbulence intensity factor, <c>c</c>.
        /// </summary>
        /// <param name="category">Exposure category (B, C, or D).</param>
        /// <returns>
        /// <c>c</c> — Turbulence intensity constant in Equation (26.11-7).
        /// </returns>
        public double GetC(ExposureCategory category) => cValues[category];

        /// <summary>
        /// Gets the integral length scale factor, <c>l</c>.
        /// </summary>
        /// <param name="category">Exposure category (B, C, or D).</param>
        /// <returns>
        /// <c>l</c> [ft] — Integral length scale of turbulence from Table 26.11-1.
        /// </returns>
        public double GetL(ExposureCategory category) => lValues[category];

        /// <summary>
        /// Gets the integral length scale exponent, <c>ε</c>.
        /// </summary>
        /// <param name="category">Exposure category (B, C, or D).</param>
        /// <returns>
        /// <c>ε</c> — Power law exponent in Equation (26.11-9).
        /// </returns>
        public double GetEBar(ExposureCategory category) => eBarValues[category];

        /// <summary>
        /// Gets the minimum exposure height, <c>z<sub>min</sub></c>.
        /// </summary>
        /// <param name="category">Exposure category (B, C, or D).</param>
        /// <returns>
        /// <c>z<sub>min</sub></c> [ft] — Minimum height used to ensure equivalent height 
        /// is the greater of 0.6h or <c>z<sub>min</sub></c>.
        /// </returns>
        public double GetZmin(ExposureCategory category) => zminValues[category];

    }
}