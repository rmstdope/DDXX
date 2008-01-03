using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class SetupLogicTest
    {
        private SetupLogic logic;
        private Mockery mockery;
        private ISetupDialog dialog;
        private IGraphicsFactory graphicsFactory;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            dialog = mockery.NewMock<ISetupDialog>();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();

            //supportedDisplayModes = new DisplayMode[5];
            //for (int i = 0; i < 5; i++)
            //{
            //    supportedDisplayModes[i] = new DisplayMode();
            //    supportedDisplayModes[i].Height = 0;
            //    supportedDisplayModes[i].Width = 0;
            //    supportedDisplayModes[3].Format = Format.Unknown;
            //}
            //Stub.On(manager).Method("SupportedDisplayModes").
            //    With(0).Will(Return.Value(supportedDisplayModes));
            //Stub.On(manager).Method("CurrentDisplayMode").
            //    With(0).Will(Return.Value(new DisplayMode()));
            //Stub.On(graphicsFactory).GetProperty("Manager").Will(Return.Value(manager));

            logic = new SetupLogic();
            logic.Dialog = dialog;
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestResolutionWidth100()
        {
            // Setup
            Expect.Once.On(dialog).GetProperty("SelectedResolution").
                Will(Return.Value("100x200"));
            // Exercise SUT and verify
            Assert.AreEqual(100, logic.ResolutionWidth);
        }

        [Test]
        public void TestResolutionWidth50000()
        {
            // Setup
            Expect.Once.On(dialog).GetProperty("SelectedResolution").
                Will(Return.Value("50000x70000"));
            // Exercise SUT and verify
            Assert.AreEqual(50000, logic.ResolutionWidth);
        }

        [Test]
        public void TestResolutionHeight200()
        {
            // Setup
            Expect.Once.On(dialog).GetProperty("SelectedResolution").
                Will(Return.Value("100x200"));
            // Exercise SUT and verify
            Assert.AreEqual(200, logic.ResolutionHeight);
        }

        [Test]
        public void TestResolutionHeight70000()
        {
            // Setup
            Expect.Once.On(dialog).GetProperty("SelectedResolution").
                Will(Return.Value("50000x70000"));
            // Exercise SUT and verify
            Assert.AreEqual(70000, logic.ResolutionHeight);
        }

        //[Test]
        //public void TestDeviceDescription1()
        //{
        //    const Format colorFormat = Format.R5G6B5;
        //    const DeviceType deviceType = DeviceType.Hardware;
        //    const int height = 444;
        //    const int width = 555;
        //    const bool windowed = false;

        //    Stub.On(dialog).GetProperty("REF").Will(Return.Value(false));
        //    Stub.On(dialog).GetProperty("Windowed").Will(Return.Value(windowed));
        //    Stub.On(dialog).GetProperty("SelectedResolution").
        //        Will(Return.Value(width + "x" + height));
        //    Stub.On(dialog).GetProperty("Checked16Bit").Will(Return.Value(true));
        //    supportedDisplayModes[1].Width = width;
        //    supportedDisplayModes[1].Height = height;
        //    supportedDisplayModes[1].Format = colorFormat;

        //    DeviceDescription desc = logic.DeviceDescription;
            
        //    Assert.AreEqual(colorFormat, desc.colorFormat);
        //    Assert.AreEqual(deviceType, desc.deviceType);
        //    Assert.AreEqual(height, desc.height);
        //    Assert.AreEqual(width, desc.width);
        //    Assert.AreEqual(false, desc.useStencil);
        //    Assert.AreEqual(true, desc.useDepth);
        //    Assert.AreEqual(windowed, desc.windowed);
        //}

        //[Test]
        //public void TestDeviceDescription2()
        //{
        //    const Format colorFormat = Format.A8R8G8B8;
        //    const DeviceType deviceType = DeviceType.Reference;
        //    const int height = 777;
        //    const int width = 888;
        //    const bool windowed = true;

        //    Stub.On(dialog).GetProperty("REF").Will(Return.Value(true));
        //    Stub.On(dialog).GetProperty("Windowed").Will(Return.Value(windowed));
        //    Stub.On(dialog).GetProperty("SelectedResolution").
        //        Will(Return.Value(width + "x" + height));
        //    Stub.On(dialog).GetProperty("Checked32Bit").Will(Return.Value(true));
        //    supportedDisplayModes[1].Width = width;
        //    supportedDisplayModes[1].Height = height;
        //    supportedDisplayModes[1].Format = colorFormat;

        //    DeviceDescription desc = logic.DeviceDescription;

        //    Assert.AreEqual(colorFormat, desc.colorFormat);
        //    Assert.AreEqual(deviceType, desc.deviceType);
        //    Assert.AreEqual(height, desc.height);
        //    Assert.AreEqual(width, desc.width);
        //    Assert.AreEqual(false, desc.useStencil);
        //    Assert.AreEqual(true, desc.useDepth);
        //    Assert.AreEqual(windowed, desc.windowed);
        //}

    }
}
