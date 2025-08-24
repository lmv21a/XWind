using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XWind.Core.Enums;

namespace XWind.Asce722.Parameters
{
    /// <summary>
    /// Calculates topographic factors for wind load analysis according to ASCE 7-22
    /// </summary>
    public class TopographicFactor
    {
        /// <summary>
        /// Gets or sets the topography type (ridge, valley, escarpment, etc.)
        /// </summary>
        public TopographyType Type { get; set; }

        /// <summary>Gets or sets the crest position.</summary>
        public CrestPosition Position { get; set; }

        /// <summary>
        /// Gets or sets the exposure category (B, C, or D)
        /// </summary>
        public ExposureCategory Exposure { get; set; }

        /// <summary>
        /// Calculates the topographic factor Kzt
        /// </summary>
        /// <returns>The topographic factor value</returns>
        public double GetKzt(double K1, double K2, double K3)
        {
            switch (Type)
            {
                case TopographyType.None:
                    return 1.0;
                case TopographyType.RidgeOrValley:
                    return CalcKzt(K1, K2, K3);
                case TopographyType.Escarpment:
                    return CalcKzt(K1, K2, K3);
                case TopographyType.AxisymmetricalHill:
                    return CalcKzt(K1, K2, K3);
                default:
                    throw new ArgumentOutOfRangeException(nameof(Type), $"Unsupported topography type: {Type}");
            }
        }

        /// <summary>
        /// Calculates the K1 factor based on topography type and exposure category
        /// </summary>
        /// <param name="Type">The topography type (ridge, valley, escarpment, hill)</param>
        /// <param name="Exposure">The exposure category (B, C, or D)</param>
        /// <param name="H">The height of the topographic feature (must be > 0)</param>
        /// <param name="Lh">The horizontal distance of the topographic feature (must be > 0)</param>
        /// <returns>The calculated K1 factor value</returns>
        /// <exception cref="ArgumentException">Thrown when H or Lh is less than or equal to 0</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when Type or Exposure contains unsupported values</exception>
        public static double GetK1(TopographyType Type, ExposureCategory Exposure, double H, double Lh)
        {
            if (H <= 0) throw new ArgumentException("H must be greater than 0", nameof(H));
            if (Lh <= 0) throw new ArgumentException("Lh must be greater than 0", nameof(Lh));

            if (Type == TopographyType.None)
            {
                return 1.0;
            }
            else if (Type == TopographyType.RidgeOrValley)
            {
                switch (Exposure)
                {
                    case ExposureCategory.B:
                        return (H / Lh) * 1.30;
                    case ExposureCategory.C:
                        return (H / Lh) * 1.45;
                    case ExposureCategory.D:
                        return (H / Lh) * 1.55;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Exposure), $"Unsupported exposure category: {Exposure}");
                }
            }
            else if (Type == TopographyType.Escarpment)
            {
                switch (Exposure)
                {
                    case ExposureCategory.B:
                        return (H / Lh) * 0.75;
                    case ExposureCategory.C:
                        return (H / Lh) * 0.85;
                    case ExposureCategory.D:
                        return (H / Lh) * 0.95;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Exposure), $"Unsupported exposure category: {Exposure}");
                }
            }
            else if (Type == TopographyType.AxisymmetricalHill)
            {
                switch (Exposure)
                {
                    case ExposureCategory.B:
                        return (H / Lh) * 0.95;
                    case ExposureCategory.C:
                        return (H / Lh) * 1.05;
                    case ExposureCategory.D:
                        return (H / Lh) * 1.15;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Exposure), $"Unsupported exposure category: {Exposure}");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(Type), $"Unsupported topography type: {Type}");
            }
        }
        public static double GetK2(TopographyType Type, CrestPosition Position, double Lh, double x)
        {
            if (x <= 0) throw new ArgumentException("x must be greater than 0", nameof(x));
            if (Lh <= 0) throw new ArgumentException("Lh must be greater than 0", nameof(Lh));
            double mu;

            if (Type == TopographyType.None)
            {
                return 1.0;
            }
            else if (Type == TopographyType.RidgeOrValley)
            {
                switch (Position)
                {
                    case CrestPosition.UpwindOfCrest:
                        mu = 1.5;
                        return K2Equals(x, Lh, mu);
                    case CrestPosition.DownwindOfCrest:
                        mu = 1.5;
                        return K2Equals(x, Lh, mu);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Position), $"Unsupported crest position: {Position}");
                }
            }
            else if (Type == TopographyType.Escarpment)
            {
                switch (Position)
                {
                    case CrestPosition.UpwindOfCrest:
                        mu = 1.5;
                        return K2Equals(x, Lh, mu);
                    case CrestPosition.DownwindOfCrest:
                        mu = 4;
                        return K2Equals(x, Lh, mu);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Position), $"Unsupported crest position: {Position}");
                }
            }
            else if (Type == TopographyType.AxisymmetricalHill)
            {
                switch (Position)
                {
                    case CrestPosition.UpwindOfCrest:
                        mu = 1.5;
                        return K2Equals(x, Lh, mu);
                    case CrestPosition.DownwindOfCrest:
                        mu = 1.5;
                        return K2Equals(x, Lh, mu);
                    default:
                        throw new ArgumentOutOfRangeException(nameof(Position), $"Unsupported crest position: {Position}");
                }
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(Type), $"Unsupported topography type: {Type}");
            }

        }

        public static double GetK3(TopographyType Type, double z, double Lh)
        {
            if (z <= 0) throw new ArgumentException("z must be greater than 0", nameof(z));
            if (Lh <= 0) throw new ArgumentException("Lh must be greater than 0", nameof(Lh));
            double gamma;

            if (Type == TopographyType.None)
            {
                return 1.0;
            }
            else if (Type == TopographyType.RidgeOrValley)
            {
                gamma = 3;
                return K3Equals(z, Lh, gamma);
            }
            else if (Type == TopographyType.Escarpment)
            {
                gamma = 2.5;
                return K3Equals(z, Lh, gamma);
            }
            else if (Type == TopographyType.AxisymmetricalHill)
            {
                gamma = 4;
                return K3Equals(z, Lh, gamma);
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(Type), $"Unsupported topography type: {Type}");
            }

        }
        private static double K2Equals(double x, double Lh, double mu)
        {
            return Math.Max(1 - (Math.Abs(x) / (mu * Lh)), 0);
        }

        private static double K3Equals(double z, double Lh, double gamma)
        {
            return Math.Exp((-gamma * z) / Lh);
        }

        private static double CalcKzt(double K1, double K2, double K3)
        {
            return Math.Pow(1 + (K1 * K2 * K3), 2);
        }

    }
}