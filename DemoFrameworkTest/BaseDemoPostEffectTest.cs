using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class TestPostEffect : BaseDemoPostEffect
    {
        public TestPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        public override void Render()
        {
        }

        protected override void Initialize() { }
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
            TestPostEffect effect = new TestPostEffect("", 0.0f, 10.0f);

            effect.Initialize(null, postProcessor, null);
        }

        [Test]
        public void TestTime()
        {
            TestPostEffect effect = new TestPostEffect("", 0.0f, 10.0f);
            Assert.AreEqual(0.0f, effect.StartTime);
            Assert.AreEqual(10.0f, effect.EndTime);

            effect.StartTime = 1.0f;
            Assert.AreEqual(1.0f, effect.StartTime);
            effect.EndTime = 11.0f;
            Assert.AreEqual(11.0f, effect.EndTime);
        }
    }
}
