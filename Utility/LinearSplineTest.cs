using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class LinearSplineTest
    {
        private LinearSpline<InterpolatedFloat> spline;

        [SetUp]
        public void SetUp()
        {
            spline = new LinearSpline<InterpolatedFloat>();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestAdd()
        {
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(0.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(2.0f, new InterpolatedFloat(10.0f)));
            Assert.AreEqual(1.0f, spline.StartTime);
            Assert.AreEqual(2.0f, spline.EndTime);
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.5f, new InterpolatedFloat(10.0f)));
            Assert.AreEqual(1.0f, spline.StartTime);
            Assert.AreEqual(2.0f, spline.EndTime);
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(0.0f, new InterpolatedFloat(10.0f)));
            Assert.AreEqual(0.0f, spline.StartTime);
            Assert.AreEqual(2.0f, spline.EndTime);
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(3.0f, new InterpolatedFloat(10.0f)));
            Assert.AreEqual(0.0f, spline.StartTime);
            Assert.AreEqual(3.0f, spline.EndTime);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestStartNoAdd()
        {
            Assert.AreEqual(1.0f, spline.StartTime);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestEndNoAdd()
        {
            Assert.AreEqual(1.0f, spline.EndTime);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAddSameTime()
        {
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(0.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(10.0f)));
        }

        [Test]
        public void TestCalcOK()
        {
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(10.0f)));
            spline.Calculate();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestCalcNoAdd()
        {
            spline.Calculate();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestGetValueNoCalc()
        {
            spline.GetValue(1.0f);
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
        [ExpectedException(typeof(DDXXException))]
        public void TestGetDerivativeNoCalc()
        {
            spline.GetDerivative(1.0f);
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
