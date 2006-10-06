using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using System.Drawing;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class UserInterfaceTest : D3DMockTest
    {
        private ILine line;
        private ISprite sprite;
        private UserInterface ui;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            line = mockery.NewMock<ILine>();
            sprite = mockery.NewMock<ISprite>();

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
            Expect.Once.On(factory).
                Method("CreateLine").
                WithAnyArguments().
                Will(Return.Value(line));
            Expect.Once.On(factory).
                Method("CreateSprite").
                WithAnyArguments().
                Will(Return.Value(sprite));
            Expect.Once.On(factory).
                Method("CreateTexture").
                WithAnyArguments().
                Will(Return.Value(texture));
            //Expect.Once.On(device).
            //    Method("ColorFill").
            //    With(surface, new Rectangle(0, 0, 1, 1), Color.White);

            ui.Initialize();
        }

        [Test]
        public void TestDrawBox()
        {
            float width = presentParameters.BackBufferWidth;
            float height = presentParameters.BackBufferHeight;
            TestInitialize();

            using (mockery.Ordered)
            {
                Expect.Once.On(sprite).
                    Method("Begin").
                    With(SpriteFlags.AlphaBlend);
                //Expect.Once.On(sprite).
                //    Method("Draw2D").
                //    With(texture, Rectangle.Empty, new SizeF(width * 0.2f, height * 0.3f), new PointF(width * 0.1f, height * 0.2f), Color.FromArgb((int)(255 * 0.75f), Color.White));
                Expect.Once.On(sprite).
                    Method("End");
            }
            ui.DrawBox(0.1f, 0.2f, 0.3f, 0.5f, 0.75f, Color.Turquoise);
        }

    }
}
