using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class OverlayPostEffectTest
    {
        private OverlayPostEffect overlay;
        private Mockery mockery;
        private IPostProcessor postProcessor;
        private ITextureFactory textureFactory;
        private ITexture texture;
        private ITexture outputTexture;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            overlay = new OverlayPostEffect(0, 10);
            postProcessor = mockery.NewMock<IPostProcessor>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            texture = mockery.NewMock<ITexture>();
            outputTexture = mockery.NewMock<ITexture>();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Test initializing the effect using file 'file1'
        /// </summary>
        [Test]
        public void TestInitialize1()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("file1").Will(Return.Value(texture));

            overlay.AddNoise = true;
            overlay.Filename = "file1";
            overlay.Initialize(postProcessor, textureFactory, null, null);
        }

        /// <summary>
        /// Test initializing the effect using file 'file2'
        /// </summary>
        [Test]
        public void TestInitialize2()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("file2").Will(Return.Value(texture));

            overlay.AddNoise = true;
            overlay.Filename = "file2";
            overlay.Initialize(postProcessor, textureFactory, null, null);
        }

        /// <summary>
        /// Test initializing the effect without setting file name
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail1()
        {
            overlay.Initialize(postProcessor, textureFactory, null, null);
        }

        /// <summary>
        /// Test initializing the effect without either add or subtract
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail2()
        {
            overlay.Filename = "file2";
            overlay.Initialize(postProcessor, textureFactory, null, null);
        }

        /// <summary>
        /// Test initializing the effect with texture already set
        /// </summary>
        [Test]
        public void TestInitializeTextureAlreadySet()
        {
            overlay.Texture = texture;
            overlay.SubtractNoise = true;
            overlay.Initialize(postProcessor, textureFactory, null, null);
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
                With(BlendOperation.Add, Blend.One, Blend.InvSourceColor, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture, outputTexture);

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
                With(BlendOperation.RevSubtract, Blend.One, Blend.One, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture, outputTexture);

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
                With(BlendOperation.Add, Blend.One, Blend.InvSourceColor, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 0.5f, 0.5f, 0.5f, 0.5f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture, outputTexture);

            overlay.Render();
        }
    }
}
