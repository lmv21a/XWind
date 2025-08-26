// tests: src/XWind.Tests/VelocityPressureSeriesTests.cs
using FluentAssertions;
using XWind.Asce722.Pressure;

namespace XWind.Tests
{
    public class VelocityPressureSeriesTests
    {
        [Fact]
        public void Qz_SinglePoint_ShouldMatch_ExampleValue()
        {
            // Example: V = 115 mph, Kz ≈ 0.98 at z = 30 ft (Exposure C), Kzt = Ke = 1.0
            var kzSeries = new List<double> { 0.98 };

            var qzSeries = VelocityPressure.Qz(115, kzSeries, 1.0, 1.0);

            qzSeries.Should().ContainSingle();
            qzSeries.Single().Should().BeApproximately(33.18, 0.01); // psf
        }
    }
}
