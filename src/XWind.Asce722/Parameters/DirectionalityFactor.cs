using XWind.Core.Enums;

namespace XWind.Asce722.Parameters
{
    public class DirectionalityFactor
    {
        public StructureType Structure { get; set; }
        private readonly List<double> DirectionalityFactors = new List<double> { 0.85, 0.9, 0.95, 1 };
        /// <summary>Gets the wind directionality factor. As determined from ASCE 7-22, Table 26.6-1.</summary>
        /// <returns>Returns a double from a list of Directionality Factors</returns>
        /// <exception cref="System.ArgumentException">Unsupported structure type</exception>
        public double GetKd()
        {
            switch (Structure)
            {
                case StructureType.BuildingMwfrs:
                    return DirectionalityFactors[0];
                case StructureType.BuildingCandC:
                    return DirectionalityFactors[0];
                case StructureType.ArchedRoofs:
                    return DirectionalityFactors[0];
                case StructureType.CircularDomes:
                    return DirectionalityFactors[3];
                case StructureType.CircularDomeNonAxisymmetricStrSys:
                    return DirectionalityFactors[2];
                case StructureType.ChimneyTanksOrSimSquare:
                    return DirectionalityFactors[1];
                case StructureType.ChimneyTanksOrSimHexagonal:
                    return DirectionalityFactors[2];
                case StructureType.ChimneyTanksOrSimOctagonal:
                    return DirectionalityFactors[3];
                case StructureType.ChimneyTanksOrSimOctagonalNonAxisymmetricStrSys:
                    return DirectionalityFactors[2];
                case StructureType.ChimneyTanksOrSimRound:
                    return DirectionalityFactors[3];
                case StructureType.ChimneyTanksOrSimRoundNonAxisymmetricStrSys:
                    return DirectionalityFactors[2];
                case StructureType.SolidFreestandingWallsRooftopEquipmentSigns:
                    return DirectionalityFactors[0];
                case StructureType.OpenSignsSinglePlaneFrames:
                    return DirectionalityFactors[0];
                case StructureType.TrussedTowersTriangularSquareRectangular:
                    return DirectionalityFactors[0];
                case StructureType.TrussedTowersAllOtherCrossSections:
                    return DirectionalityFactors[2];
                default:
                    throw new ArgumentException("Unsupported structure type"); ;
            }
        }
    }
}
