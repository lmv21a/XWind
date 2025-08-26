using XWind.Core.Domain;
using XWind.Core.Enums;

namespace XWind.Tests
{
    public class RiskCategoryTests
    {
        [Fact]
        public void RiskCategory_ShouldStoreCorrectValues()
        {
            var input = new WindInput { Risk = RiskCategory.I };
            Assert.Equal(RiskCategory.I, input.Risk);
        }
    }
}
