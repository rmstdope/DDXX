using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Threading;

namespace Utility
{
    using NUnit.Framework;

    [TestFixture]
    public class TimeTest
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

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
            float epsilon = 0.000001f;
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

            long frequency;
            long startTime;
            long currentTime;
            QueryPerformanceFrequency(out frequency);
            QueryPerformanceCounter(out startTime);
            QueryPerformanceCounter(out currentTime);
            while((float)(currentTime - startTime) / (float)frequency < 2.0f)
                QueryPerformanceCounter(out currentTime);

            Time.Resume();
            float ct3 = Time.CurrentTime;
            float st3 = Time.StepTime;
            float dt3 = Time.DeltaTime;
            Time.Step();
            float ct4 = Time.CurrentTime;
            float st4 = Time.StepTime;
            float dt4 = Time.DeltaTime;

            // Check dependencies between values
            Assert.AreNotEqual(ct1, ct3);
            Assert.AreNotEqual(ct3, ct4);
            Assert.AreEqual(st2, st3, epsilon);
            Assert.AreNotEqual(st3, st4);
            Assert.AreEqual(0.0f, dt3);
            Assert.AreNotEqual(0.0f, dt4);

            // Sanity check actual values
            Assert.Less(dt4, 0.1f);
            Assert.AreEqual(dt4, st4 - st3, epsilon);
            Assert.Less(ct3 - ct1, 0.1f);

        }

        [Test]
        public void TestSet()
        {
            Time.Initialize();
            Time.Pause();
            Time.CurrentTime = 2.3f;
            Assert.AreEqual(2.3f, Time.StepTime);
            Assert.AreEqual(2.3f, Time.CurrentTime);
            Time.Resume();
            Assert.AreEqual(2.3f, Time.StepTime);
            Assert.Greater(Time.CurrentTime, 2.3f);
            Assert.Less(Time.CurrentTime, 2.31f);
            Time.Step();
            Assert.AreNotEqual(2.3f, Time.StepTime);
            Time.CurrentTime = 2.3f;
            Assert.AreEqual(2.3f, Time.StepTime);
            Assert.Greater(Time.CurrentTime, 2.3f);
            Assert.Less(Time.CurrentTime, 2.31f);
        }

    }
}
