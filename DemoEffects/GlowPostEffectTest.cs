using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.DemoFramework;

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
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.INPUT_TEXTURE));
            Expect.Once.On(postProcessor).
                Method("Process").
                With("DownSample4x", TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1);
            Expect.Once.On(postProcessor).
                Method("Process").
                With("DownSample4x", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
            effect.Render();
        }

        [Test]
        public void TestRender2()
        {
            // Starting with FULLSCREEN_1
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.FULLSIZE_TEXTURE_1));
            Expect.Once.On(postProcessor).
                Method("Process").
                With("DownSample4x", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
            Expect.Once.On(postProcessor).
                Method("Process").
                With("DownSample4x", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_3);
            effect.Render();
        }

        [Test]
        public void TestRender3()
        {
            // Starting with FULLSCREEN_2
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.FULLSIZE_TEXTURE_2));
            Expect.Once.On(postProcessor).
                Method("Process").
                With("DownSample4x", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
            Expect.Once.On(postProcessor).
                Method("Process").
                With("DownSample4x", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_3);
            effect.Render();
        }
    }
}
