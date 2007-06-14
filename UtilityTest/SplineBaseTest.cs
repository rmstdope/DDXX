using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class SplineBaseTest
    {
        private class TestSpline<Type> : SplineBase<Type>
            where Type : IArithmetic, new()
        {
            public bool calcCalled = false;
            public bool getValueCalled = false;
            public bool getDerivativeCalled = false;
            protected override void DoCalculate()
            {
                calcCalled = true;
            }

            protected override Type DoGetValue(float time)
            {
                getValueCalled = true;
                return new Type();
            }

            protected override Type DoGetDerivative(float time)
            {
                getDerivativeCalled = true;
                return new Type();
            }
        }

        private TestSpline<InterpolatedFloat> spline;

        [SetUp]
        public virtual void SetUp()
        {
            spline = new TestSpline<InterpolatedFloat>();
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
            Assert.IsFalse(spline.calcCalled);
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(10.0f)));
            spline.Calculate();
            Assert.IsTrue(spline.calcCalled);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestCalcNoAdd()
        {
            Assert.IsFalse(spline.calcCalled);
            spline.Calculate();
            Assert.IsFalse(spline.calcCalled);
        }

        [Test]
        public void TestGetValueOK()
        {
            Assert.IsFalse(spline.getValueCalled);
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(10.0f)));
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(2.0f, new InterpolatedFloat(20.0f)));
            Assert.AreEqual(10.0f, (float)spline.GetValue(0.0f));
            Assert.IsFalse(spline.getValueCalled);
            Assert.AreEqual(20.0f, (float)spline.GetValue(3.0f));
            Assert.IsFalse(spline.getValueCalled);
            spline.GetValue(1.5f);
            Assert.IsTrue(spline.getValueCalled);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestGetValueNoAdd()
        {
            Assert.IsFalse(spline.getValueCalled);
            spline.GetValue(1.0f);
            Assert.IsFalse(spline.getValueCalled);
        }

        [Test]
        public void TestGetDerivativeOK()
        {
            Assert.IsFalse(spline.getDerivativeCalled);
            spline.AddKeyFrame(new KeyFrame<InterpolatedFloat>(1.0f, new InterpolatedFloat(10.0f)));
            spline.GetDerivative(1.0f);
            Assert.IsTrue(spline.getDerivativeCalled);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestGetDerivativeNoAdd()
        {
            Assert.IsFalse(spline.getDerivativeCalled);
            spline.GetDerivative(1.0f);
            Assert.IsFalse(spline.getDerivativeCalled);
        }

    }
}
