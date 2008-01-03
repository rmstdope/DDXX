using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using System.IO;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.DemoFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class TextFadingEffectTest : DemoMockTest
    {
        private const int VIEWPORT_WIDTH = 444;
        private const int VIEWPORT_HEIGHT = 666;

        private TextFadingEffect textFadingEffect;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            textFadingEffect = new TextFadingEffect("", 0, 10);
            presentParameters.BackBufferHeight = VIEWPORT_HEIGHT;
            presentParameters.BackBufferWidth = VIEWPORT_WIDTH;
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitializeArial()
        {
            InitializeEffect("Arial");
        }

        [Test]
        public void TestInitializeCourier()
        {
            InitializeEffect("Courier");
        }

        [Test]
        public void TestRenderCenter()
        {
            string text = "Text1";
            TestInitializeArial();

            textFadingEffect.TextPosition = new Vector3(0.5f, 0.5f, 0);
            textFadingEffect.Text = text;
            textFadingEffect.TextColor = new Color(1, 2, 3);
            Stub.On(spriteFont).Method("MeasureString").With(text).
                Will(Return.Value(new Vector2(10, 20)));
            Expect.Once.On(spriteBatch).Method("Begin").WithNoArguments();
            Expect.Once.On(spriteBatch).Method("DrawString").
                With(spriteFont, text, new Vector2(VIEWPORT_WIDTH / 2 - 5, VIEWPORT_HEIGHT / 2 - 10),
                new Color(1, 2, 3, 255));
            Expect.Once.On(spriteBatch).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestRenderTopLeft()
        {
            string text = "Text2";
            TestInitializeArial();

            textFadingEffect.TextPosition = new Vector3(0, 0, 0);
            textFadingEffect.Text = text;
            textFadingEffect.TextColor = new Color(4, 5, 6);
            Stub.On(spriteFont).Method("MeasureString").With(text).
                Will(Return.Value(new Vector2(20, 40)));
            Expect.Once.On(spriteBatch).Method("Begin").WithNoArguments();
            Expect.Once.On(spriteBatch).Method("DrawString").
                With(spriteFont, text, new Vector2(-10, -20),
                new Color(4, 5, 6, 255));
            Expect.Once.On(spriteBatch).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestFadeInStart()
        {
            string text = "Text2";
            TestInitializeArial();

            Time.CurrentTime = 0;
            textFadingEffect.FadeInLength = 2.0f;
            textFadingEffect.TextPosition = new Vector3(0, 0, 0);
            textFadingEffect.Text = text;
            textFadingEffect.TextColor = new Color(4, 5, 6);
            Stub.On(spriteFont).Method("MeasureString").With(text).
                Will(Return.Value(new Vector2(20, 40)));
            Expect.Once.On(spriteBatch).Method("Begin").WithNoArguments();
            Expect.Once.On(spriteBatch).Method("DrawString").
                With(spriteFont, text, new Vector2(-10, -20),
                new Color(4, 5, 6, 0));
            Expect.Once.On(spriteBatch).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestFadeInHalf()
        {
            string text = "Text2";
            TestInitializeArial();

            Time.CurrentTime = 1;
            textFadingEffect.FadeInLength = 2.0f;
            textFadingEffect.TextPosition = new Vector3(0, 0, 0);
            textFadingEffect.Text = text;
            textFadingEffect.TextColor = new Color(4, 5, 6);
            Stub.On(spriteFont).Method("MeasureString").With(text).
                Will(Return.Value(new Vector2(20, 40)));
            Expect.Once.On(spriteBatch).Method("Begin").WithNoArguments();
            Expect.Once.On(spriteBatch).Method("DrawString").
                With(spriteFont, text, new Vector2(-10, -20),
                new Color(4, 5, 6, 127));
            Expect.Once.On(spriteBatch).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestFullyFaded()
        {
            string text = "Text2";
            TestInitializeArial();

            Time.CurrentTime = 4;
            textFadingEffect.FadeInLength = 2.0f;
            textFadingEffect.TextPosition = new Vector3(0, 0, 0);
            textFadingEffect.Text = text;
            textFadingEffect.TextColor = new Color(4, 5, 6);
            Stub.On(spriteFont).Method("MeasureString").With(text).
                Will(Return.Value(new Vector2(20, 40)));
            Expect.Once.On(spriteBatch).Method("Begin").WithNoArguments();
            Expect.Once.On(spriteBatch).Method("DrawString").
                With(spriteFont, text, new Vector2(-10, -20),
                new Color(4, 5, 6, 255));
            Expect.Once.On(spriteBatch).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestFadeOutHalf()
        {
            string text = "Text2";
            TestInitializeArial();

            Time.CurrentTime = 9;
            textFadingEffect.FadeOutLength = 2.0f;
            textFadingEffect.TextPosition = new Vector3(0, 0, 0);
            textFadingEffect.Text = text;
            textFadingEffect.TextColor = new Color(4, 5, 6);
            Stub.On(spriteFont).Method("MeasureString").With(text).
                Will(Return.Value(new Vector2(20, 40)));
            Expect.Once.On(spriteBatch).Method("Begin").WithNoArguments();
            Expect.Once.On(spriteBatch).Method("DrawString").
                With(spriteFont, text, new Vector2(-10, -20),
                new Color(4, 5, 6, 126));
            Expect.Once.On(spriteBatch).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestFadeOutEnd()
        {
            string text = "Text2";
            TestInitializeArial();

            Time.CurrentTime = 10;
            textFadingEffect.FadeOutLength = 2.0f;
            textFadingEffect.TextPosition = new Vector3(0, 0, 0);
            textFadingEffect.Text = text;
            textFadingEffect.TextColor = new Color(4, 5, 6);
            Stub.On(spriteFont).Method("MeasureString").With(text).
                Will(Return.Value(new Vector2(20, 40)));
            Expect.Once.On(spriteBatch).Method("Begin").WithNoArguments();
            Expect.Once.On(spriteBatch).Method("DrawString").
                With(spriteFont, text, new Vector2(-10, -20),
                new Color(4, 5, 6, 255));
            Expect.Once.On(spriteBatch).Method("End");
            textFadingEffect.Render();
        }

        private void InitializeEffect(string fontName)
        {
            textFadingEffect.FontName = fontName;
            ExpectSprite();
            ExpectFont(fontName);
            textFadingEffect.Initialize(graphicsFactory, effectFactory, textureFactory, mixer, postProcessor);
        }

        private void ExpectFont(string fontName)
        {
            Expect.Once.On(graphicsFactory).
                Method("SpriteFontFromFile").
                With(fontName).Will(Return.Value(spriteFont));
        }

        private void ExpectSprite()
        {
            Expect.Once.On(graphicsFactory).
                Method("CreateSpriteBatch").Will(Return.Value(spriteBatch));
        }

    }
}
