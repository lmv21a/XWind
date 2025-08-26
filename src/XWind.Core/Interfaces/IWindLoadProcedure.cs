using XWind.Core.Domain;
using XWind.Core.Enums;

namespace XWind.Core.Interfaces
{
    public interface IWindLoadProcedure
    {
        public (double PosWindPressure, double NegWindPressure) CalculatePressure(WindInput input, SurfaceType surface);
    }
}
