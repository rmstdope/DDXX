using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class LinearSplineTest : SplineBaseTest
    {
        private LinearSpline<InterpolatedFloat> spline;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            spline = new LinearSpline<InterpolatedFloat>();
        }

        [Test]
        public void TestGetValues()
        {
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(0.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(2.0f, new InterpolatedFloat(100.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(3.0f, new InterpolatedFloat(-100.0f)));

            Assert.AreEqual(0.0f, (float)spline.GetValue(0.0f));
            Assert.AreEqual(0.0f, (float)spline.GetValue(1.0f));
            Assert.AreEqual(-100.0f, (float)spline.GetValue(3.0f));
            Assert.AreEqual(-100.0f, (float)spline.GetValue(4.0f));

            Assert.AreEqual(25.0f, (float)spline.GetValue(1.25f));

        }

        [Test]
        public void TestDerivative()
        {
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(0.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(2.0f, new InterpolatedFloat(100.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(3.0f, new InterpolatedFloat(-100.0f)));

            Assert.AreEqual(0.0f, (float)spline.GetDerivative(0.0f));
            Assert.AreEqual(0.0f, (float)spline.GetDerivative(3.1f));

            Assert.AreEqual(100.0f, (float)spline.GetDerivative(1.0f));
            Assert.AreEqual(100.0f, (float)spline.GetDerivative(1.2f));

            Assert.AreEqual(-200.0f, (float)spline.GetDerivative(2.0f));
            Assert.AreEqual(-200.0f, (float)spline.GetDerivative(2.1f));
            Assert.AreEqual(-200.0f, (float)spline.GetDerivative(3.0f));

        }
    }
}
