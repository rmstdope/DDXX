using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using System.Drawing;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class GlowPostEffectTest
    {
        private Mockery mockery;
        private GlowPostEffect effect;
        private IPostProcessor postProcessor;
        private ITexture outputTexture;
        private ITexture texture1;
        private ITexture texture2;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            outputTexture = mockery.NewMock<ITexture>();
            texture1 = mockery.NewMock<ITexture>();
            texture2 = mockery.NewMock<ITexture>();
            effect = new GlowPostEffect("", 1.0f, 2.0f);
            effect.Initialize(null, postProcessor, null, null, null);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
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

        private void TestRenderOneDownsample(ITexture startTexture, ITexture tempTexture1, ITexture tempTexture2)
        {
            List<ITexture> textures = new List<ITexture>();
            textures.Add(tempTexture1);
            textures.Add(tempTexture2);
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(startTexture));
            using (mockery.Ordered)
            {
                effect.Luminance = 0.06f;
                effect.Exposure = 0.18f;
                effect.WhiteCutoff = 0.1f;
                effect.BloomScale = 1.5f;
                effect.DownSamples = 1;
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
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
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
                    With(BlendOperation.Add, Blend.One, Blend.One, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", tempTexture1, startTexture);
            }
            effect.Render();
        }

        private void TestRenderTwoDownsamples(ITexture startTexture, ITexture tempTexture1, ITexture tempTexture2, bool advancedBrighten)
        {
            List<ITexture> textures = new List<ITexture>();
            textures.Add(tempTexture1);
            textures.Add(tempTexture2);
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(startTexture));
            using (mockery.Ordered)
            {
                effect.Luminance = 0.06f;
                effect.Exposure = 0.18f;
                effect.WhiteCutoff = 0.1f;
                effect.BloomScale = 1.5f;
                effect.DownSamples = 2;
                effect.AdvancedGlow = advancedBrighten;
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
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
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
                    With(BlendOperation.Add, Blend.One, Blend.One, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", tempTexture2, startTexture);
            }
            effect.Render();
        }

    }
}
