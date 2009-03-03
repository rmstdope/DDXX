using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.DemoFramework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class OverlayPostEffectTest : DemoMockTest
    {
        private OverlayPostEffect overlay;
        private IRenderTarget2D outputTexture;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            overlay = new OverlayPostEffect("", 0, 10);
            outputTexture = mockery.NewMock<IRenderTarget2D>();
            Stub.On(textureFactory).GetProperty("WhiteTexture").Will(Return.Value(texture2D));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void InitializeFailNoTexture()
        {
            overlay.Initialize(graphicsFactory, postProcessor);
        }

        [Test]
        public void InitializeOk()
        {
            overlay.Texture = texture2D;
            overlay.Initialize(graphicsFactory, postProcessor);
        }

        [Test]
        public void RenderAdd()
        {
            InitializeOk();

            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(outputTexture));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendFunction.Add, Blend.One, Blend.InverseSourceColor, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture2D, outputTexture);

            overlay.AddNoise = true;
            overlay.Render();
        }

        [Test]
        public void RenderSubtract()
        {
            InitializeOk();

            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(outputTexture));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendFunction.ReverseSubtract, Blend.One, Blend.One, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture2D, outputTexture);

            overlay.SubtractNoise = true;
            overlay.Render();
        }

        [Test]
        public void RenderWithBlendFactor()
        {
            InitializeOk();

            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(outputTexture));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendFunction.Add, Blend.One, Blend.InverseSourceColor, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 0.5f, 0.5f, 0.5f, 0.5f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture2D, outputTexture);

            overlay.BlendFactor = 0.5f;
            overlay.AddNoise = true;
            overlay.Render();
        }
    }
}
