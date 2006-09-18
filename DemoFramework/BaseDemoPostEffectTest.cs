using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class TestPostEffect : BaseDemoPostEffect
    {
        public bool startCalled;
        public bool endCalled;

        public TestPostEffect(float startTime, float endTime)
            : base(startTime, endTime)
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
    }

    [TestFixture]
    public class BaseDemoPostEffectTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestInit()
        {
            PostProcessor postProcessor = new PostProcessor();
            TestPostEffect effect = new TestPostEffect(0.0f, 10.0f);

            effect.Initialize(postProcessor);
        }

        [Test]
        public void TestTime()
        {
            TestPostEffect effect = new TestPostEffect(0.0f, 10.0f);
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