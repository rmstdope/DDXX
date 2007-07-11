using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class MonochromePostEffectTest
    {
        private Mockery mockery;
        private MonochromePostEffect effect;
        private IPostProcessor postProcessor;
        private ITexture outputTexture;
        private ITexture texture1;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            outputTexture = mockery.NewMock<ITexture>();
            texture1 = mockery.NewMock<ITexture>();
            effect = new MonochromePostEffect("", 1.0f, 2.0f);
            effect.Initialize(postProcessor, null, null, null);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestRender1()
        {
            List<ITexture> textures = new List<ITexture>();
            textures.Add(texture1);
            Expect.Once.On(postProcessor).
                Method("GetTemporaryTextures").
                With(1, false).Will(Return.Value(textures));
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(outputTexture));
            Expect.Once.On(postProcessor).
                Method("SetBlendParameters").
                With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            Expect.Once.On(postProcessor).
                Method("Process").
                With("Monochrome", outputTexture, texture1);
            effect.Render();
        }
    }
}
