using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class MonochromePostEffectTest
    {
        private Mockery mockery;
        private MonochromePostEffect effect;
        private IPostProcessor postProcessor;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            effect = new MonochromePostEffect(1.0f, 2.0f);
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
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.INPUT_TEXTURE));
            Expect.Once.On(postProcessor).
                Method("Process").
                With("Monochrome", TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1);
            effect.Render();
        }

        [Test]
        public void TestRender2()
        {
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.FULLSIZE_TEXTURE_1));
            Expect.Once.On(postProcessor).
                Method("Process").
                With("Monochrome", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
            effect.Render();
        }
    }
}
