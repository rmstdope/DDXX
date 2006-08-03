using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Direct3D;
using Utility;
using NUnit.Framework;
using NMock2;

namespace DemoFramework
{
    [TestFixture]
    public class DemoWindowTest
    {
        DemoWindow window;
        private Mockery mockery;
        private IFactory factory;
        private IDevice device;
        private IManager manager;
        private DisplayMode displayMode = new DisplayMode();

        [SetUp]
        public void Setup()
        {
            displayMode.Width = 800;
            displayMode.Height = 600;
            displayMode.Format = Format.R8G8B8G8;

            mockery = new Mockery();
            factory = mockery.NewMock<IFactory>();
            device = mockery.NewMock<IDevice>();
            manager = mockery.NewMock<IManager>();
            Stub.On(factory).
                Method("CreateManager").
                Will(Return.Value(manager));
            Stub.On(factory).
                Method("CreateDevice").
                WithAnyArguments().
                Will(Return.Value(device));
            Expect.Once.On(manager).
                Method("CurrentDisplayMode").
                With(0).
                Will(Return.Value(displayMode));

            window = new DemoWindow(factory);
        }

        [TearDown]
        public void TearDown()
        {
            Expect.Once.On(device).
                Method("Dispose");
            D3DDriver.DestroyInstance();
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestInitialize_4_3()
        {
            string WindowText = "4_3_Window";
            DeviceDescription desc = new DeviceDescription();
            desc.deviceType = DeviceType.Hardware;
            desc.width = 800;
            desc.height = 600;

            window.Initialize(WindowText, desc);
            Assert.AreEqual(window.Text, WindowText);
            Assert.AreEqual(window.ClientSize.Height, 600);
            Assert.AreEqual(window.ClientSize.Width, 800);
            Assert.AreEqual(window.CompanyName, "Dope");
            Assert.IsTrue(window.Created);
            Assert.IsTrue(window.Enabled);
            Assert.IsTrue(window.Visible);
            Assert.AreEqual(window.AspectRatio.Ratio, AspectRatio.Ratios.RATIO_4_3);
        }

        [Test]
        public void TestInitialize_16_9()
        {
            string WindowText = "16_9_Window";
            DeviceDescription desc = new DeviceDescription();
            desc.deviceType = DeviceType.Hardware;
            desc.width = 800;
            desc.height = 450;

            window.Initialize(WindowText, desc);
            Assert.AreEqual(window.Text, WindowText);
            Assert.AreEqual(window.ClientSize.Height, 450);
            Assert.AreEqual(window.ClientSize.Width, 800);
            Assert.AreEqual(window.CompanyName, "Dope");
            Assert.IsTrue(window.Created);
            Assert.IsTrue(window.Enabled);
            Assert.IsTrue(window.Visible);
            Assert.AreEqual(window.AspectRatio.Ratio, AspectRatio.Ratios.RATIO_16_9);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail()
        {
            DeviceDescription desc = new DeviceDescription();
            desc.deviceType = DeviceType.Hardware;
            desc.height = 601;

            window.Initialize("nisse", desc);
        }
    }
}
