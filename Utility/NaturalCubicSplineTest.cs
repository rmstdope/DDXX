using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class NaturalCubicSplineTest : SplineBaseTest
    {
        private NaturalCubicSpline<InterpolatedFloat> spline;
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            spline = new NaturalCubicSpline<InterpolatedFloat>();
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
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(2.0f, new InterpolatedFloat(140.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(3.0f, new InterpolatedFloat(-100.0f)));
            spline.Calculate();

            // Second derivative is zero
            // Approximate second derivative
            float epsilon = 0.0001f;
            float p1 = (float)spline.GetDerivative(1.0f);
            float p2 = (float)spline.GetDerivative(1.0f + epsilon);
            Assert.AreEqual(0.0f, (p2 - p1) / epsilon, epsilon);
            p1 = (float)spline.GetDerivative(3.0f - epsilon);
            p2 = (float)spline.GetDerivative(3.0f);
            Assert.AreEqual(0.0f, (p2 - p1) / epsilon, epsilon);
        }
    }
}
