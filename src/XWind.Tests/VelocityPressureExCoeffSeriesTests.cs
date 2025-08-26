using FluentAssertions;
using XWind.Asce722.Pressure;
using XWind.Core.Enums;

namespace XWind.Tests
{
    public class VelocityPressureExCoeffSeriesTests
    {
        [Fact]
        public void KzSeries_ExposureC_At30ft_LastPointIsExpected()
        {
            var coeff = new VelocityPressureExCoeff();
            var series = coeff.KzSeries(ExposureCategory.C, zStart: 15, zEnd: 30, zStep: 15);

            series.Should().ContainSingle(p => Math.Abs(p.z - 30.0) < 1e-9);
            var k30 = series.Single(p => Math.Abs(p.z - 30.0) < 1e-9).Kz;

            k30.Should().BeApproximately(0.98, 0.02); // ≈0.98 at 30 ft, Exposure C
        }

    }
}