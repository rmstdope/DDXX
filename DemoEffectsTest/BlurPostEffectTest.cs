using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class BlurPostEffectTest : DemoMockTest
    {
        private BlurPostEffect sut;
        private IRenderTarget2D outputTexture;
        private IRenderTarget2D texture1;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            outputTexture = mockery.NewMock<IRenderTarget2D>();
            texture1 = mockery.NewMock<IRenderTarget2D>();
            sut = new BlurPostEffect("", 1.0f, 2.0f);
            sut.Initialize(graphicsFactory, postProcessor, textureFactory, textureBuilder);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        /// <summary>
        /// Test running the post effect with INPUT_TEXTURE as input.
        /// </summary>
        [Test]
        public void TestInput()
        {
            TestRender(outputTexture, texture1);
        }

        /// <summary>
        /// Test running the post effect with three blur passes.
        /// </summary>
        [Test]
        public void TestThreePasses()
        {
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(texture1);
            textures.Add(outputTexture);
            sut.NumPasses = 3;
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(outputTexture));
            // Due to some strange bug in NMock we can not put all Process calls in the same Ordered block.
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("GetTemporaryTextures").
                    With(2, false).Will(Return.Value(textures));
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("BloomScale", 1.0f);
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
            }
            for (int i = 0; i < 3; i++)
            {
                using (mockery.Ordered)
                {
                    Expect.Once.On(postProcessor).
                        Method("Process").
                        With("HorizontalBloom", outputTexture, texture1);
                    Expect.Once.On(postProcessor).
                        Method("Process").
                        With("VerticalBloom", texture1, outputTexture);
                }
            }
            sut.Render();
        }


        private void TestRender(IRenderTarget2D startTexture, IRenderTarget2D tempTexture1)
        {
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(tempTexture1);
            textures.Add(startTexture);
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(startTexture));
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("GetTemporaryTextures").
                    With(2, false).Will(Return.Value(textures));
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("BloomScale", 1.0f);
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("HorizontalBloom", startTexture, tempTexture1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("VerticalBloom", tempTexture1, startTexture);
            }
            sut.Render();
        }
    }
}
