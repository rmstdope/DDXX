using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class MonochromePostEffectTest : DemoMockTest
    {
        private MonochromePostEffect sut;
        private IRenderTarget2D outputTexture;
        private IRenderTarget2D texture1;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            outputTexture = mockery.NewMock<IRenderTarget2D>();
            texture1 = mockery.NewMock<IRenderTarget2D>();
            sut = new MonochromePostEffect("", 1.0f, 2.0f);
            sut.Initialize(graphicsFactory, postProcessor);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestRender1()
        {
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(texture1);
            Expect.Once.On(postProcessor).
                Method("GetTemporaryTextures").
                With(1, false).Will(Return.Value(textures));
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(outputTexture));
            Expect.Once.On(postProcessor).
                Method("SetBlendParameters").
                With(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
            Expect.Once.On(postProcessor).
                Method("Process").
                With("Monochrome", outputTexture, texture1);
            sut.Render();
        }
    }
}
