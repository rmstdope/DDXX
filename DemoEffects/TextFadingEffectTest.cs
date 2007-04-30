using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using NMock2;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoEffects
{
    [TestFixture]
    public class TextFadingEffectTest
    {
        private TextFadingEffect textFadingEffect;

        private Mockery mockery;
        private IGraphicsFactory graphicsFactory;
        private IDevice device;
        private ISprite sprite;
        private IFont font;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            device = mockery.NewMock<IDevice>();
            sprite = mockery.NewMock<ISprite>();
            font = mockery.NewMock<IFont>();

            textFadingEffect = new TextFadingEffect(0, 10);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestInitialize()
        {
            FontDescription description = new FontDescription();
            description.FaceName = "Arial";
            description.Height = 25;
            Expect.Once.On(graphicsFactory).
                Method("CreateSprite").Will(Return.Value(sprite));
            Expect.Once.On(graphicsFactory).
                Method("CreateFont").With(device, description).Will(Return.Value(font));
            textFadingEffect.Initialize(graphicsFactory, device);
        }

        [Test]
        public void TestRender()
        {
            TestInitialize();

            textFadingEffect.Render();
        }

    }
}
