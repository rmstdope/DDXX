using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class UserInterfaceTest : D3DMockTest
    {
        private UserInterface ui;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            ui = new UserInterface();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitialize()
        {
            Expect.Once.On(graphicsFactory).
                Method("CreateSpriteBatch").
                Will(Return.Value(spriteBatch));
            Expect.Once.On(graphicsFactory).
                Method("SpriteFontFromFile").
                With("Content/fonts/TweakerFont").
                Will(Return.Value(spriteFont));
            Expect.Once.On(textureFactory).
                GetProperty("WhiteTexture").
                Will(Return.Value(texture2D));

            ui.Initialize(graphicsFactory, textureFactory);
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
            int x11 = (int)(x1 * width);
            int x21 = (int)(x2 * width);
            int y11 = (int)(y1 * height);
            int y21 = (int)(y2 * height);
            int x12 = (int)(x11 + x1 * (x21 - x11));
            int x22 = (int)(x11 + x2 * (x21 - x11));
            int y12 = (int)(y11 + y1 * (y21 - y11));
            int y22 = (int)(y11 + y2 * (y21 - y11));
            TestInitialize();

            BoxControl box1 = new BoxControl(new Vector4(x1, y1, x2 - x1, y2 - y1), 190, new Color(1, 2, 3), null);
            BoxControl box2 = new BoxControl(new Vector4(x1, y1, x2 - x1, y2 - y1), 190, new Color(4, 5, 6), box1);

            ExpectAlphaBlending();
            ExpectBox(x11, x21 - x11, y11, y21 - y11, new Color(1, 2, 3, 190));
            ExpectBox(x12, x22 - x12, y12, y22 - y12, new Color(4, 5, 6, 190));

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
            int x11 = (int)(x1 * width);
            int x21 = (int)(x2 * width);
            int y11 = (int)(y1 * height);
            int y21 = (int)(y2 * height);
            int x12 = (int)(x11 + x1 * (x21 - x11));
            int x22 = (int)(x11 + x2 * (x21 - x11));
            int y12 = (int)(y11 + y1 * (y21 - y11));
            int y22 = (int)(y11 + y2 * (y21 - y11));
            TestInitialize();

            BoxControl box1 = new BoxControl(new Vector4(x1, y1, x2 - x1, y2 - y1), 0, Color.Turquoise, null);
            BoxControl box2 = new BoxControl(new Vector4(x1, y1, x2 - x1, y2 - y1), 190, new Color(), box1);

            ExpectAlphaBlending();
            ExpectBox(x12, x22 - x12, y12, y22 - y12, new Color(0, 0, 0, 190));

            ui.DrawControl(box1);
        }

        [Test]
        public void TestTextControlTopLeft()
        {
            float x1 = 0.1f;
            float width = 0.3f;
            float y1 = 0.2f;
            float height = 0.5f;
            float screenWidth = presentParameters.BackBufferWidth;
            float screenHeight = presentParameters.BackBufferHeight;
            TestInitialize();

            int x = (int)(x1 * screenWidth);
            int y = (int)(y1 * screenHeight);

            Stub.On(spriteFont).Method("MeasureString").Will(Return.Value(new Vector2(2, 4)));
            ExpectAlphaBlending();
            ExpectText(x, y);

            ui.DrawControl(new TextControl("Text", new Vector4(x1, y1, width, height), TextFormatting.Top, 190, new Color(1, 2, 3), null));
        }

        [Test]
        public void TestTextControlBottomRight()
        {
            float x1 = 0.1f;
            float width = 0.3f;
            float y1 = 0.2f;
            float height = 0.5f;
            float screenWidth = presentParameters.BackBufferWidth;
            float screenHeight = presentParameters.BackBufferHeight;
            TestInitialize();

            int x = (int)((x1 + width) * screenWidth) - 2;
            int y = (int)((y1 + height) * screenHeight) - 4;

            Stub.On(spriteFont).Method("MeasureString").Will(Return.Value(new Vector2(2, 4)));
            ExpectAlphaBlending();
            ExpectText(x, y);

            ui.DrawControl(new TextControl("Text", new Vector4(x1, y1, width, height), TextFormatting.Bottom | TextFormatting.Right, 190, new Color(1, 2, 3), null));
        }

        [Test]
        public void TestTextControlCentered()
        {
            float x1 = 0.1f;
            float width = 0.3f;
            float y1 = 0.2f;
            float height = 0.5f;
            float screenWidth = presentParameters.BackBufferWidth;
            float screenHeight = presentParameters.BackBufferHeight;
            TestInitialize();

            int x = (int)((x1 + width / 2) * screenWidth) - 1;
            int y = (int)((y1 + height / 2) * screenHeight) - 2;

            Stub.On(spriteFont).Method("MeasureString").Will(Return.Value(new Vector2(2, 4)));
            ExpectAlphaBlending();
            ExpectText(x, y);

            ui.DrawControl(new TextControl("Text", new Vector4(x1, y1, width, height), TextFormatting.Center | TextFormatting.VerticalCenter, 190, new Color(1, 2, 3), null));
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestLineControlDiagonal()
        {
            float x1 = 0.1f;
            float x2 = 0.1f;
            float y1 = 0.1f;
            float y2 = 0.1f;
            verifyExpectations = false;
            TestLine(x1, x2, y1, y2);
        }

        [Test]
        public void TestLineControlHorizontal()
        {
            float x1 = 0.1f;
            float x2 = 0.3f;
            float y1 = 0.2f;
            float y2 = 0.0f;
            TestLine(x1, x2, y1, y2);
        }

        [Test]
        public void TestLineControlVertical()
        {
            float x1 = 0.1f;
            float x2 = 0.0f;
            float y1 = 0.2f;
            float y2 = 0.4f;
            TestLine(x1, x2, y1, y2);
        }

        private void TestLine(float x1, float x2, float y1, float y2)
        {
            float width = presentParameters.BackBufferWidth;
            float height = presentParameters.BackBufferHeight;
            TestInitialize();

            ExpectAlphaBlending();
            ExpectLine((int)(x1 * width), (int)(x2 * width), (int)(y1 * height), (int)(y2 * height), new Color(1, 2, 3, 4));

            ui.DrawControl(new LineControl(new Vector4(x1, y1, x2, y2), 4, new Color(1, 2, 3), null));
        }

        private void ExpectText(int x, int y)
        {
            Stub.On(spriteBatch).GetProperty("GraphicsDevice").Will(Return.Value(device));
            using (mockery.Ordered)
            {
                Expect.Once.On(spriteBatch).Method("Begin").
                    With(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
                Expect.Once.On(spriteBatch).Method("DrawString").
                    With(spriteFont, "Text", new Vector2(x + 2, y + 2), new Color(0, 0, 0, 190));
                Expect.Once.On(spriteBatch).Method("DrawString").
                    With(spriteFont, "Text", new Vector2(x, y), new Color(1, 2, 3, 190));
                Expect.Once.On(spriteBatch).
                    Method("End");
            }
            //Expect.Once.On(font).
            //    Method("DrawText").
            //    With(null, "Text", new Rectangle((int)(x1 * width) + 1, (int)(y1 * height) + 1, (int)(x2 * width), (int)(y2 * height)), DrawTextFormat.Center, Color.FromArgb((int)(255 * 0.75f), Color.Black)).
            //    Will(Return.Value(0));
            //Expect.Once.On(font).
            //    Method("DrawText").
            //    With(null, "Text", new Rectangle((int)(x1 * width), (int)(y1 * height), (int)(x2 * width), (int)(y2 * height)), DrawTextFormat.Center, Color.FromArgb((int)(255 * 0.75f), Color.Turquoise)).
            //    Will(Return.Value(0));
        }

        private void ExpectLine(int x, int width, int y, int height, Color color)
        {
            Stub.On(spriteBatch).GetProperty("GraphicsDevice").Will(Return.Value(device));
            using (mockery.Ordered)
            {
                Expect.Once.On(spriteBatch).Method("Begin").
                    With(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
                ExpectLineDrawCall(x, width, y, height, color);
                Expect.Once.On(spriteBatch).
                    Method("End");
            }
        }

        private void ExpectLineDrawCall(int x, int width, int y, int height, Color color)
        {
            if (width == 0)
                width = 1;
            if (height == 0)
                height = 1;
            Expect.Once.On(spriteBatch).
                Method("Draw").
                With(texture2D, new Rectangle(x, y, width, height), color);
        }

        private void ExpectBox(int x, int width, int y, int height, Color color)
        {
            Color color2 = new Color(0, 0, 0, color.A);
            Color color3 = new Color(255, 255, 255, color.A);
            Stub.On(spriteBatch).GetProperty("GraphicsDevice").Will(Return.Value(device));
            using (mockery.Ordered)
            {
                Expect.Once.On(spriteBatch).
                    Method("Begin").
                    With(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
                Expect.Once.On(spriteBatch).
                    Method("Draw").
                    With(texture2D, new Rectangle(x, y, width, height), color);
                Expect.Once.On(spriteBatch).
                    Method("End");

                Expect.Once.On(spriteBatch).
                    Method("Begin").
                    With(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
                ExpectLineDrawCall(x + 1, width, y + 1, 1, color2);
                ExpectLineDrawCall(x + 1, width, y + 1 + height, 1, color2);
                ExpectLineDrawCall(x + 1, 1, y + 1, height, color2);
                ExpectLineDrawCall(x + 1 + width, 1, y + 1, height, color2);
                ExpectLineDrawCall(x, width, y, 1, color3);
                ExpectLineDrawCall(x, width, y + height, 1, color3);
                ExpectLineDrawCall(x, 1, y, height, color3);
                ExpectLineDrawCall(x + width, 1, y, height, color3);
                Expect.Once.On(spriteBatch).
                    Method("End");

            }
        }

        private void ExpectAlphaBlending()
        {
            Expect.Once.On(renderState).SetProperty("AlphaBlendEnable").To(true);
            Expect.Once.On(renderState).SetProperty("BlendFunction").To(BlendFunction.Add);
            Expect.Once.On(renderState).SetProperty("SourceBlend").To(Blend.SourceAlpha);
            Expect.Once.On(renderState).SetProperty("DestinationBlend").To(Blend.InverseSourceAlpha);
        }

    }
}
