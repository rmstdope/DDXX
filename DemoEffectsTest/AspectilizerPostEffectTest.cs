using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class AspectilizerPostEffectTest : DemoMockTest
    {
        private AspectilizerPostEffect aspectilizer;
        private IRenderTarget2D outputTexture;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            aspectilizer = new AspectilizerPostEffect("", 0, 10);
            outputTexture = mockery.NewMock<IRenderTarget2D>();
            Stub.On(textureFactory).GetProperty("WhiteTexture").Will(Return.Value(texture2D));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        /// <summary>
        /// Test initializing the effect
        /// </summary>
        [Test]
        public void TestInitialize1()
        {
            Expect.Once.On(textureFactory).Method("CreateFromGenerator").
                With(Is.EqualTo(presentParameters.BackBufferWidth), Is.EqualTo(presentParameters.BackBufferHeight), Is.EqualTo(1), Is.EqualTo(TextureUsage.None), Is.EqualTo(SurfaceFormat.Color), Is.Anything).
                Will(Return.Value(texture2D));
            aspectilizer.Initialize(graphicsFactory, postProcessor, textureFactory);
        }

        /// <summary>
        /// Test add rendering with INPUT_TEXTURE and no blend factor
        /// </summary>
        [Test]
        public void TestRenderAdd()
        {
            TestInitialize1();

            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(outputTexture));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendFunction.Add, Blend.One, Blend.InverseSourceColor, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture2D, outputTexture);

            aspectilizer.Render();
        }

        /// <summary>
        /// Test subtract rendering with INPUT_TEXTURE and no blend factor
        /// </summary>
        [Test]
        public void TestRenderSubtract()
        {
            TestInitialize1();

            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(outputTexture));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendFunction.ReverseSubtract, Blend.One, Blend.One, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture2D, outputTexture);

            aspectilizer.SubtractNoise = true;
            aspectilizer.Render();
        }

        /// <summary>
        /// Test rendering with FULLSIZE_TEXTURE_1 and blend factor 0.5
        /// </summary>
        [Test]
        public void TestRenderWithBlendFactor()
        {
            TestInitialize1();
            aspectilizer.BlendFactor = 0.5f;

            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(outputTexture));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendFunction.Add, Blend.One, Blend.InverseSourceColor, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 0.5f, 0.5f, 0.5f, 0.5f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture2D, outputTexture);

            aspectilizer.Render();
        }

    }
}
