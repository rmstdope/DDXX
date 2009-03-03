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
    public class FadeTransitionTest
    {
        private FadeTransition transition;
        private Mockery mockery;
        private IRenderTarget2D fromTexture;
        private IRenderTarget2D toTexture;
        private IPostProcessor postProcessor;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            fromTexture = mockery.NewMock<IRenderTarget2D>();
            toTexture = mockery.NewMock<IRenderTarget2D>();
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
        public void CombineDelta0()
        {
            Time.CurrentTime = 1.0f;
            CreateTransition(1.0f, 2.94f);
            ExpectProcessing(255);
            transition.Combine(fromTexture, toTexture);
        }

        [Test]
        public void CombineDelta1()
        {
            Time.CurrentTime = 4.0f;
            CreateTransition(2.32f, 4.0f);
            ExpectProcessing(0);
            transition.Combine(fromTexture, toTexture);
        }

        [Test]
        public void CombineDelta05()
        {
            Time.CurrentTime = 2.0f;
            CreateTransition(1.0f, 3.0f);
            ExpectProcessing(127);
            transition.Combine(fromTexture, toTexture);
        }

        private void ExpectProcessing(byte alpha)
        {
            Expect.Once.On(postProcessor).Method("StartFrame").With(fromTexture);
            Expect.Once.On(postProcessor).Method("SetBlendParameters").With(BlendFunction.Add, Blend.BlendFactor, Blend.InverseBlendFactor, new Color(alpha, alpha, alpha, alpha));
            Expect.Once.On(postProcessor).Method("Process").With("Copy", fromTexture, toTexture);
        }

        private void CreateTransition(float start, float end)
        {
            transition = new FadeTransition("", start, end);
            transition.Initialize(postProcessor, null);
        }
    }
}
