using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
