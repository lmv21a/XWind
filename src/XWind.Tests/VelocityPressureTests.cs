using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XWind.Asce722.Pressure;
using FluentAssertions;
using Xunit;

namespace XWind.Tests
{
    public class VelocityPressureTests
    {
        [Fact]
        public void Qz_ShouldMatch_ExampleValues()
        {
            // Example: V = 115 mph, Kz ≈ 0.98 at z = 30 ft (Exposure C), Kzt = Ke = 1.0
            var qz = VelocityPressure.Qz(115, 0.98, 1.0, 1.0);

            qz.Should().BeApproximately(33.18, 0.01); // psf
        }
    }
}
