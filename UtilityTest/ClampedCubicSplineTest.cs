using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class ClampedCubicSplineTest : SplineBaseTest
    {
        private ClampedCubicSpline<InterpolatedFloat> spline;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            spline = new ClampedCubicSpline<InterpolatedFloat>(new InterpolatedFloat(1.0f), new InterpolatedFloat(2.0f));
        }

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

        [Test]
        public void TestGetDerivative()
        {
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(0.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(2.0f, new InterpolatedFloat(100.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(3.0f, new InterpolatedFloat(200.0f)));
            spline.Calculate();

            Assert.AreEqual(1.0f, (float)spline.GetDerivative(1.0f));
            Assert.AreEqual(2.0f, (float)spline.GetDerivative(3.0f));
        }
    }
}
