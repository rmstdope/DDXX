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
        private Interpolator interpolator;
        private Mockery mockery;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            interpolator = new Interpolator();
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

        //[Test]
        //public void TestGetValue()
        //{
        //    ISpline spline1 = CreateMockedSpline(1.0f, 2.0f);
        //    ISpline spline2 = CreateMockedSpline(2.0f, 3.0f);
        //    ISpline spline3 = CreateMockedSpline(100.0f, 101.0f);
        //    interpolator.AddSpline(spline1);
        //    interpolator.AddSpline(spline2);
        //    interpolator.AddSpline(spline3);

        //    Expect.Once.On(spline1).Method("GetValue").With(1.0f).Will(Return.Value(new InterpolatedFloat()));
        //}

        private ISpline CreateMockedSpline(float start, float end)
        {
            ISpline spline = mockery.NewMock<ISpline>();
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
