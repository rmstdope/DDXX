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
    public class RingsPostEffectTest
    {
        private Mockery mockery;
        private RingsPostEffect effect;
        private IPostProcessor postProcessor;
        private Vector2 time;
        private float distance;
        private float scale;
        private ITexture texture1;
        private ITexture texture2;
        private ITexture outputTexture;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            postProcessor = mockery.NewMock<IPostProcessor>();
            texture1 = mockery.NewMock<ITexture>();
            texture2 = mockery.NewMock<ITexture>();
            outputTexture = mockery.NewMock<ITexture>();
            effect = new RingsPostEffect(1.0f, 2.0f);
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
            time = new Vector2(0.25f, 0.7827875f);
            distance = 5;
            scale = 15.0f;
            TestRender(texture1, texture2);
        }

        [Test]
        public void TestRenderFs1IsInput()
        {
            Time.CurrentTime = 1.33f; // 2,3387651
            time = new Vector2(0.33f, 0.3387651f);
            distance = 2;
            scale = 1.0f;
            TestRender(texture1, texture2);
        }

        private void TestRender(ITexture startTexture, ITexture endTexture)
        {
            List<ITexture> textures = new List<ITexture>();
            textures.Add(endTexture);
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(startTexture));
            effect.Distance = distance;
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
                    Method("SetValue").With("RingsDistance", distance);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("RingsScale", scale);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With(Is.EqualTo("WaveTime"), new Vector2Matcher(time));
                Expect.Once.On(postProcessor).
                    Method("Process").With("Rings", startTexture, endTexture);
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
