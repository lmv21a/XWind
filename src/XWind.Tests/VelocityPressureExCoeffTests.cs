using FluentAssertions;
using Xunit;
using XWind.Asce722.Pressure;
using XWind.Core.Enums;

namespace XWind.Tests
{
    public class VelocityPressureExCoeffTests
    {
        private readonly VelocityPressureExCoeff _coeff = new();

        [Fact]
        public void Kz_ExposureC_At30ft_ShouldMatchExpected()
        {
            // Example: ASCE 7-22, Exposure C, z = 30 ft
            var result = _coeff.Kz(ExposureCategory.C, 30);

            // Expected ~0.98 (from Table 26.10-1 or formula check)
            result.Should().BeApproximately(0.98, 0.01);
        }

        [Fact]
        public void Kz_ExposureC_Below15ft_Uses15ftEquivalent()
        {
            // For z < 15 ft, should use z = 15 ft in the formula
            var below15 = _coeff.Kz(ExposureCategory.C, 5);
            var at15 = _coeff.Kz(ExposureCategory.C, 15);

            below15.Should().BeApproximately(at15, 1e-6);
        }

        [Fact]
        public void Kz_ExposureC_AtZg_ShouldMatchFormulaValue()
        {
            // Exposure C has zg = 2460 ft
            var result = _coeff.Kz(ExposureCategory.C, 2460);

            // Expected = 2.41 * (zg/zg)^(2/alpha) = 2.41 * 1 = 2.41
            result.Should().BeApproximately(2.41, 1e-6);
        }

        [Fact]
        public void Kz_ExposureC_AboveZg_ShouldClampTo241()
        {
            var result = _coeff.Kz(ExposureCategory.C, 3000);
            result.Should().BeApproximately(2.41, 1e-6);
        }

        [Fact]
        public void Kz_InvalidHeight_ShouldThrow()
        {
            Action act = () => _coeff.Kz(ExposureCategory.C, -10);

            act.Should().Throw<ArgumentException>()
               .WithMessage("*z must be > 0*");
        }

        [Fact]
        public void Kz_ExposureB_At30ft_ShouldMatchExpected()
        {
            var result = _coeff.Kz(ExposureCategory.B, 30);

            // Expected ~0.70 (per Table 26.10-1 note for Ch. 28, but formula gives slightly different value)
            result.Should().BeGreaterThan(0.6).And.BeLessThan(0.8);
        }

        [Fact]
        public void Kz_ExposureD_At15ft_ShouldMatchExpected()
        {
            var result = _coeff.Kz(ExposureCategory.D, 15);

            // From formula with alpha=11.5, zg=1935
            // Kz = 2.41 * (15/1935)^(2/11.5) ≈ ~1.035
            result.Should().BeApproximately(1.035, 0.02);
        }
    }
}
