using System;
using XWind.Asce722.Pressure;
using XWind.Core.Domain;
using XWind.Core.Enums;
using XWind.Core.Interfaces;

namespace XWind.Asce722.Procedures
{
    /// <summary>
    /// Implements ASCE 7-22 Chapter 27 Directional Procedure (Eq. 27.3-1).
    /// </summary>
    public class DirectionalProcedure : IWindLoadProcedure
    {
        public (double PosWindPressure, double NegWindPressure) CalculatePressure(WindInput Input, SurfaceType Surface)
        {

        }

        private double GetQValue(SurfaceType Surface, double qz_or_h)
        {
            double q;
            switch (Surface)
            {
                case SurfaceType.WindwardWall:
                    q = VelocityPressure.Qz();
            }
        }
    }
}
