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

        /// <summary>
        /// Test initializing the effect using file 'file1'
        /// </summary>
        [Test]
        public void TestInitialize1()
        {
            Expect.Once.On(textureFactory).Method("CreateFromName").
                With("file1").Will(Return.Value(texture2D));

            overlay.AddNoise = true;
            overlay.Filename = "file1";
            overlay.Initialize(graphicsFactory, postProcessor);
        }

        /// <summary>
        /// Test initializing the effect using file 'file2'
        /// </summary>
        [Test]
        public void TestInitialize2()
        {
            Expect.Once.On(textureFactory).Method("CreateFromName").
                With("file2").Will(Return.Value(texture2D));

            overlay.AddNoise = true;
            overlay.Filename = "file2";
            overlay.Initialize(graphicsFactory, postProcessor);
        }

        /// <summary>
        /// Test initializing the effect without setting file name
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail1()
        {
            overlay.Initialize(graphicsFactory, postProcessor);
        }

        /// <summary>
        /// Test initializing the effect without either add or subtract
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail2()
        {
            overlay.Filename = "file2";
            overlay.Initialize(graphicsFactory, postProcessor);
        }

        /// <summary>
        /// Test initializing the effect with texture already set
        /// </summary>
        [Test]
        public void TestInitializeTextureAlreadySet()
        {
            overlay.Texture = texture2D;
            overlay.SubtractNoise = true;
            overlay.Initialize(graphicsFactory, postProcessor);
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

            overlay.Render();
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

            overlay.AddNoise = false;
            overlay.SubtractNoise = true;
            overlay.Render();
        }

        /// <summary>
        /// Test rendering with FULLSIZE_TEXTURE_1 and blend factor 0.5
        /// </summary>
        [Test]
        public void TestRenderWithBlendFactor()
        {
            TestInitialize1();
            overlay.BlendFactor = 0.5f;

            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(outputTexture));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendFunction.Add, Blend.One, Blend.InverseSourceColor, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 0.5f, 0.5f, 0.5f, 0.5f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture2D, outputTexture);

            overlay.Render();
        }
    }
}
