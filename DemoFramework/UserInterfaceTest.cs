using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

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
                To(true);
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

        [Test]
        public void TestDrawBoxControl()
        {
            float x1 = 0.1f;
            float x2 = 0.3f;
            float y1 = 0.2f;
            float y2 = 0.5f;
            float width = presentParameters.BackBufferWidth;
            float height = presentParameters.BackBufferHeight;
            TestInitialize();

            using (mockery.Ordered)
            {
                Expect.Once.On(sprite).
                    Method("Begin").
                    With(SpriteFlags.AlphaBlend);
                Expect.Once.On(sprite).
                    Method("Draw2D").
                    With(Is.EqualTo(texture), Is.EqualTo(Rectangle.Empty), 
                         Is.EqualTo(new SizeF(width * (x2 - x1), height * (y2 - y1))), 
                         Is.EqualTo(new PointF(width * x1, height * y1)),
                         Is.EqualTo(Color.FromArgb((int)(255 * 0.75f), Color.Turquoise)));
                Expect.Once.On(sprite).
                    Method("End");

                Expect.Once.On(line).
                    Method("Begin");
                Expect.Once.On(line).
                    Method("Draw").
                    With(new Vector2[] { new Vector2(width * x1, height * y1), new Vector2(width * x2, height * y1), new Vector2(width * x2, height * y2), new Vector2(width * x1, height * y2), new Vector2(width * x1, height * y1) }, Color.White);
                Expect.Once.On(line).
                    Method("End");
            }
            ui.DrawControl(new BoxControl(x1, y1, x2, y2, 0.75f, Color.Turquoise));
        }

        [Test]
        public void TestTextBoxControl()
        {
            float x1 = 0.1f;
            float y1 = 0.2f;
            float width = presentParameters.BackBufferWidth;
            float height = presentParameters.BackBufferHeight;
            TestInitialize();

            Expect.Once.On(font).
                Method("DrawText").
                With(null, "Text", (int)(width * x1), (int)(height * y1), Color.FromArgb((int)(255 * 0.75f), Color.Turquoise)).
                Will(Return.Value(0));

            ui.DrawControl(new TextControl("Text", x1, y1, 0.75f, Color.Turquoise));
        }
    }
}
