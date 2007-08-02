using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.DemoFramework;
using NMock2;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class RadialBlurPostEffectTest
    {
        private Mockery mockery;
        private RadialBlurPostEffect effect;
        private IPostProcessor postProcessor;
        private ITexture texture1;
        private ITexture texture2;
        private ITexture outputTexture;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            texture1 = mockery.NewMock<ITexture>();
            texture2 = mockery.NewMock<ITexture>();
            outputTexture = mockery.NewMock<ITexture>();
            effect = new RadialBlurPostEffect("", 1.0f, 2.0f);
            effect.Initialize(null, postProcessor, null, null, null);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
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

        private void TestRender(ITexture startTexture, ITexture tempTexture1, ITexture tempTexture2, Color color)
        {
            effect.BlurColor = color;
            List<ITexture> textures = new List<ITexture>();
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
                    With(BlendOperation.Add, Blend.BlendFactor, Blend.One, color);
                Expect.Once.On(postProcessor).
                    Method("Process").With("Copy", tempTexture1, startTexture);
                effect.Render();
            }
        }
    }
}
