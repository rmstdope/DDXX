using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerTest : D3DMockTest
    {
        private DemoTweaker tweaker;
        private ISprite sprite;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            sprite = mockery.NewMock<ISprite>();

            tweaker = new DemoTweaker();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitialize()
        {
            Expect.Once.On(factory).
                Method("CreateSprite").
                WithAnyArguments().
                Will(Return.Value(sprite));
            
            tweaker.Initialize();
        }

        [Test]
        public void TestDraw()
        {
            TestInitialize();

            Assert.IsFalse(tweaker.Enabled);
            tweaker.Draw();

            tweaker.Enabled = true;
            Assert.IsTrue(tweaker.Enabled);
            Expect.Once.On(sprite).
                Method("Begin").
                With(SpriteFlags.AlphaBlend);
            Expect.Once.On(sprite).
                Method("End");
            tweaker.Draw();

            tweaker.Enabled = false;
            Assert.IsFalse(tweaker.Enabled);
            tweaker.Draw();
        }
    }
}
