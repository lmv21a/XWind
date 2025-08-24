using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XWind.Core.Enums;

namespace XWind.Core.Domain
{
    /// <summary>
    /// Inputs required for ASCE 7-22 Ch. 26 calculations.
    /// </summary>
    public class WindInput
    {
        /// <summary>Gets or sets the velocity</summary>
        /// <value>The v speed.</value>
        public double V_speed { get; set; }     // Basic wind speed (mph) — Sec. 26.5
        public double Kz { get; set; } = 0.85;      // Exposure coefficient — Table 26.10-1
        public double Kh { get; set; } = 0.85;      // Exposure coefficient — Table 26.10-1
        public double Kzt { get; set; } = 1;  // Topographic factor — Sec. 26.8
        public double Ke { get; set; } = 1;   // Ground elevation factor — Sec. 26.9

        /// <summary>The amount of elevation steps are samples we are taking from 0 to z = height above ground surface at the site of the building or other structure.</summary>
        public double ZSteps = 10;

        /// <summary>Gets or sets the exposure cateogry B, C, D</summary>
        /// <value>Returns an enum B, C, D</value>
        public ExposureCategory Exposure { get; set; } // "B", "C", or "D"
        /// <summary>Gets or sets the enclosure type, "Enclosed", "PartiallyEnclosed", "PartiallyOpen", "Open"</summary>
        public EnclosureType Enclosure { get; set; }

        public RiskCategory Risk { get; set; } //"I", "II", "III", "IV"
        /// <summary>Mean roof height, h (ft)</summary>
        public double MeanRoofHeight { get; set; }

        /// <summary>Building width, B (ft) — perpendicular to wind</summary>
        public double WidthB { get; set; }

        /// <summary>Building length, L (ft) — parallel to wind</summary>
        public double LengthL { get; set; }

        /// <summary>Roof slope in degrees</summary>
        public double RoofSlopeDegrees { get; set; }

        /// <summary>Roof type (Flat, Gable, Hip, Monoslope, etc.)</summary>
        public RoofType Roof { get; set; }


    }
}
