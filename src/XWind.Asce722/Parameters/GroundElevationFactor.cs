using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XWind.Asce722.Parameters
{
    /// <summary>This class is for ground elevation factor, Ke which adjusts wind pressure based on site elevation above sea level. Refer to ASCE 7-22 26.9.</summary>
    public class GroundElevationFactor
    {
        /// <summary>Gets the Ke. This adjusts wind pressure based on site elevation above sea level. Permitted to be taken as 1.0 for all values of Ze.</summary>
        /// <param name="Ze">ze= ground elevation above sea level.</param>
        /// <returns>
        ///   <br />
        /// </returns>
        public static double GetKe(double Ze)
        {
            return 1.0;
        }
    }
}
