namespace XWind.Core.Enums
{
    /// <summary>
    /// Defines possible structure types for structure as defined in ASCE 7-22 (for use with Directionality Factor)
    /// </summary>
    public enum StructureType
    {
        BuildingMwfrs,
        BuildingCandC,
        ArchedRoofs,
        CircularDomes,
        CircularDomeNonAxisymmetricStrSys,
        ChimneyTanksOrSimSquare,
        ChimneyTanksOrSimHexagonal,
        ChimneyTanksOrSimOctagonal,
        ChimneyTanksOrSimOctagonalNonAxisymmetricStrSys,
        ChimneyTanksOrSimRound,
        ChimneyTanksOrSimRoundNonAxisymmetricStrSys,
        SolidFreestandingWallsRooftopEquipmentSigns,
        OpenSignsSinglePlaneFrames,
        TrussedTowersTriangularSquareRectangular,
        TrussedTowersAllOtherCrossSections
    }
}