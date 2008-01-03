using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace Dope.DDXX.Utility
{
    using NUnit.Framework;

    [TestFixture]
    public class TimeTest
    {
        [SetUp]
        public void SetUp()
        {
            Time.Reset();
        }

        [Test]
        public void TestCurrentTime()
        {
            float delta = 1.23f;
            float t1 = Time.CurrentTime;
            float t2 = Time.CurrentTime;
            float d1 = Time.DeltaTime;
            Time.Step(delta);
            float t3 = Time.CurrentTime;
            float d2 = Time.DeltaTime;
            Time.Step(delta * 2);
            float t4 = Time.CurrentTime;
            float d3 = Time.DeltaTime;

            Assert.AreEqual(0.0f, d1);
            Assert.AreEqual(0.0f, t1);
            Assert.AreEqual(0.0f, t2);

            Assert.AreEqual(delta, d2);
            Assert.AreEqual(delta, t3);

            Assert.AreEqual(delta * 2, d3);
            Assert.AreEqual(delta * 3, t4);
        }

        [Test]
        public void TestPause()
        {
            Time.Step(1.0f);
            Time.Pause();
            float ct1 = Time.CurrentTime;
            float dt1 = Time.DeltaTime;
            Time.Step(1.0f);
            float ct2 = Time.CurrentTime;
            float dt2 = Time.DeltaTime;
            
            Assert.AreEqual(ct1, ct2);
            Assert.AreNotEqual(0.0f, dt1);
            Assert.AreEqual(0.0f, dt2);

            Time.Resume();
            float ct3 = Time.CurrentTime;
            float dt3 = Time.DeltaTime;
            Time.Step(1.0f);
            float ct4 = Time.CurrentTime;
            float dt4 = Time.DeltaTime;

            // Check dependencies between values
            Assert.AreEqual(ct3 + 1.0f, ct4);
            Assert.AreEqual(0.0f, dt3);
            Assert.AreNotEqual(0.0f, dt4);

        }

        [Test]
        public void TestSet()
        {
            Time.Pause();
            Time.CurrentTime = 2.3f;
            Assert.AreEqual(2.3f, Time.CurrentTime);
            Time.Resume();
            Assert.AreEqual(2.3f, Time.CurrentTime);
            Time.Step(1.0f);
            Assert.AreEqual(3.3f, Time.CurrentTime);
            Time.CurrentTime = 2.3f;
            Assert.AreEqual(2.3f, Time.CurrentTime);
        }

    }
}
