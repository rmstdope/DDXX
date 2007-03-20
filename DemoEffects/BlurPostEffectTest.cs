using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.DemoFramework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class BlurPostEffectTest
    {
        private Mockery mockery;
        private BlurPostEffect effect;
        private IPostProcessor postProcessor;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            effect = new BlurPostEffect(1.0f, 2.0f);
            effect.Initialize(postProcessor);
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
            TestRender(TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
        }

        /// <summary>
        /// Test running the post effect with FULLSIZE_TEXTURE_1 as input.
        /// </summary>
        [Test]
        public void TestFullsize1()
        {
            TestRender(TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
        }

        /// <summary>
        /// Test running the post effect with FULLSIZE_TEXTURE_2 as input.
        /// </summary>
        [Test]
        public void TestFullsize2()
        {
            TestRender(TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
        }

        /// <summary>
        /// Test running the post effect with three blur passes.
        /// </summary>
        [Test]
        public void TestThreePasses()
        {
            effect.NumPasses = 3;
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.INPUT_TEXTURE));
            // Due to some strange bug in NMock we can not put all Process calls in the same Ordered block.
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("SetValue").
                    With("BloomScale", 1.0f);
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("HorizontalBloom", TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("VerticalBloom", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
            }
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("HorizontalBloom", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("VerticalBloom", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
            }
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("HorizontalBloom", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("VerticalBloom", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
            }
            effect.Render();
        }


        private void TestRender(TextureID startTexture, TextureID tempTexture1, TextureID tempTexture2)
        {
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(startTexture));
            using (mockery.Ordered)
            {
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
                    With("VerticalBloom", tempTexture1, tempTexture2);
            }
            effect.Render();
        }
    }
}
