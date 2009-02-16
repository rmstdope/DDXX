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
    public class GlowPostEffectTest : DemoMockTest
    {
        private GlowPostEffect sut;
        private IRenderTarget2D outputTexture;
        private IRenderTarget2D texture1;
        private IRenderTarget2D texture2;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            outputTexture = mockery.NewMock<IRenderTarget2D>();
            texture1 = mockery.NewMock<IRenderTarget2D>();
            texture2 = mockery.NewMock<IRenderTarget2D>();
            sut = new GlowPostEffect("", 1.0f, 2.0f);
            sut.Initialize(graphicsFactory, postProcessor);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestRenderInputIsInput()
        {
            TestRenderOneDownsample(outputTexture, texture1, texture2);
        }

        [Test]
        public void TestTwoDownSamples()
        {
            TestRenderTwoDownsamples(outputTexture, texture1, texture2, false);
        }

        [Test]
        public void TestAdvancedBrighten()
        {
            TestRenderTwoDownsamples(outputTexture, texture1, texture2, true);
        }

        private void TestRenderOneDownsample(IRenderTarget2D startTexture, IRenderTarget2D tempTexture1, IRenderTarget2D tempTexture2)
        {
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(tempTexture1);
            textures.Add(tempTexture2);
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(startTexture));
            using (mockery.Ordered)
            {
                sut.Luminance = 0.06f;
                sut.Exposure = 0.18f;
                sut.WhiteCutoff = 0.1f;
                sut.BloomScale = 1.5f;
                sut.DownSamples = 1;
                Expect.Once.On(postProcessor).
                    Method("GetTemporaryTextures").
                    With(2, true).Will(Return.Value(textures));
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("Luminance", 0.06f);
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("Exposure", 0.18f);
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("WhiteCutoff", 0.1f);
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("BloomScale", 1.5f);
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("DownSample4x", startTexture, tempTexture2);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("Brighten", tempTexture2, tempTexture1);
                for (int i = 0; i < 2; i++)
                {
                    Expect.Once.On(postProcessor).
                        Method("Process").
                        With("HorizontalBloom", tempTexture1, tempTexture2);
                    Expect.Once.On(postProcessor).
                        Method("Process").
                        With("VerticalBloom", tempTexture2, tempTexture1);
                }
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendFunction.Add, Blend.One, Blend.One, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", tempTexture1, startTexture);
            }
            sut.Render();
        }

        private void TestRenderTwoDownsamples(IRenderTarget2D startTexture, IRenderTarget2D tempTexture1, IRenderTarget2D tempTexture2, bool advancedBrighten)
        {
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(tempTexture1);
            textures.Add(tempTexture2);
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(startTexture));
            using (mockery.Ordered)
            {
                sut.Luminance = 0.06f;
                sut.Exposure = 0.18f;
                sut.WhiteCutoff = 0.1f;
                sut.BloomScale = 1.5f;
                sut.DownSamples = 2;
                sut.AdvancedGlow = advancedBrighten;
                Expect.Once.On(postProcessor).
                    Method("GetTemporaryTextures").
                    With(2, true).Will(Return.Value(textures));
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("Luminance", 0.06f);
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("Exposure", 0.18f);
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("WhiteCutoff", 0.1f);
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("BloomScale", 1.5f);
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("DownSample4x", startTexture, tempTexture1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("DownSample4x", tempTexture1, tempTexture2);
                if (advancedBrighten)
                    Expect.Once.On(postProcessor).
                        Method("Process").
                        With("AdvancedBrighten", tempTexture2, tempTexture1);
                else
                    Expect.Once.On(postProcessor).
                        Method("Process").
                        With("Brighten", tempTexture2, tempTexture1);
                for (int i = 0; i < 2; i++)
                {
                    Expect.Once.On(postProcessor).
                        Method("Process").
                        With("HorizontalBloom", tempTexture1, tempTexture2);
                    Expect.Once.On(postProcessor).
                        Method("Process").
                        With("VerticalBloom", tempTexture2, tempTexture1);
                }
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", tempTexture1, tempTexture2);
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendFunction.Add, Blend.One, Blend.One, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", tempTexture2, startTexture);
            }
            sut.Render();
        }

    }
}
