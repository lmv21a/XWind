using FluentAssertions;
using XWind.Asce722.Pressure;
using XWind.Core.Enums;

namespace XWind.Tests
{
    public class InternalPressureCoeffTests
    {
        private readonly InternalPressureCoeff _coeff = new();

        [Fact]
        public void GetGCpi_Enclosed_ReturnsPoint18AndNegPoint18()
        {
            var result = _coeff.GetGCpi(EnclosureType.Enclosed);

            result.PosGCpi.Should().BeApproximately(0.18, 1e-6);
            result.NegGCpi.Should().BeApproximately(-0.18, 1e-6);
        }

        [Fact]
        public void GetGCpi_PartiallyEnclosed_ReturnsPoint55AndNegPoint55()
        {
            var result = _coeff.GetGCpi(EnclosureType.PartiallyEnclosed);

            result.PosGCpi.Should().BeApproximately(0.55, 1e-6);
            result.NegGCpi.Should().BeApproximately(-0.55, 1e-6);
        }

        [Fact]
        public void GetGCpi_PartiallyOpen_ReturnsPoint18AndNegPoint18()
        {
            var result = _coeff.GetGCpi(EnclosureType.PartiallyOpen);

            result.PosGCpi.Should().BeApproximately(0.18, 1e-6);
            result.NegGCpi.Should().BeApproximately(-0.18, 1e-6);
        }

        [Fact]
        public void GetGCpi_Open_ReturnsZeroAndZero()
        {
            var result = _coeff.GetGCpi(EnclosureType.Open);

            result.PosGCpi.Should().Be(0.0);
            result.NegGCpi.Should().Be(0.0);
        }

        [Fact]
        public void GetGCpi_InvalidEnumValue_Throws()
        {
            var invalidValue = (EnclosureType)999;
            Action act = () => _coeff.GetGCpi(invalidValue);

            act.Should().Throw<ArgumentOutOfRangeException>()
               .WithMessage("*Unsupported enclosure type*");
        }
    }
}
