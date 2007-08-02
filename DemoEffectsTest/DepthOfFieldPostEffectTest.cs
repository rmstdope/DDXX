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
    public class DepthOfFieldPostEffectTest
    {
        private Mockery mockery;
        private DepthOfFieldPostEffect effect;
        private IPostProcessor postProcessor;
        private ITexture texture1;
        private ITexture outputTexture;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            texture1 = mockery.NewMock<ITexture>();
            outputTexture = mockery.NewMock<ITexture>();
            effect = new DepthOfFieldPostEffect("", 1.0f, 2.0f);
            effect.Initialize(null, postProcessor, null, null, null);
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
                With("DepthOfField", outputTexture, texture1);
            effect.Render();
        }
    }
}
