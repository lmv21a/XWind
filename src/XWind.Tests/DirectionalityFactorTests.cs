using FluentAssertions;
using XWind.Asce722.Parameters;
using XWind.Core.Enums;

namespace XWind.Tests
{
    public class DirectionalityFactorTests
    {
        [Fact]
        public void GetKd_BuildingMwfrs_Returns085()
        {
            // Arrange
            var df = new DirectionalityFactor
            {
                Structure = StructureType.BuildingMwfrs
            };

            // Act
            var kd = df.GetKd();

            // Assert
            kd.Should().Be(0.85,
                "ASCE 7-22 Table 26.6-1 specifies Kd = 0.85 for buildings (MWFRS)");
        }

        [Fact]
        public void GetKd_UnsupportedType_ThrowsArgumentException()
        {
            // Arrange
            var df = new DirectionalityFactor
            {
                // Assume StructureType.None (or any enum you don’t handle in your switch)
                Structure = (StructureType)999 // fake unsupported value
            };

            // Act
            Action act = () => df.GetKd();

            // Assert
            act.Should()
               .Throw<ArgumentException>()
               .WithMessage("*Unsupported structure type*");
        }
    }
}
