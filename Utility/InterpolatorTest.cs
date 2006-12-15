using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class InterpolatorTest
    {
        private Interpolator<InterpolatedFloat> interpolator;
        private Mockery mockery;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            interpolator = new Interpolator<InterpolatedFloat>();
        }

        [Test]
        public void TestStartEnd()
        {
            interpolator.AddSpline(CreateMockedSpline(1.0f, 2.0f));
            Assert.AreEqual(1.0f, interpolator.StartTime);
            Assert.AreEqual(2.0f, interpolator.EndTime);

            interpolator.AddSpline(CreateMockedSpline(11.0f, 12.0f));
            Assert.AreEqual(1.0f, interpolator.StartTime);
            Assert.AreEqual(12.0f, interpolator.EndTime);

            interpolator.AddSpline(CreateMockedSpline(2.0f, 3.0f));
            Assert.AreEqual(1.0f, interpolator.StartTime);
            Assert.AreEqual(12.0f, interpolator.EndTime);

            interpolator.AddSpline(CreateMockedSpline(0.0f, 1.0f));
            Assert.AreEqual(0.0f, interpolator.StartTime);
            Assert.AreEqual(12.0f, interpolator.EndTime);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAddFail1()
        {
            interpolator.AddSpline(CreateMockedSpline(10.0f, 100.0f));
            interpolator.AddSpline(CreateMockedSpline(0.0f, 20.0f));
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAddFail2()
        {
            interpolator.AddSpline(CreateMockedSpline(10.0f, 100.0f));
            interpolator.AddSpline(CreateMockedSpline(20.0f, 120.0f));
        }
        
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAddFail3()
        {
            interpolator.AddSpline(CreateMockedSpline(10.0f, 100.0f));
            interpolator.AddSpline(CreateMockedSpline(20.0f, 30.0f));
        }
        
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAddFail4()
        {
            interpolator.AddSpline(CreateMockedSpline(10.0f, 100.0f));
            interpolator.AddSpline(CreateMockedSpline(0.0f, 200.0f));
        }

        [Test]
        public void TestGetValue()
        {
            ISpline<InterpolatedFloat> spline1 = CreateMockedSpline(1.0f, 2.0f);
            ISpline<InterpolatedFloat> spline2 = CreateMockedSpline(2.0f, 3.0f);
            ISpline<InterpolatedFloat> spline3 = CreateMockedSpline(100.0f, 101.0f);
            interpolator.AddSpline(spline1);
            interpolator.AddSpline(spline2);
            interpolator.AddSpline(spline3);

            Expect.Once.On(spline1).Method("GetValue").With(1.0f).Will(Return.Value(new InterpolatedFloat(10.0f)));
            Assert.AreEqual(10.0f, (float)interpolator.GetValue(1.0f));

            Expect.Once.On(spline1).Method("GetValue").With(2.0f).Will(Return.Value(new InterpolatedFloat(20.0f)));
            Assert.AreEqual(20.0f, (float)interpolator.GetValue(2.0f));

            Expect.Once.On(spline2).Method("GetValue").With(2.5f).Will(Return.Value(new InterpolatedFloat(25.0f)));
            Assert.AreEqual(25.0f, (float)interpolator.GetValue(2.5f));

            Expect.Once.On(spline2).Method("GetValue").With(3.0f).Will(Return.Value(new InterpolatedFloat(30.0f)));
            Assert.AreEqual(30.0f, (float)interpolator.GetValue(3.0f));

            Expect.Once.On(spline3).Method("GetValue").With(101.0f).Will(Return.Value(new InterpolatedFloat(1010.0f)));
            Assert.AreEqual(1010.0f, (float)interpolator.GetValue(101.0f));

            Expect.Once.On(spline3).Method("GetValue").With(500.0f).Will(Return.Value(new InterpolatedFloat(5000.0f)));
            Assert.AreEqual(5000.0f, (float)interpolator.GetValue(500.0f));
        }

        [Test]
        public void TestGetDerivative()
        {
            ISpline<InterpolatedFloat> spline1 = CreateMockedSpline(1.0f, 2.0f);
            ISpline<InterpolatedFloat> spline2 = CreateMockedSpline(2.0f, 3.0f);
            ISpline<InterpolatedFloat> spline3 = CreateMockedSpline(100.0f, 101.0f);
            interpolator.AddSpline(spline1);
            interpolator.AddSpline(spline2);
            interpolator.AddSpline(spline3);

            Expect.Once.On(spline1).Method("GetDerivative").With(1.0f).Will(Return.Value(new InterpolatedFloat(10.0f)));
            Assert.AreEqual(10.0f, (float)interpolator.GetDerivative(1.0f));

            Expect.Once.On(spline1).Method("GetDerivative").With(2.0f).Will(Return.Value(new InterpolatedFloat(20.0f)));
            Assert.AreEqual(20.0f, (float)interpolator.GetDerivative(2.0f));

            Expect.Once.On(spline2).Method("GetDerivative").With(2.5f).Will(Return.Value(new InterpolatedFloat(25.0f)));
            Assert.AreEqual(25.0f, (float)interpolator.GetDerivative(2.5f));

            Expect.Once.On(spline2).Method("GetDerivative").With(3.0f).Will(Return.Value(new InterpolatedFloat(30.0f)));
            Assert.AreEqual(30.0f, (float)interpolator.GetDerivative(3.0f));

            Expect.Once.On(spline3).Method("GetDerivative").With(101.0f).Will(Return.Value(new InterpolatedFloat(1010.0f)));
            Assert.AreEqual(1010.0f, (float)interpolator.GetDerivative(101.0f));

            Expect.Once.On(spline3).Method("GetDerivative").With(500.0f).Will(Return.Value(new InterpolatedFloat(5000.0f)));
            Assert.AreEqual(5000.0f, (float)interpolator.GetDerivative(500.0f));
        }

        private ISpline<InterpolatedFloat> CreateMockedSpline(float start, float end)
        {
            ISpline<InterpolatedFloat> spline = mockery.NewMock<ISpline<InterpolatedFloat>>();
            Stub.On(spline).
                GetProperty("StartTime").
                Will(Return.Value(start));
            Stub.On(spline).
                GetProperty("EndTime").
                Will(Return.Value(end));
            return spline;
        }
    }
}
