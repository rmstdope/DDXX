using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.DemoFramework;
using NMock2;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class RadialBlurPostEffectTest
    {
        private Mockery mockery;
        private RadialBlurPostEffect effect;
        private IPostProcessor postProcessor;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            effect = new RadialBlurPostEffect(1.0f, 2.0f);
            effect.Initialize(postProcessor, null, null, null);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestRenderInputIsInput()
        {
            // Starting with INPUT
            TestRender(TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
        }

        [Test]
        public void TestRenderFs1IsInput()
        {
            // Starting with FULLSCREEN_1
            TestRender(TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_3);
        }

        [Test]
        public void TestRenderFs2IsInput()
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
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
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
                    With(BlendOperation.Add, Blend.One, Blend.One, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").With("Copy", tempTexture1, startTexture);
                effect.Render();
            }
        }
    }
}
