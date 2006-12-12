using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class NaturalCubicSplineTest : SplineBaseTest
    {
        private NaturalCubicSpline<InterpolatedFloat> spline = new NaturalCubicSpline<InterpolatedFloat>();

        [Test]
        public void TestGetValues()
        {
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(0.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(2.0f, new InterpolatedFloat(100.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(3.0f, new InterpolatedFloat(200.0f)));
            spline.Calculate();

            Assert.AreEqual(0.0f, (float)spline.GetValue(1.0f));
            Assert.AreEqual(100.0f, (float)spline.GetValue(2.0f));
            Assert.AreEqual(200.0f, (float)spline.GetValue(3.0f));
        }
    }
}
