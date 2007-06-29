using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using NMock2;
using Microsoft.DirectX.Direct3D;
using System.IO;
using System.Drawing;
using Microsoft.DirectX;
using Dope.DDXX.Utility;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class TextFadingEffectTest
    {
        private const int VIEWPORT_WIDTH = 444;
        private const int VIEWPORT_HEIGHT = 666;

        private TextFadingEffect textFadingEffect;

        private Mockery mockery;
        private IGraphicsFactory graphicsFactory;
        private IDevice device;
        private ISprite sprite;
        private IFont font;
        private IDemoMixer mixer;

        private Viewport viewport;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            device = mockery.NewMock<IDevice>();
            sprite = mockery.NewMock<ISprite>();
            font = mockery.NewMock<IFont>();
            mixer = mockery.NewMock<IDemoMixer>();
            viewport = new Viewport();
            viewport.Width = VIEWPORT_WIDTH;
            viewport.Height = VIEWPORT_HEIGHT;
            Stub.On(device).GetProperty("Viewport").Will(Return.Value(viewport));

            Time.Initialize();
            Time.Pause();
            textFadingEffect = new TextFadingEffect(0, 10);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestInitializeArial25()
        {
            InitializeEffect("Arial", 25);
        }

        [Test]
        public void TestInitializeCourier200()
        {
            InitializeEffect("Courier", 200);
        }

        [Test]
        public void TestReInitializeSameData()
        {
            TestInitializeArial25();

            textFadingEffect.FontName = "Arial";
            textFadingEffect.FontHeight = 25;
        }

        [Test]
        public void TestReInitializeDifferentName()
        {
            TestInitializeArial25();

            ExpectFont("Courier", 25);
            textFadingEffect.FontName = "Courier";
        }

        [Test]
        public void TestReInitializeDifferentHeight()
        {
            TestInitializeArial25();

            ExpectFont("Arial", 125);
            textFadingEffect.FontHeight = 125;
        }

        [Test]
        public void TestRenderCenter()
        {
            TestInitializeArial25();

            textFadingEffect.TextPosition = new Vector2(0.5f, 0.5f);
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.Anything, Is.EqualTo(new Rectangle(VIEWPORT_WIDTH / 2 - 500, VIEWPORT_HEIGHT / 2 - 500, 1000, 1000)),
                Is.EqualTo(DrawTextFormat.Center | DrawTextFormat.VerticalCenter), Is.Anything).
                Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestRenderTopLeft()
        {
            TestInitializeArial25();

            textFadingEffect.TextPosition = new Vector2(0.0f, 0.0f);
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.Anything, Is.EqualTo(new Rectangle(-500, -500, 1000, 1000)),
                Is.EqualTo(DrawTextFormat.Center | DrawTextFormat.VerticalCenter), Is.Anything).
                Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestRenderLowerRight()
        {
            TestInitializeArial25();

            textFadingEffect.TextPosition = new Vector2(1.0f, 1.0f);
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.Anything, Is.EqualTo(new Rectangle(VIEWPORT_WIDTH - 500, VIEWPORT_HEIGHT - 500, 1000, 1000)), 
                Is.EqualTo(DrawTextFormat.Center | DrawTextFormat.VerticalCenter), Is.Anything).
                Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestText1()
        {
            TestInitializeArial25();

            textFadingEffect.Text = "Text1";
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.EqualTo("Text1"), Is.Anything, Is.Anything, 
                Is.Anything).Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestText2()
        {
            TestInitializeArial25();

            textFadingEffect.Text = "Text2";
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.EqualTo("Text2"), Is.Anything, Is.Anything,
                Is.Anything).Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestFadeInStart()
        {
            TestInitializeArial25();

            Time.CurrentTime = 0.0f;
            textFadingEffect.FadeInLength = 2.0f;
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.Anything, Is.Anything, Is.Anything,
                Is.EqualTo(Color.FromArgb(0, Color.White))).Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestFadeInHalf()
        {
            TestInitializeArial25();

            Time.CurrentTime = 1.0f;
            textFadingEffect.FadeInLength = 2.0f;
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.Anything, Is.Anything, Is.Anything,
                Is.EqualTo(Color.FromArgb(127, Color.White))).Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestFullyFaded()
        {
            TestInitializeArial25();

            Time.CurrentTime = 4.0f;
            textFadingEffect.FadeInLength = 2.0f;
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.Anything, Is.Anything, Is.Anything,
                Is.EqualTo(Color.FromArgb(255, Color.White))).Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestFadeOutHalf()
        {
            TestInitializeArial25();

            Time.CurrentTime = 9.0f;
            textFadingEffect.FadeOutLength = 2.0f;
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.Anything, Is.Anything, Is.Anything,
                Is.EqualTo(Color.FromArgb(127, Color.White))).Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestFadeOutEnd()
        {
            TestInitializeArial25();

            Time.CurrentTime = 10.0f;
            textFadingEffect.FadeOutLength = 2.0f;
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.Anything, Is.Anything, Is.Anything,
                Is.EqualTo(Color.FromArgb(0, Color.White))).Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        [Test]
        public void TestColorRed()
        {
            TestInitializeArial25();

            textFadingEffect.TextColor = Color.Red;
            Expect.Once.On(sprite).Method("Begin").With(SpriteFlags.AlphaBlend);
            Expect.Once.On(font).Method("DrawText").
                With(Is.EqualTo(sprite), Is.Anything, Is.Anything, Is.Anything,
                Is.EqualTo(Color.FromArgb(255, Color.Red))).Will(Return.Value(0));
            Expect.Once.On(sprite).Method("End");
            textFadingEffect.Render();
        }

        private void InitializeEffect(string fontName, int fontHeight)
        {
            textFadingEffect.FontName = fontName;
            textFadingEffect.FontHeight = fontHeight;
            ExpectSprite();
            ExpectFont(fontName, fontHeight);
            textFadingEffect.Initialize(graphicsFactory, null, device, mixer);
        }

        private void ExpectFont(string fontName, int fontHeight)
        {
            FontDescription description = new FontDescription();
            description.FaceName = fontName;
            description.Height = fontHeight;
            description.Quality = FontQuality.AntiAliased;
            //description.Weight = FontWeight.Bold;
            Expect.Once.On(graphicsFactory).
                Method("CreateFont").
                With(Is.EqualTo(device), new DescMatcher(description)).Will(Return.Value(font));
        }

        private void ExpectSprite()
        {
            Expect.Once.On(graphicsFactory).
                Method("CreateSprite").Will(Return.Value(sprite));
        }

        private class DescMatcher : Matcher
        {
            private FontDescription desc;

            public DescMatcher(FontDescription desc)
            {
                this.desc = desc;
            }

            public override void DescribeTo(TextWriter writer)
            {
                writer.WriteLine(desc.ToString());
            }

            public override bool Matches(object o)
            {
                if (!(o is FontDescription))
                    return false;
                FontDescription desc2 = (FontDescription)o;
                return desc.CharSet == desc.CharSet && desc.FaceName == desc2.FaceName &&
                    desc.Height == desc2.Height && desc.IsItalic == desc2.IsItalic &&
                    desc.MipLevels == desc2.MipLevels && desc.OutputPrecision == desc2.OutputPrecision &&
                    desc.Quality == desc2.Quality && desc.Weight == desc2.Weight && desc.Width == desc2.Width;
            }
        }
}
}
