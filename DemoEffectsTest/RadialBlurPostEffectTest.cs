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
    public class RadialBlurPostEffectTest : DemoMockTest
    {
        private RadialBlurPostEffect sut;
        private IRenderTarget2D texture1;
        private IRenderTarget2D texture2;
        private IRenderTarget2D outputTexture;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            texture1 = mockery.NewMock<IRenderTarget2D>();
            texture2 = mockery.NewMock<IRenderTarget2D>();
            outputTexture = mockery.NewMock<IRenderTarget2D>();
            sut = new RadialBlurPostEffect("", 1.0f, 2.0f);
            sut.Initialize(graphicsFactory, postProcessor, textureFactory, textureBuilder);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestRenderGray()
        {
            TestRender(outputTexture, texture1, texture2, Color.Gray);
        }

        [Test]
        public void TestRenderBlue()
        {
            TestRender(outputTexture, texture1, texture2, Color.Blue);
        }

        private void TestRender(IRenderTarget2D startTexture, IRenderTarget2D tempTexture1, IRenderTarget2D tempTexture2, Color color)
        {
            sut.BlurColor = color;
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(texture1);
            textures.Add(texture2);
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(startTexture));
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("GetTemporaryTextures").
                    With(2, true).Will(Return.Value(textures));
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("ZoomFactor", 0.20f);
                Expect.Once.On(postProcessor).
                    Method("Process").With("ZoomAdd", startTexture, tempTexture1);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("ZoomFactor", 0.60f);
                Expect.Once.On(postProcessor).
                    Method("Process").With("ZoomAdd", tempTexture1, tempTexture2);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("ZoomFactor", 0.80f);
                Expect.Once.On(postProcessor).
                    Method("Process").With("ZoomAdd", tempTexture2, tempTexture1);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("ZoomFactor", 0.90f);
                Expect.Once.On(postProcessor).
                    Method("Process").With("ZoomAdd", tempTexture1, tempTexture2);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("ZoomFactor", 0.95f);
                Expect.Once.On(postProcessor).
                    Method("Process").With("ZoomAdd", tempTexture2, tempTexture1);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("ZoomFactor", 0.975f);
                Expect.Once.On(postProcessor).
                    Method("Process").With("ZoomAdd", tempTexture1, tempTexture2);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("ZoomFactor", 0.9875f);
                Expect.Once.On(postProcessor).
                    Method("Process").With("ZoomAdd", tempTexture2, tempTexture1);
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendFunction.Add, Blend.BlendFactor, Blend.One, color);
                Expect.Once.On(postProcessor).
                    Method("Process").With("Copy", tempTexture1, startTexture);
                sut.Render();
            }
        }
    }
}
