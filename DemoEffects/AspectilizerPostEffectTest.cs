using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.DemoFramework;
using NMock2;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class AspectilizerPostEffectTest
    {
        private AspectilizerPostEffect aspectilizer;
        private Mockery mockery;
        private IPostProcessor postProcessor;
        private ITextureFactory textureFactory;
        private ITextureBuilder textureBuilder;
        private ITexture texture;
        private IDevice device;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            aspectilizer = new AspectilizerPostEffect(0, 10);
            postProcessor = mockery.NewMock<IPostProcessor>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            textureBuilder = mockery.NewMock<ITextureBuilder>();
            texture = mockery.NewMock<ITexture>();
            device = mockery.NewMock<IDevice>();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Test initializing the effect
        /// </summary>
        [Test]
        public void TestInitialize1()
        {
            Viewport viewport = new Viewport();
            viewport.Height = 1;
            viewport.Width = 2;
            Expect.Once.On(device).GetProperty("Viewport").Will(Return.Value(viewport));
            Expect.Once.On(textureBuilder).Method("Generate").
                With(Is.Anything, Is.EqualTo(viewport.Width), Is.EqualTo(viewport.Height), Is.EqualTo(1), Is.EqualTo(Format.A8R8G8B8)).
                Will(Return.Value(texture));
            aspectilizer.Initialize(postProcessor, textureFactory, textureBuilder, device);
        }

        /// <summary>
        /// Test add rendering with INPUT_TEXTURE and no blend factor
        /// </summary>
        [Test]
        public void TestRenderAdd()
        {
            TestInitialize1();

            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.INPUT_TEXTURE));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendOperation.Add, Blend.One, Blend.InvSourceColor, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture, TextureID.INPUT_TEXTURE);

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
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.INPUT_TEXTURE));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendOperation.RevSubtract, Blend.One, Blend.One, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture, TextureID.INPUT_TEXTURE);

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
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.FULLSIZE_TEXTURE_1));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendOperation.Add, Blend.One, Blend.InvSourceColor, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 0.5f, 0.5f, 0.5f, 0.5f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture, TextureID.FULLSIZE_TEXTURE_1);

            aspectilizer.Render();
        }

    }
}
