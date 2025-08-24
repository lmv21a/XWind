using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XWind.Asce722.Pressure
{
    public static class VelocityPressure
    {

        /// <summary>
        /// ASCE 7-22 Eq. 26.10-1
        /// qz = 0.00256 * Kz * Kzt * Ke * V^2  [psf]
        /// </summary>
        public static List<double> Qz(double V_speed, List<double> Kz, double Kzt, double Ke)
        {
            List<double> qzValues = new List<double>();
            for (int i = 0; i <= Kz.Count; i = i++)
            {
                qzValues.Add(0.00256 * Kz[i] * Kzt * Ke * V_speed * V_speed);
            }
            return qzValues;
        }
        public static double Qh(double V_speed, double Kh, double Kzt, double Ke)
            => 0.00256 * Kh * Kzt * Ke * V_speed * V_speed;


    }
}
