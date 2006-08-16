using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using NUnit.Framework;

namespace Dope.DDXX.DemoFramework
{
    public class TestEffect : BaseEffect
    {
        public bool startCalled;
        public bool endCalled;

        public TestEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override void Step()
        {
        }

        public override void Render()
        {
        }

        public override void StartTimeUpdated()
        {
            startCalled = true;
        }

        public override void EndTimeUpdated()
        {
            endCalled = true;
        }

        public bool IsDeviceEqual(IDevice compareWith)
        {
            return compareWith == Device;
        }

    }

    [TestFixture]
    public class BaseEffectTest : D3DMockTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }
        
        [Test]
        public void TestInit()
        {
            TestEffect effect = new TestEffect(0.0f, 10.0f);
            Assert.IsTrue(effect.IsDeviceEqual(null));

            effect.Initialize();

            Assert.IsTrue(effect.IsDeviceEqual(D3DDriver.GetInstance().GetDevice()));
        }

        [Test]
        public void TestTime()
        {
            TestEffect effect = new TestEffect(0.0f, 10.0f);
            Assert.AreEqual(0.0f, effect.StartTime);
            Assert.AreEqual(10.0f, effect.EndTime);

            effect.StartTime = 1.0f;
            Assert.AreEqual(1.0f, effect.StartTime);
            Assert.IsTrue(effect.startCalled);
            effect.EndTime = 11.0f;
            Assert.AreEqual(11.0f, effect.EndTime);
            Assert.IsTrue(effect.endCalled);

        }
    }
}
