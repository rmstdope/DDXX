using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.DemoFramework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class BlurPostEffectTest
    {
        private Mockery mockery;
        private BlurPostEffect effect;
        private IPostProcessor postProcessor;
        private ITexture outputTexture;
        private ITexture texture1;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            outputTexture = mockery.NewMock<ITexture>();
            texture1 = mockery.NewMock<ITexture>();
            effect = new BlurPostEffect(1.0f, 2.0f);
            effect.Initialize(postProcessor, null, null, null);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
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
            List<ITexture> textures = new List<ITexture>();
            textures.Add(texture1);
            textures.Add(outputTexture);
            effect.NumPasses = 3;
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
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
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
            effect.Render();
        }


        private void TestRender(ITexture startTexture, ITexture tempTexture1)
        {
            List<ITexture> textures = new List<ITexture>();
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
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("HorizontalBloom", startTexture, tempTexture1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("VerticalBloom", tempTexture1, startTexture);
            }
            effect.Render();
        }
    }
}
