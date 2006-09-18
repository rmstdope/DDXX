using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using System.Drawing;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class GlowPostEffectTest
    {
        private Mockery mockery;
        private GlowPostEffect effect;
        private IPostProcessor postProcessor;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            effect = new GlowPostEffect(1.0f, 2.0f);
            effect.Initialize(postProcessor);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestRender1()
        {
            // Starting with INPUT
            TestRender(TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
        }

        [Test]
        public void TestRender2()
        {
            // Starting with FULLSCREEN_1
            TestRender(TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_3);
        }

        [Test]
        public void TestRender3()
        {
            // Starting with FULLSCREEN_2
            TestRender(TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_3);
        }

        private void TestRender(TextureID startTexture, TextureID tempTexture1, TextureID tempTexture2)
        {
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(startTexture));
            using (mockery.Ordered)
            {
                effect.Luminance = 0.06f;
                effect.Exposure = 0.18f;
                effect.WhiteCutoff = 0.1f;
                effect.BloomScale = 1.5f;
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
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("Brighten", tempTexture2, tempTexture1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("HorizontalBloom", tempTexture1, tempTexture2);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("VerticalBloom", tempTexture2, tempTexture1);
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
