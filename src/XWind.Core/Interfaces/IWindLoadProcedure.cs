using XWind.Core.Domain;
using XWind.Core.Enums;

namespace XWind.Core.Interfaces
{
    public interface IWindLoadProcedure
    {
        public static List<(double NegPressure, double PosPressure)> CalculatePressure(WindInput input, SurfaceType surface);
    }
}
