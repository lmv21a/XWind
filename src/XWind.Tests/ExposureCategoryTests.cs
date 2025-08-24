using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XWind.Core.Domain;
using XWind.Core.Enums;

namespace XWind.Tests
{
    public class ExposureCategoryTests
    {
        [Fact]
        public void ExposureCategory_ShouldStoreCorrectValue()
        {
            var input = new WindInput { Exposure = ExposureCategory.C };
            Assert.Equal(ExposureCategory.C, input.Exposure);
        }
    }
}
