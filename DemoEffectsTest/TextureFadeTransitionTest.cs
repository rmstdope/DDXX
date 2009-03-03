using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class TextureFadeTransitionTest
    {
        private TextureFadeTransition transition;
        private Mockery mockery;
        private IRenderTarget2D fromTexture;
        private IRenderTarget2D toTexture;
        private ITexture2D thirdTexture;
        private IPostProcessor postProcessor;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            fromTexture = mockery.NewMock<IRenderTarget2D>();
            toTexture = mockery.NewMock<IRenderTarget2D>();
            thirdTexture = mockery.NewMock<ITexture2D>();
            postProcessor = mockery.NewMock<IPostProcessor>();
            Time.Pause();
        }

        [TearDown]
        public void TearDown()
        {
            Time.Resume();
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void Combine1()
        {
            Time.CurrentTime = 1.0f;
            CreateTransition(1.0f, 2.0f);
            transition.FadeDelay = 17.2f;
            transition.Texture = thirdTexture;
            Expect.Once.On(postProcessor).Method("StartFrame").With(fromTexture);
            Expect.Once.On(postProcessor).Method("SetBlendParameters").With(BlendFunction.Add, Blend.SourceAlpha, Blend.InverseSourceAlpha, Color.Black);
            Expect.Once.On(postProcessor).Method("SetValue").With("MiscTexture", thirdTexture);
            Expect.Once.On(postProcessor).Method("SetValue").With("Time", 0.0f);
            Expect.Once.On(postProcessor).Method("SetValue").With("TextureAlphaFadeDelay", 17.2f);
            Expect.Once.On(postProcessor).Method("SetValue").With("TextureAlphaFadeLength", -16.2f);
            Expect.Once.On(postProcessor).Method("Process").With("TextureAlpha", fromTexture, toTexture);
            transition.Combine(fromTexture, toTexture);
        }

        [Test]
        public void Combine2()
        {
            Time.CurrentTime = 15.0f;
            CreateTransition(10.0f, 20.0f);
            transition.FadeDelay = 1f;
            transition.Texture = thirdTexture;
            Expect.Once.On(postProcessor).Method("StartFrame").With(fromTexture);
            Expect.Once.On(postProcessor).Method("SetBlendParameters").With(BlendFunction.Add, Blend.SourceAlpha, Blend.InverseSourceAlpha, Color.Black);
            Expect.Once.On(postProcessor).Method("SetValue").With("MiscTexture", thirdTexture);
            Expect.Once.On(postProcessor).Method("SetValue").With("Time", 5.0f);
            Expect.Once.On(postProcessor).Method("SetValue").With("TextureAlphaFadeDelay", 1.0f);
            Expect.Once.On(postProcessor).Method("SetValue").With("TextureAlphaFadeLength", 9.0f);
            Expect.Once.On(postProcessor).Method("Process").With("TextureAlpha", fromTexture, toTexture);
            transition.Combine(fromTexture, toTexture);
        }

        private void CreateTransition(float start, float end)
        {
            transition = new TextureFadeTransition("", start, end);
            transition.Initialize(postProcessor, null);
        }
    }
}
