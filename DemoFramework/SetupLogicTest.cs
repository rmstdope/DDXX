using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class SetupLogicTest
    {
        private SetupLogic logic;
        private Mockery mockery;
        private ISetupDialog dialog;
        private IManager manager;
        private IGraphicsFactory graphicsFactory;
        private DisplayMode[] supportedDisplayModes;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            dialog = mockery.NewMock<ISetupDialog>();
            manager = mockery.NewMock<IManager>();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();

            D3DDriver.DestroyInstance();
            D3DDriver.Factory = graphicsFactory;

            supportedDisplayModes = new DisplayMode[5];
            for (int i = 0; i < 5; i++)
            {
                supportedDisplayModes[i] = new DisplayMode();
                supportedDisplayModes[i].Height = 0;
                supportedDisplayModes[i].Width = 0;
                supportedDisplayModes[3].Format = Format.Unknown;
            }
            Stub.On(manager).Method("SupportedDisplayModes").
                With(0).Will(Return.Value(supportedDisplayModes));
            Stub.On(manager).Method("CurrentDisplayMode").
                With(0).Will(Return.Value(new DisplayMode()));
            Stub.On(graphicsFactory).GetProperty("Manager").Will(Return.Value(manager));

            logic = new SetupLogic();
            logic.Dialog = dialog;
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestResolutionWidth()
        {
            Expect.Once.On(dialog).GetProperty("SelectedResolution").
                Will(Return.Value("100x200"));
            Assert.AreEqual(100, logic.ResolutionWidth);
            Expect.Once.On(dialog).GetProperty("SelectedResolution").
                Will(Return.Value("50000x70000"));
            Assert.AreEqual(50000, logic.ResolutionWidth);
        }

        [Test]
        public void TestResolutionHeight()
        {
            Expect.Once.On(dialog).GetProperty("SelectedResolution").
                Will(Return.Value("100x200"));
            Assert.AreEqual(200, logic.ResolutionHeight);
            Expect.Once.On(dialog).GetProperty("SelectedResolution").
                Will(Return.Value("50000x70000"));
            Assert.AreEqual(70000, logic.ResolutionHeight);
        }

        private void TestSingleColorFormat(string property, Format format)
        {
            Stub.On(dialog).GetProperty("SelectedResolution").
                Will(Return.Value("100x200"));
            Stub.On(dialog).GetProperty(property).Will(Return.Value(true));

            supportedDisplayModes[3].Width = 100;
            supportedDisplayModes[3].Height = 200;
            supportedDisplayModes[3].Format = format;

            Assert.AreEqual(format, logic.ColorFormat);
        }

        [Test]
        public void TestColorFormatUnknown()
        {
            TestSingleColorFormat("Bit16", Format.Unknown);
        }

        [Test]
        public void TestColorFormatR5G6B5()
        {
            TestSingleColorFormat("Bit16", Format.R5G6B5);
        }

        [Test]
        public void TestColorFormatA8B8G8R8()
        {
            TestSingleColorFormat("Bit32", Format.A8B8G8R8);
        }

        [Test]
        public void TestColorFormatX8B8G8R8()
        {
            TestSingleColorFormat("Bit32", Format.X8B8G8R8);
        }

        [Test]
        public void TestColorFormatX8R8G8B8()
        {
            TestSingleColorFormat("Bit32", Format.X8R8G8B8);
        }

        [Test]
        public void TestColorFormatA8R8G8B8()
        {
            TestSingleColorFormat("Bit32", Format.A8R8G8B8);
        }

        [Test]
        public void TestColorFormatA2R10G10B10()
        {
            TestSingleColorFormat("Bit32", Format.A2R10G10B10);
        }

    }
}
