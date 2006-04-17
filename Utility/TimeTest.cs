using System;
using System.Collections.Generic;
using System.Text;

namespace Utility
{
    using NUnit.Framework;

    [TestFixture]
    public class TimeTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoInit1()
        {
            float t = Time.CurrentTime;
        }

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestNoInit2()
        {
            float t = Time.StepTime;
        }

        [Test]
        public void TestInit()
        {
            Time.Initialize();
            Assert.AreEqual(0.0f, Time.StepTime);
            Assert.Greater(Time.CurrentTime, Time.StepTime);
        }

        [Test]
        public void TestCurrentTime()
        {
            Time.Initialize();
            float t1 = Time.CurrentTime;
            float t2 = Time.CurrentTime;

            Assert.Greater(t1, 0.0f);
            Assert.Greater(1.0f, t1);
            Assert.Greater(t2, t1);
        }

        [Test]
        public void TestStepTime()
        {
            float epsilon = 0.000001f;
            Time.Initialize();
            float t1 = Time.StepTime;
            float t2 = Time.StepTime;
            float d1 = Time.DeltaTime;
            Time.Step();
            float t3 = Time.StepTime;
            float d2 = Time.DeltaTime;

            Assert.AreEqual(0.0f, d1);
            Assert.AreEqual(0.0f, t1);
            Assert.AreEqual(t1, t2);
            Assert.AreEqual(t1 + d2, t3, epsilon);
            Assert.Greater(t3, t1);
        }

        [Test]
        public void TestPause()
        {
            Time.Initialize();
            Time.Step();
            Time.Pause();
            float ct1 = Time.CurrentTime;
            float st1 = Time.StepTime;
            float dt1 = Time.DeltaTime;
            Time.Step();
            float ct2 = Time.CurrentTime;
            float st2 = Time.StepTime;
            float dt2 = Time.DeltaTime;
            
            Assert.AreEqual(ct1, ct2);
            Assert.AreEqual(st1, st2);
            Assert.AreNotEqual(0.0f, dt1);
            Assert.AreEqual(0.0f, dt2);

            Time.Resume();
            float ct3 = Time.CurrentTime;
            float st3 = Time.StepTime;
            float dt3 = Time.DeltaTime;
            Time.Step();
            float ct4 = Time.CurrentTime;
            float st4 = Time.StepTime;
            float dt4 = Time.DeltaTime;

            Assert.AreNotEqual(ct1, ct3);
            Assert.AreNotEqual(ct3, ct4);
            Assert.AreEqual(st2, st3);
            Assert.AreNotEqual(st3, st4);
            Assert.AreEqual(0.0f, dt3);
            Assert.AreNotEqual(0.0f, dt4);

        }

    }
}
