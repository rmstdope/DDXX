using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock;

namespace Utility
{
    [TestFixture]
    class AspectRatioTest
    {
        [Test]
        public void RatioTest()
        {
            AspectRatio ratio1 = new AspectRatio(1280, 960);
            AspectRatio ratio2 = new AspectRatio(1280, 800);
            AspectRatio ratio3 = new AspectRatio(1280, 720);

            Assert.AreEqual(AspectRatio.Ratios.RATIO_4_3, ratio1.Ratio);
            Assert.AreEqual(AspectRatio.Ratios.RATIO_16_10, ratio2.Ratio);
            Assert.AreEqual(AspectRatio.Ratios.RATIO_16_9, ratio3.Ratio);
        }
    }
}
