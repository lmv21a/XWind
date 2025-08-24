using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XWind.Asce722.Parameters;
using XWind.Core.Enums;

namespace XWind.Tests
{
    public class GustEffectFactorTests
    {
        [Fact]
        public void GetG_Rigid_Returns085()
        {
            // Arrange
            var gef = new GustEffectFactor();

            // Act
            var g = gef.GetG(StructureFlexibility.Rigid);

            // Assert
            // Using precision to avoid floating point surprises.
            Assert.Equal(0.85, g, precision: 3);
        }

        [Fact]
        public void GetG_Flexible_ThrowsNotImplemented()
        {
            // Arrange
            var gef = new GustEffectFactor();

            // Act & Assert
            Assert.Throws<NotImplementedException>(() => gef.GetG(StructureFlexibility.Flexible));
        }
    }
}