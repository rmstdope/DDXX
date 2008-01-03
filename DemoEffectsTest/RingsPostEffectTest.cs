using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class RingsPostEffectTest : DemoMockTest
    {
        private RingsPostEffect sut;
        private Vector2 time;
        private float distance;
        private float scale;
        private IRenderTarget2D texture1;
        private IRenderTarget2D texture2;
        private IRenderTarget2D outputTexture;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            texture1 = mockery.NewMock<IRenderTarget2D>();
            texture2 = mockery.NewMock<IRenderTarget2D>();
            outputTexture = mockery.NewMock<IRenderTarget2D>();
            sut = new RingsPostEffect("", 1.0f, 2.0f);
            sut.Initialize(graphicsFactory, postProcessor, textureFactory, textureBuilder);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
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

        private void TestRender(IRenderTarget2D startTexture, IRenderTarget2D endTexture)
        {
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(endTexture);
            Stub.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(startTexture));
            sut.Distance = distance;
            sut.Scale = scale;
            using (mockery.Ordered)
            {
                Expect.Once.On(postProcessor).
                    Method("GetTemporaryTextures").
                    With(1, false).Will(Return.Value(textures));
                Expect.Once.On(postProcessor).
                    Method("SetBlendParameters").
                    With(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("RingsDistance", distance);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With("RingsScale", scale);
                Expect.Once.On(postProcessor).
                    Method("SetValue").With(Is.EqualTo("WaveTime"), new Vector2Matcher(time));
                Expect.Once.On(postProcessor).
                    Method("Process").With("Rings", startTexture, endTexture);
                sut.Render();
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
