using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XWind.Core.Enums
{
    public enum SurfaceType
    {
        // Walls
        WindwardWall,
        LeewardWall,
        SideWall,

        // Flat Roof Zones
        RoofFlatZone1,
        RoofFlatZone2,
        RoofFlatZone3,

        // Gable Roof Zones
        RoofGableWindwardZone1,
        RoofGableWindwardZone2,
        RoofGableWindwardZone3,
        RoofGableLeewardZone1,
        RoofGableLeewardZone2,
        RoofGableLeewardZone3,
        RoofGableRidgeZone,

        // Hip Roof Zones
        RoofHipZone1,
        RoofHipZone2,
        RoofHipZone3,

        // Monoslope Roof
        RoofMonoslopeWindward,
        RoofMonoslopeLeeward,

        // Special Roofs
        RoofMansard,
        RoofArched,
        RoofDome,

        // Appurtenances
        Parapet,
        RoofOverhangEdge,
        RoofOverhangCorner
    }

}
