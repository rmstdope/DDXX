using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.DemoFramework;
using System.Drawing;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class GlowPostEffectTest
    {
        private Mockery mockery;
        private GlowPostEffect effect;
        private IPostProcessor postProcessor;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            effect = new GlowPostEffect(1.0f, 2.0f);
            effect.Initialize(postProcessor);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestRender1()
        {
            // Starting with INPUT
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.INPUT_TEXTURE));
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("DownSample4x", TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("DownSample4x", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("Brighten", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("HorizontalBloom", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("VerticalBloom", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendOperation.Add, Blend.One, Blend.One, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", TextureID.FULLSIZE_TEXTURE_2, TextureID.INPUT_TEXTURE);
            }
            effect.Render();
        }

        [Test]
        public void TestRender2()
        {
            // Starting with FULLSCREEN_1
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.FULLSIZE_TEXTURE_1));
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("DownSample4x", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("DownSample4x", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_3);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("Brighten", TextureID.FULLSIZE_TEXTURE_3, TextureID.FULLSIZE_TEXTURE_2);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("HorizontalBloom", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_3);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("VerticalBloom", TextureID.FULLSIZE_TEXTURE_3, TextureID.FULLSIZE_TEXTURE_2);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_3);
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendOperation.Add, Blend.One, Blend.One, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", TextureID.FULLSIZE_TEXTURE_3, TextureID.FULLSIZE_TEXTURE_1);
            }
            effect.Render();
        }

        [Test]
        public void TestRender3()
        {
            // Starting with FULLSCREEN_2
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(TextureID.FULLSIZE_TEXTURE_2));
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("DownSample4x", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("DownSample4x", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_3);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("Brighten", TextureID.FULLSIZE_TEXTURE_3, TextureID.FULLSIZE_TEXTURE_1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("HorizontalBloom", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_3);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("VerticalBloom", TextureID.FULLSIZE_TEXTURE_3, TextureID.FULLSIZE_TEXTURE_1);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_3);
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendOperation.Add, Blend.One, Blend.One, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("Process").
                    With("UpSample4x", TextureID.FULLSIZE_TEXTURE_3, TextureID.FULLSIZE_TEXTURE_2);
            }
            effect.Render();
        }
    }
}
