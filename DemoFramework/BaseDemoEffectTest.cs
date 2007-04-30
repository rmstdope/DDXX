using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class TestEffect : BaseDemoEffect
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

        protected override void Initialize()
        {
        }
    }

    [TestFixture]
    public class BaseDemoEffectTest : D3DMockTest
    {
        private IEffect poolEffect;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            D3DDriver.EffectFactory = new EffectFactory(null, factory);
            poolEffect = mockery.NewMock<IEffect>();
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

            effect.Initialize(factory, device);

            Assert.IsTrue(effect.IsDeviceEqual(D3DDriver.GetInstance().Device));
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
