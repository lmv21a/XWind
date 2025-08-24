using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XWind.Core.Enums;

namespace XWind.Asce722.Pressure
{
    public class InternalPressureCoeff
    {
        /// <summary>Gets the Internal pressure coefficients (GCpi) shall be determined from ASCE 7-22 Table 26.13-1 based on building enclosure classifications determined from ASCE 7-22 Section 26.12.</summary>
        /// <param name="Enclosure">
        /// Enclosure classification is about how openings in walls/roofs affect internal pressures. Possible exposure classifications include enclosed, partially enclosed, partially open and open.
        /// </param>
        /// <returns>Returns a tuple containing internal pressure coefficients (+GCpi, -GCpi)<br /></returns>
        /// <exception cref="System.ArgumentOutOfRangeException">Enclosure - Unsupported enclosure type: {Enclosure}</exception>
        public (double PosGCpi, double NegGCpi) GetGCpi(EnclosureType Enclosure)
        {
            switch (Enclosure)
            {
                case EnclosureType.Enclosed:
                    return (0.18, -0.18);
                case EnclosureType.PartiallyEnclosed:
                    return (0.55, -0.55);
                case EnclosureType.PartiallyOpen:
                    return (0.18, -0.18);
                case EnclosureType.Open:
                    return (0, 0);
                default:
                    throw new ArgumentOutOfRangeException(nameof(Enclosure), $"Unsupported enclosure type: {Enclosure}");
            }
        }
    }
}
