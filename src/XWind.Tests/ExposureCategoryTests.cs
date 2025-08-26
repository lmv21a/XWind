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
