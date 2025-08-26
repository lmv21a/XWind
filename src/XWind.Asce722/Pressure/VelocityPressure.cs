namespace XWind.Asce722.Pressure
{
    public static class VelocityPressure
    {
        private const double Coef = 0.00256; // psf per (mph)^2, ASCE 7-22 Eq. 26.10-1

        /// <summary>
        /// ASCE 7-22 Eq. 26.10-1
        /// qz = 0.00256 * Kz * Kzt * Ke * V^2  [psf]
        /// </summary>
        public static List<double> Qz(double V_speed, List<double> Kz, double Kzt, double Ke)
        {
            ArgumentNullException.ThrowIfNull(Kz);

            var qzValues = new List<double>(Kz.Count);
            double v2 = V_speed * V_speed;

            for (int i = 0; i < Kz.Count; i++)
            {
                qzValues.Add(Coef * Kz[i] * Kzt * Ke * v2);
            }
            return qzValues;
        }

        public static double Qh(double V_speed, double Kh, double Kzt, double Ke)
            => Coef * Kh * Kzt * Ke * V_speed * V_speed;
    }
}
