using System;
using FluentAssertions;
using Xunit;
using XWind.Asce722.Constants;
using XWind.Core.Enums;

namespace XWind.Tests
{
    public class TerrainExposureConstantTests
    {
        private readonly TerrainExposureConstant _constants = new();

        [Fact]
        public void GetZg_ReturnsExpectedValues()
        {
            _constants.GetZg(ExposureCategory.B).Should().Be(3280);
            _constants.GetZg(ExposureCategory.C).Should().Be(2460);
            _constants.GetZg(ExposureCategory.D).Should().Be(1935);
        }

        [Fact]
        public void GetAlpha_ReturnsExpectedValues()
        {
            _constants.GetAlpha(ExposureCategory.B).Should().Be(7.5);
            _constants.GetAlpha(ExposureCategory.C).Should().Be(9.8);
            _constants.GetAlpha(ExposureCategory.D).Should().Be(11.5);
        }

        [Fact]
        public void GetAlphaHat_ReturnsExpectedValues()
        {
            _constants.GetAlphaHat(ExposureCategory.B).Should().BeApproximately(1.0 / 7.5, 1e-6);
            _constants.GetAlphaHat(ExposureCategory.C).Should().BeApproximately(1.0 / 9.8, 1e-6);
            _constants.GetAlphaHat(ExposureCategory.D).Should().BeApproximately(1.0 / 11.5, 1e-6);
        }

        [Fact]
        public void GetBHat_ReturnsExpectedValues()
        {
            _constants.GetBHat(ExposureCategory.B).Should().Be(0.84);
            _constants.GetBHat(ExposureCategory.C).Should().Be(1.00);
            _constants.GetBHat(ExposureCategory.D).Should().Be(1.09);
        }

        [Fact]
        public void GetABar_ReturnsExpectedValues()
        {
            _constants.GetABar(ExposureCategory.B).Should().BeApproximately(1.0 / 4.5, 1e-6);
            _constants.GetABar(ExposureCategory.C).Should().BeApproximately(1.0 / 6.4, 1e-6);
            _constants.GetABar(ExposureCategory.D).Should().BeApproximately(1.0 / 8.0, 1e-6);
        }

        [Fact]
        public void GetBBar_ReturnsExpectedValues()
        {
            _constants.GetBBar(ExposureCategory.B).Should().Be(0.47);
            _constants.GetBBar(ExposureCategory.C).Should().Be(0.66);
            _constants.GetBBar(ExposureCategory.D).Should().Be(0.78);
        }

        [Fact]
        public void GetC_ReturnsExpectedValues()
        {
            _constants.GetC(ExposureCategory.B).Should().Be(0.30);
            _constants.GetC(ExposureCategory.C).Should().Be(0.20);
            _constants.GetC(ExposureCategory.D).Should().Be(0.15);
        }

        [Fact]
        public void GetL_ReturnsExpectedValues()
        {
            _constants.GetL(ExposureCategory.B).Should().Be(320);
            _constants.GetL(ExposureCategory.C).Should().Be(500);
            _constants.GetL(ExposureCategory.D).Should().Be(650);
        }

        [Fact]
        public void GetEBar_ReturnsExpectedValues()
        {
            _constants.GetEBar(ExposureCategory.B).Should().BeApproximately(1.0 / 3.0, 1e-6);
            _constants.GetEBar(ExposureCategory.C).Should().BeApproximately(1.0 / 5.0, 1e-6);
            _constants.GetEBar(ExposureCategory.D).Should().BeApproximately(1.0 / 8.0, 1e-6);
        }

        [Fact]
        public void GetZmin_ReturnsExpectedValues()
        {
            _constants.GetZmin(ExposureCategory.B).Should().Be(30);
            _constants.GetZmin(ExposureCategory.C).Should().Be(15);
            _constants.GetZmin(ExposureCategory.D).Should().Be(7);
        }
    }
}