using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.DemoFramework;
using NMock2;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class PerturbationPostEffectTest
    {
        private Mockery mockery;
        private PerturbationPostEffect effect;
        private IPostProcessor postProcessor;
        private Vector4 strength;
        private Vector2 time;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            effect = new PerturbationPostEffect(1.0f, 2.0f);
            effect.Initialize(postProcessor);
            Time.Initialize();
            Time.Pause();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestRenderInputIsInput()
        {
            // Starting with INPUT
            Time.CurrentTime = 11.25f;
            strength = new Vector4(1, 2, 3, 4);
            time = new Vector2(0.25f, 0.7827875f);
            TestRender(TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1);
        }

        [Test]
        public void TestRenderFs1IsInput()
        {
            // Starting with FULLSCREEN_1
            Time.CurrentTime = 1.33f; // 2,3387651
            strength = new Vector4(4, 3, 2, 1);
            time = new Vector2(0.33f, 0.3387651f);
            TestRender(TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
        }

        private void TestRender(TextureID startTexture, TextureID endTexture)
        {
            Stub.On(postProcessor).
                GetProperty("OutputTextureID").
                Will(Return.Value(startTexture));
            effect.Strength = strength;
            //using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("PerturbationStrength", strength);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With(Is.EqualTo("PerturbationTime"), new Vector2Matcher(time));
                Expect.Once.On(postProcessor).
                    Method("Process").With("Perturbate", startTexture, endTexture);
                effect.Render();
            }
        }

        private class Vector2Matcher : Matcher
        {
            private const float epsilon = 0.00001f;
            private Vector2 compareTo;

            public Vector2Matcher(Vector2 compareTo)
            {
                this.compareTo = compareTo;
            }

            public override void DescribeTo(System.IO.TextWriter writer)
            {
                writer.WriteLine(compareTo.ToString());
            }

            public override bool Matches(object o)
            {
                if (!(o is Vector2))
                    return false;
                Vector2 value = (Vector2)o;
                if (Math.Abs(compareTo.X - value.X) > epsilon)
                    return false;
                if (Math.Abs(compareTo.Y - value.Y) > epsilon)
                    return false;
                return true;
            }
        }
    }
}
