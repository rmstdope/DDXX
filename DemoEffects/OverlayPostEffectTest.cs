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
    public class OverlayPostEffectTest : D3DMockTest
    {
        private OverlayPostEffect overlay;
        private IPostProcessor postProcessor;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            overlay = new OverlayPostEffect(0, 10);
            postProcessor = mockery.NewMock<IPostProcessor>();
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
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("file1").Will(Return.Value(texture));

            overlay.AddNoise = true;
            overlay.Filename = "file1";
            overlay.Initialize(postProcessor);
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
            overlay.Initialize(postProcessor);
        }

        /// <summary>
        /// Test initializing the effect without setting file name
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail1()
        {
            overlay.Initialize(postProcessor);
        }

        /// <summary>
        /// Test initializing the effect without either add or subtract
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail2()
        {
            overlay.Filename = "file2";
            overlay.Initialize(postProcessor);
        }

        /// <summary>
        /// Test initializing the effect with both add and subtract
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail3()
        {
            overlay.Filename = "file2";
            overlay.AddNoise = true;
            overlay.SubtractNoise = true;
            overlay.Initialize(postProcessor);
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
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.INPUT_TEXTURE));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendOperation.RevSubtract, Blend.One, Blend.One, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 1.0f, 1.0f, 1.0f, 1.0f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture, TextureID.INPUT_TEXTURE);

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
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.FULLSIZE_TEXTURE_1));
            Expect.Once.On(postProcessor).Method("SetBlendParameters").
                With(BlendOperation.Add, Blend.One, Blend.InvSourceColor, Color.White);
            Expect.Once.On(postProcessor).Method("SetValue").
                With("Color", new float[] { 0.5f, 0.5f, 0.5f, 0.5f });
            Expect.Once.On(postProcessor).Method("Process").
                With("Blend", texture, TextureID.FULLSIZE_TEXTURE_1);

            overlay.Render();
        }

    }
}
