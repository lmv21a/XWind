using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XWind.Core.Domain;
using XWind.Core.Enums;

namespace XWind.Tests
{
    public class EnclosureTypeTests
    {
        [Fact]
        public void EnclosureType_ShouldStoreCorrectValue()
        {
            var input = new WindInput { Enclosure = EnclosureType.Enclosed };
            Assert.Equal(EnclosureType.Enclosed, input.Enclosure);
        }
    }
}