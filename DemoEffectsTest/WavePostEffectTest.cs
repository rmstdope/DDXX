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
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class WavePostEffectTest
    {
        private Mockery mockery;
        private WavePostEffect effect;
        private IPostProcessor postProcessor;
        private ITexture outputTexture;
        private ITexture texture1;
        private Vector4 strength;
        private Vector2 time;
        private float scale;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            outputTexture = mockery.NewMock<ITexture>();
            texture1 = mockery.NewMock<ITexture>();
            effect = new WavePostEffect(1.0f, 2.0f);
            effect.Initialize(postProcessor, null, null, null);
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
            Time.CurrentTime = 11.25f;
            strength = new Vector4(1, 2, 3, 4);
            time = new Vector2(0.25f, 0.7827875f);
            scale = 15.0f;
            TestRender(outputTexture, texture1);
        }

        [Test]
        public void TestRenderFs1IsInput()
        {
            Time.CurrentTime = 1.33f; // 2,3387651
            strength = new Vector4(4, 3, 2, 1);
            time = new Vector2(0.33f, 0.3387651f);
            scale = 1.0f;
            TestRender(outputTexture, texture1);
        }

        private void TestRender(ITexture startTexture, ITexture endTexture)
        {
            List<ITexture> textures = new List<ITexture>();
            textures.Add(endTexture);
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(startTexture));
            effect.Strength = strength;
            effect.Scale = scale;
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("GetTemporaryTextures").
                    With(1, false).Will(Return.Value(textures));
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("WaveStrength", strength);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("WaveScale", scale);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With(Is.EqualTo("WaveTime"), new Vector2Matcher(time));
                Expect.Once.On(postProcessor).
                    Method("Process").With("Wave", startTexture, endTexture);
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
