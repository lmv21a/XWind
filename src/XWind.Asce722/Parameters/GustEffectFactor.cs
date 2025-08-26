using XWind.Core.Enums;

namespace XWind.Asce722.Parameters
{
    public class GustEffectFactor
    {
        public double GetG(StructureFlexibility Flexibility)
        {
            switch (Flexibility)
            {
                case StructureFlexibility.Rigid:
                    return 0.85;
                case StructureFlexibility.Flexible:
                    throw new NotImplementedException("Flexible building type not yet implemented.");
                default:
                    throw new NotImplementedException($"{nameof(Flexibility)}, Non-Implemented structure flexibility: {Flexibility}");
            }
        }
    }
}
