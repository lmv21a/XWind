using XWind.Asce722.Pressure;
using XWind.Core.Domain;
using XWind.Core.Enums;
using XWind.Core.Interfaces;

namespace XWind.Asce722.Procedures
{
    /// <summary>
    /// Implements ASCE 7-22 Chapter 27 Directional Procedure.
    /// Calculates MWFRS wind pressures using Eq. 27.3-1:
    /// p = q·Kd·G·Cp − qi·Kd·GCpi
    /// </summary>
    public class DirectionalProcedure : IWindLoadProcedure
    {
        // --- Basic ASCE 7-22 parameters (Ch. 26) ---
        private double V_speed { get; } // Basic wind speed, V
        private List<double> Kz { get; } // Exposure coefficients by height
        private double Kzt { get; } // Topographic factor
        private double Ke { get; } // Ground elevation factor
        private double Kh { get; } // Velocity pressure coefficient at mean roof height
        private double Kd { get; } // Directionality factor
        private double G { get; } // Gust-effect factor
        private double Cp { get; } // External pressure coefficient (Fig. 27.3-x)
        private (double PosGCpi, double NegGCpi) GCpi { get; } // Internal pressure coefficients (Table 26.13-1)
        private List<double> Qi { get; } // Velocity pressures for internal pressure evaluation
        private double Q { get; } // General velocity pressure
        private double L { get; } // Building length
        private double B { get; } // Building width
        private double angleDeg { get; } // Roof angle
        private double H { get; } // Building height
        private double HoverL { get; }
        private double PlanArea { get; } // Building plan area  

        /// <summary>
        /// Calculates wind pressures for the specified surface type (windward, leeward, sidewall).
        /// Internally selects the appropriate equation variant based on ASCE 7-22 Fig. 27.3-1.
        /// </summary>
        /// <param name="Input">All site and building input parameters (wind speed, exposure, height, etc.).</param>
        /// <param name="Surface">The building surface being evaluated (windward wall, leeward wall, sidewall wall, windward roof, etc.).</param>
        /// <returns>
        /// A list of calculated pressures. Each entry contains negative (suction) and positive (toward surface) values.
        /// Units: psf (lb/ft²) depending on input convention.
        /// </returns>
        public List<double> CalculatePressure(WindInput Input, SurfaceType Surface)
        {
            List<(double NegPressure, double PosPressure)> CalculatedPressure = null;

            double Qz;
            double Qh;

            switch (Surface)
            {
                case SurfaceType.WindwardWall:
                    CalculatedPressure = PWinwardWall(Qz, Cp);
                    break;
                case SurfaceType.LeewardWall:
                    var leeward = PLeewardWall(Qh, Cp);
                    CalculatedPressure = new List<double> { leeward.NegPressure, leeward.PosPressure };
                    break;
                case SurfaceType.SideWall:
                    var side = PSidewall(Qh, Cp);
                    CalculatedPressure = new List<double> { side.NegPressure, side.PosPressure };
                    break;
            }

            return CalculatedPressure;
        }

        /// <summary>
        /// Computes windward wall pressures using ASCE 7-22 Eq. 27.3-1.
        /// Evaluates both positive and negative internal pressure cases.
        /// </summary>
        /// <param name="Qz">Velocity pressure at height z (Eq. 26.10-1).</param>
        /// <param name="Cp">External pressure coefficient for windward wall (Fig. 27.3-1).</param>
        /// <returns>
        /// A list of (Negative, Positive) pressure pairs for each internal pressure case.
        /// </returns>
        private static List<(double NegPressure, double PosPressure)> PWinwardWall(double Qz, double Cp)
        {
            var myDirPro = new DirectionalProcedure();

            List<double> NegPs = new List<double>();
            List<double> PosPs = new List<double>();
            List<(double NegPressure, double PosPressure)> PWindWardWall = new List<(double NegPressure, double PosPressure)>();

            foreach (double qi in myDirPro.Qi)
            {
                // ASCE 7-22 Eq. 27.3-1
                NegPs.Add(Qz * myDirPro.Kd * myDirPro.G * Cp - qi * myDirPro.Kd * (myDirPro.GCpi.NegGCpi));
                PosPs.Add(Qz * myDirPro.Kd * myDirPro.G * Cp - qi * myDirPro.Kd * (myDirPro.GCpi.PosGCpi));
            }

            for (int i = 0; i < NegPs.Count; i++)
            {
                PWindWardWall.Add((NegPs[i], PosPs[i]));
            }

            return PWindWardWall;
        }

        /// <summary>
        /// Computes leeward wall pressures using ASCE 7-22 Eq. 27.3-1.
        /// Uses velocity pressure at mean roof height (qh).
        /// </summary>
        /// <param name="Qh">Velocity pressure at mean roof height h.</param>
        /// <param name="Cp">External pressure coefficient for leeward wall (Fig. 27.3-1).</param>
        /// <returns>A tuple of (Negative, Positive) pressures.</returns>
        private static (double NegPressure, double PosPressure) PLeewardWall(double Qh, double Cp)
        {
            var myDirPro = new DirectionalProcedure();
            double NegPressure = Qh * myDirPro.Kd * myDirPro.G * Cp - Qh * myDirPro.Kd * (myDirPro.GCpi.NegGCpi);
            double PosPressure = Qh * myDirPro.Kd * myDirPro.G * Cp - Qh * myDirPro.Kd * (myDirPro.GCpi.PosGCpi);

            return (NegPressure, PosPressure);
        }

        /// <summary>
        /// Computes sidewall pressures using ASCE 7-22 Eq. 27.3-1.
        /// Uses velocity pressure at mean roof height (qh).
        /// </summary>
        /// <param name="Qh">Velocity pressure at mean roof height h.</param>
        /// <param name="Cp">External pressure coefficient for sidewall (Fig. 27.3-1).</param>
        /// <returns>A tuple of (Negative, Positive) pressures.</returns>
        private static (double NegPressure, double PosPressure) PSidewall(double Qh, double Cp)
        {
            var myDirPro = new DirectionalProcedure();
            double NegPressure = Qh * myDirPro.Kd * myDirPro.G * Cp - Qh * myDirPro.Kd * (myDirPro.GCpi.NegGCpi);
            double PosPressure = Qh * myDirPro.Kd * myDirPro.G * Cp - Qh * myDirPro.Kd * (myDirPro.GCpi.PosGCpi);

            return (NegPressure, PosPressure);
        }

        /// <summary>
        /// Calculates windward roof pressures per ASCE 7-22 Eq. 27.3-1
        /// using Cp values from Fig. 27.3-1.
        /// </summary>
        /// <param name="Qh">Velocity pressure at mean roof height h.</param>
        /// <param name="CpWindwardRoof">
        /// Cp coefficients for windward roof zones.
        /// </param>
        /// <returns>
        /// List of (Negative, Positive) pressure pairs for each Cp zone.
        /// Negative = suction, Positive = toward surface.
        /// </returns>
        private static List<(double NegPresure, double PosPressure)> PWinwardRoof(double Qh, (double Cp1, double Cp2) CpWindwardRoof)
        {
            myDirPro = new DirectionalProcedure();
            CpWindwardRoof = GetWindwardRoofCp(myDirPro.L, myDirPro.H, myDirPro.angleDeg);

            double NegPressure1 = Qh * myDirPro.Kd * myDirPro.G * CpWindwardRoof.Cp1 - Qh * myDirPro.Kd * (myDirPro.GCpi.NegGCpi);

            double NegPressure2 = Qh * myDirPro.Kd * myDirPro.G * CpWindwardRoof.Cp2 - Qh * myDirPro.Kd * (myDirPro.GCpi.NegGCpi);

            double PosPressure1 = Qh * myDirPro.Kd * myDirPro.G * CpWindwardRoof.Cp1 - Qh * myDirPro.Kd * (myDirPro.GCpi.PosGCpi);

            double PosPressure2 = Qh * myDirPro.Kd * myDirPro.G * CpWindwardRoof.Cp2 - Qh * myDirPro.Kd * (myDirPro.GCpi.PosGCpi);

            return new List<(double NegPresure, double PosPressure)>
            {
                (NegPressure1, PosPressure1),
                (NegPressure2, PosPressure2)
            };
        }

        /// <summary>
        /// Calculates leeward roof pressures per ASCE 7-22 Eq. 27.3-1
        /// using Cp from Fig. 27.3-1.
        /// </summary>
        /// <param name="Qh">Velocity pressure at mean roof height h.</param>
        /// <param name="Cp">Cp coefficient for leeward roof.</param>
        /// <returns>(Negative, Positive) pressure pair for suction and toward surface.</returns>
        private static (double NegPressure, double PosPressure) PLeewardRoof(double Qh, double Cp)
        {
            myDirPro = new DirectionalProcedure();
            CpLeewardRoof = GetLeewardRoofCp(myDirPro.HoverL, myDirPro.angleDeg, myDirPro.PlanArea);
            double NegPressure = Qh * myDirPro.Kd * myDirPro.G * CpLeewardRoof - Qh * myDirPro.Kd * (myDirPro.GCpi.NegGCpi);
            double PosPressure = Qh * myDirPro.Kd * myDirPro.G * CpLeewardRoof - Qh * myDirPro.Kd * (myDirPro.GCpi.PosGCpi);
            PLeewardRoof = (NegPressure, PosPressure);
            return PLeewardRoof;
        }

        /// <summary>
        /// Computes design pressures for roof zones with wind parallel to the ridge
        /// using the ASCE 7-22 Directional Procedure (Eq. 27.3-1).
        /// </summary>
        /// <param name="Qh">Velocity pressure at mean roof height (qh).</param>
        /// <param name="Cp">
        /// External pressure coefficient (not used here; per-zone Cp1/Cp2 are used).
        /// Kept for signature/interface consistency.
        /// </param>
        /// <param name="CpParallelToRidge">
        /// Zone data containing Cp1/Cp2 (and optional reduction factor already applied)
        /// from the parallel-to-ridge tables.
        /// </param>
        /// <returns>
        /// A list of (Negative, Positive) pressure pairs for each zone and Cp value:
        /// one pair for Cp1 and one pair for Cp2 (order preserved).
        /// </returns>
        /// <remarks>
        /// Uses p = qh · Kd · G · Cp − qh · Kd · GCpi with both GCpi signs.
        /// </remarks>
        public List<(double NegPressure, double PosPressure)> PParallelToRidge(double Qh, double Cp,
            List<(string Zone, double Cp1, double Cp2, double? ReductionFactorCp1)> CpParallelToRidge)
        {
            myDirPro = new DirectionalProcedure();
            List<(double NegPressure, double PosPressure)> PParallelToRidge = new List<(double NegPressure, double PosPressure)>();
            foreach (var zone in CpParallelToRidge)
            {
                double NegPressure1 = Qh * myDirPro.Kd * myDirPro.G * zone.Cp1 - Qh * myDirPro.Kd * (myDirPro.GCpi.NegGCpi);
                double NegPressure2 = Qh * myDirPro.Kd * myDirPro.G * zone.Cp2 - Qh * myDirPro.Kd * (myDirPro.GCpi.NegGCpi);
                double PosPressure1 = Qh * myDirPro.Kd * myDirPro.G * zone.Cp1 - Qh * myDirPro.Kd * (myDirPro.GCpi.PosGCpi);
                double PosPressure2 = Qh * myDirPro.Kd * myDirPro.G * zone.Cp2 - Qh * myDirPro.Kd * (myDirPro.GCpi.PosGCpi);
                PParallelToRidge.Add((NegPressure1, PosPressure1));
                PParallelToRidge.Add((NegPressure2, PosPressure2));
            }
            return PParallelToRidge;
        }

    }
}
