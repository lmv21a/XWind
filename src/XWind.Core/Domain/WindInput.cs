using XWind.Core.Enums;

namespace XWind.Core.Domain
{
    /// <summary>
    /// User inputs for ASCE 7‑22 wind analysis.
    /// Units: V_speed [mph], lengths [ft], angles [degrees].
    /// </summary>
    public class WindInput
    {
        /// <summary>Basic wind speed V [mph]. Sec. 26.5.</summary>
        public double V_speed { get; set; }

        /// <summary>Exposure category (B/C/D). Sec. 26.7.</summary>
        public ExposureCategory Exposure { get; set; }

        /// <summary>Enclosure classification. Sec. 26.12, Table 26.13‑1.</summary>
        public EnclosureType Enclosure { get; set; }

        /// <summary>Risk category (I–IV). Ch. 1.</summary>
        public RiskCategory Risk { get; set; }

        /// <summary>Mean roof height h [ft].</summary>
        public double MeanRoofHeight { get; set; }

        /// <summary>Building width B [ft] (perpendicular to wind).</summary>
        public double WidthB { get; set; }

        /// <summary>Building length L [ft] (parallel to wind).</summary>
        public double LengthL { get; set; }

        /// <summary>Roof slope [deg].</summary>
        public double RoofSlopeDegrees { get; set; }

        /// <summary>Roof type (flat/gable/hip/...)</summary>
        public RoofType Roof { get; set; }

        /// <summary>Optional topographic override. If null, compute later; else use given value.</summary>
        public double? KztOverride { get; set; }

        /// <summary>Optional ground elevation factor override Ke. If null, compute or default to 1.0.</summary>
        public double? KeOverride { get; set; }

        /// <summary>Validate ranges & required fields; throws ArgumentException on invalid input.</summary>
        public void Validate()
        {
            if (V_speed <= 0) throw new ArgumentException("V_speed must be > 0 mph.");
            if (MeanRoofHeight <= 0) throw new ArgumentException("MeanRoofHeight must be > 0 ft.");
            if (WidthB <= 0) throw new ArgumentException("WidthB must be > 0 ft.");
            if (LengthL <= 0) throw new ArgumentException("LengthL must be > 0 ft.");
            if (RoofSlopeDegrees < 0 || RoofSlopeDegrees > 90) throw new ArgumentException("RoofSlopeDegrees must be between 0 and 90.");
        }
    }
}