using System;
using FluentAssertions;
using Xunit;
using XWind.Asce722.Parameters;
using XWind.Core.Enums;

namespace XWind.Tests
{
    public class TopographicFactorTests
    {
        [Fact]
        public void GetKzt_None_ReturnsOne()
        {
            var tf = new TopographicFactor { Type = TopographyType.None };
            tf.GetKzt(0.5, 0.5, 0.5).Should().Be(1.0);
        }

        [Fact]
        public void GetKzt_RidgeOrValley_ComputesCorrectly()
        {
            var tf = new TopographicFactor { Type = TopographyType.RidgeOrValley };
            var result = tf.GetKzt(0.5, 0.8, 1.0);
            result.Should().BeApproximately(Math.Pow(1 + (0.5 * 0.8 * 1.0), 2), 1e-6);
        }

        [Fact]
        public void GetK1_RidgeOrValley_ReturnsExpected()
        {
            var result = TopographicFactor.GetK1(TopographyType.RidgeOrValley, ExposureCategory.B, 100, 1000);
            result.Should().BeApproximately((100.0 / 1000.0) * 1.30, 1e-6);
        }

        [Fact]
        public void GetK1_InvalidHeight_Throws()
        {
            Action act = () => TopographicFactor.GetK1(TopographyType.RidgeOrValley, ExposureCategory.B, 0, 100);
            act.Should().Throw<ArgumentException>().WithMessage("*H must be greater than 0*");
        }

        [Fact]
        public void GetK2_RidgeOrValley_Upwind_ComputesCorrectly()
        {
            var result = TopographicFactor.GetK2(TopographyType.RidgeOrValley, CrestPosition.UpwindOfCrest, 1000, 500);
            result.Should().BeApproximately(1 - (500.0 / (1.5 * 1000.0)), 1e-6);
        }

        [Fact]
        public void GetK2_InvalidInputs_Throws()
        {
            Action act = () => TopographicFactor.GetK2(TopographyType.RidgeOrValley, CrestPosition.UpwindOfCrest, 0, 100);
            act.Should().Throw<ArgumentException>().WithMessage("*Lh must be greater than 0*");
        }

        [Fact]
        public void GetK3_RidgeOrValley_ComputesCorrectly()
        {
            var result = TopographicFactor.GetK3(TopographyType.RidgeOrValley, 50, 1000);
            var expected = Math.Exp((-3.0 * 50.0) / 1000.0); // gamma=3 for ridge
            result.Should().BeApproximately(expected, 1e-6);
        }

        [Fact]
        public void GetK3_InvalidInputs_Throws()
        {
            Action act = () => TopographicFactor.GetK3(TopographyType.RidgeOrValley, -10, 1000);
            act.Should().Throw<ArgumentException>().WithMessage("*z must be greater than 0*");
        }

        [Fact]
        public void GetKzt_WithUnsupportedType_Throws()
        {
            var tf = new TopographicFactor { Type = (TopographyType)999 };
            Action act = () => tf.GetKzt(1, 1, 1);
            act.Should().Throw<ArgumentOutOfRangeException>().WithMessage("*Unsupported topography type*");
        }
    }
}
