using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.IO;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class UserInterfaceTest : D3DMockTest
    {
        private ILine line;
        private ISprite sprite;
        private UserInterface ui;
        private IFont font;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            line = mockery.NewMock<ILine>();
            sprite = mockery.NewMock<ISprite>();
            font = mockery.NewMock<IFont>();

            ui = new UserInterface();

            SetupD3DDriver();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitialize()
        {
            Expect.Once.On(factory).
                Method("CreateLine").
                WithAnyArguments().
                Will(Return.Value(line));
            Expect.Once.On(line).
                SetProperty("Width").
                To(1.0f);
            Expect.Once.On(line).
                SetProperty("Antialias").
                To(false);
            Expect.Once.On(factory).
                Method("CreateSprite").
                WithAnyArguments().
                Will(Return.Value(sprite));
            Expect.Once.On(factory).
                Method("CreateFont").
                WithAnyArguments().
                Will(Return.Value(font));
            Expect.Once.On(factory).
                Method("CreateTexture").
                WithAnyArguments().
                Will(Return.Value(texture));
            Expect.Once.On(device).
                Method("ColorFill").
                With(surface, new Rectangle(0, 0, 1, 1), Color.White);

            ui.Initialize();
        }

        class SizeFMatcher : Matcher
        {
            private SizeF size;
            public SizeFMatcher(SizeF size)
            {
                this.size = size;
            }
            public override void DescribeTo(TextWriter writer)
            {
                writer.WriteLine("Matching");
            }
            public override bool Matches(object o)
            {
                float epsilon = 1e-4f;
                if (!(o is SizeF))
                    return false;
                SizeF newSize = (SizeF)o;
                Assert.AreEqual(size.Height, newSize.Height, epsilon);
                Assert.AreEqual(size.Width, newSize.Width, epsilon);
                return true;
            }
        }

        class Vector2ArrayMatcher : Matcher
        {
            private Vector2[] vector;
            public Vector2ArrayMatcher(Vector2[] vector)
            {
                this.vector = vector;
            }
            public override void DescribeTo(TextWriter writer)
            {
                writer.WriteLine("Matching");
            }
            public override bool Matches(object o)
            {
                float epsilon = 1e-4f;
                if (!(o is Vector2[]))
                    return false;
                Vector2[] newVec = (Vector2[])o;
                Assert.AreEqual(vector.Length, newVec.Length);
                for (int i = 0; i < vector.Length; i++)
                {
                    Assert.AreEqual(vector[i].X, newVec[i].X, epsilon);
                    Assert.AreEqual(vector[i].Y, newVec[i].Y, epsilon);
                }
                return true;
            }
        }

        [Test]
        public void TestDrawBoxControl()
        {
            float x1 = 0.1f;
            float x2 = 0.4f;
            float y1 = 0.2f;
            float y2 = 0.8f;
            float width = presentParameters.BackBufferWidth;
            float height = presentParameters.BackBufferHeight;
            float x11 = x1 * width;
            float x21 = x2 * width;
            float y11 = y1 * height;
            float y21 = y2 * height;
            float x12 = x11 + x1 * (x21 - x11);
            float x22 = x11 + x2 * (x21 - x11);
            float y12 = y11 + y1 * (y21 - y11);
            float y22 = y11 + y2 * (y21 - y11);
            TestInitialize();

            BoxControl box1 = new BoxControl(new RectangleF(x1, y1, x2 - x1, y2 - y1), 0.75f, Color.Turquoise, null);
            BoxControl box2 = new BoxControl(new RectangleF(x1, y1, x2 - x1, y2 - y1), 0.75f, Color.Turquoise, box1);

            ExpectBox(x11, x21, y11, y21);
            ExpectBox(x12, x22, y12, y22);

            ui.DrawControl(box1);
        }

        [Test]
        public void TestDrawBlackBoxControl()
        {
            float x1 = 0.1f;
            float x2 = 0.4f;
            float y1 = 0.2f;
            float y2 = 0.8f;
            float width = presentParameters.BackBufferWidth;
            float height = presentParameters.BackBufferHeight;
            float x11 = x1 * width;
            float x21 = x2 * width;
            float y11 = y1 * height;
            float y21 = y2 * height;
            float x12 = x11 + x1 * (x21 - x11);
            float x22 = x11 + x2 * (x21 - x11);
            float y12 = y11 + y1 * (y21 - y11);
            float y22 = y11 + y2 * (y21 - y11);
            TestInitialize();

            BoxControl box1 = new BoxControl(new RectangleF(x1, y1, x2 - x1, y2 - y1), 0.0f, Color.Turquoise, null);
            BoxControl box2 = new BoxControl(new RectangleF(x1, y1, x2 - x1, y2 - y1), 0.75f, Color.Turquoise, box1);

            ExpectBox(x12, x22, y12, y22);

            ui.DrawControl(box1);
        }

        [Test]
        public void TestTextControl()
        {
            float x1 = 0.1f;
            float x2 = 0.3f;
            float y1 = 0.2f;
            float y2 = 0.5f;
            float width = presentParameters.BackBufferWidth;
            float height = presentParameters.BackBufferHeight;
            TestInitialize();

            ExpectText(x1, x2, y1, y2, width, height);

            ui.DrawControl(new TextControl("Text", new RectangleF(x1, y1, x2, y2), DrawTextFormat.Center, 0.75f, Color.Turquoise, null));
        }

        [Test]
        public void TestLineControl()
        {
            float x1 = 0.1f;
            float x2 = 0.3f;
            float y1 = 0.2f;
            float y2 = 0.5f;
            float width = presentParameters.BackBufferWidth;
            float height = presentParameters.BackBufferHeight;
            TestInitialize();

            ExpectLine(x1 * width, (x2 + x1) * width, y1 * height, (y2 + y1) * height);

            ui.DrawControl(new LineControl(new RectangleF(x1, y1, x2, y2), 0.75f, Color.Turquoise, null));
        }

        private void ExpectText(float x1, float x2, float y1, float y2, float width, float height)
        {
            Expect.Once.On(font).
                Method("DrawText").
                With(null, "Text", new Rectangle((int)(x1 * width) + 1, (int)(y1 * height) + 1, (int)(x2 * width), (int)(y2 * height)), DrawTextFormat.Center, Color.FromArgb((int)(255 * 0.75f), Color.Black)).
                Will(Return.Value(0));
            Expect.Once.On(font).
                Method("DrawText").
                With(null, "Text", new Rectangle((int)(x1 * width), (int)(y1 * height), (int)(x2 * width), (int)(y2 * height)), DrawTextFormat.Center, Color.FromArgb((int)(255 * 0.75f), Color.Turquoise)).
                Will(Return.Value(0));
        }

        private void ExpectLine(float x1, float x2, float y1, float y2)
        {
            using (mockery.Ordered)
            {
                Expect.Once.On(line).
                    Method("Begin");
                Expect.Once.On(line).
                    Method("Draw").
                    With(new Vector2ArrayMatcher(new Vector2[] { new Vector2(x1 + 1, y1 + 1), new Vector2(x2 + 1, y2 + 1) }), Is.EqualTo(Color.FromArgb((int)(255 * 0.75f), Color.Black)));
                Expect.Once.On(line).
                    Method("Draw").
                    With(new Vector2ArrayMatcher(new Vector2[] { new Vector2(x1, y1), new Vector2(x2, y2) }), Is.EqualTo(Color.FromArgb((int)(255 * 0.75f), Color.White)));
                Expect.Once.On(line).
                    Method("End");
            }
        }

        private void ExpectBox(float x1, float x2, float y1, float y2)
        {
            using (mockery.Ordered)
            {
                Expect.Once.On(sprite).
                    Method("Begin").
                    With(SpriteFlags.AlphaBlend);
                Expect.Once.On(sprite).
                    Method("Draw2D").
                    With(Is.EqualTo(texture), Is.EqualTo(Rectangle.Empty),
                         new SizeFMatcher(new SizeF((x2 - x1), (y2 - y1))),
                         Is.EqualTo(new PointF(x1, y1)),
                         Is.EqualTo(Color.FromArgb((int)(255 * 0.75f), Color.Turquoise)));
                Expect.Once.On(sprite).
                    Method("End");

                Expect.Once.On(line).
                    Method("Begin");
                Expect.Once.On(line).
                    Method("Draw").
                    With(new Vector2ArrayMatcher(new Vector2[] { new Vector2(x1 + 1, y1 + 1), new Vector2(x2 + 1, y1 + 1), new Vector2(x2 + 1, y2 + 1), new Vector2(x1 + 1, y2 + 1), new Vector2(x1 + 1, y1 + 1) }), Is.EqualTo(Color.FromArgb((int)(255 * 0.75f), Color.Black)));
                Expect.Once.On(line).
                    Method("Draw").
                    With(new Vector2ArrayMatcher(new Vector2[] { new Vector2(x1, y1), new Vector2(x2, y1), new Vector2(x2, y2), new Vector2(x1, y2), new Vector2(x1, y1) }), Is.EqualTo(Color.FromArgb((int)(255 * 0.75f), Color.White)));
                Expect.Once.On(line).
                    Method("End");
            }
        }

    }
}
