using MathNet.Numerics.Interpolation;
using XWind.Core.Enums;

namespace XWind.Asce722.Pressure
{
    public class WallPressureCoeff
    {
        // Static interpolator - created once and reused
        private static readonly IInterpolation WallLeewardInterp =
            LinearSpline.InterpolateSorted(new[] { 1.0, 2.0, 4.0 }, new[] { -0.5, -0.3, -0.2 });

        public static double GetCp(double LengthL, double WidthB, SurfaceType Surface)
        {


            double L_B = LengthL / WidthB;
            double Cp;
            if (L_B <= 0) throw new ArgumentException("L/B must be > 0.", nameof(LengthL));

            if (Surface == SurfaceType.WindwardWall)
            {
                Cp = 0.8;
            }
            else if (Surface == SurfaceType.LeewardWall)
            {
                if (L_B <= 1)
                {
                    Cp = 0.8;
                }
                else if (L_B <= 4)
                {
                    Cp = WallLeewardInterp.Interpolate(L_B);
                }
                else // L_B > 4
                {
                    Cp = -0.2;
                }
            }
            else if (Surface == SurfaceType.SideWall)
            {
                Cp = -0.7;
            }
            else if (Surface == SurfaceType.Parapet)
            {
                throw new NotImplementedException("Cp for parapets is not valid. See ASCE 7-22 Section 27.3.4 for GCpn");
            }
            else
            {
                throw new NotImplementedException();
            }

            return Cp;
        }
    }
}