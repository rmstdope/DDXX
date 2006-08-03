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
    public class DemoWindowTest : D3DMockTest
    {
        DemoWindow window;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            window = new DemoWindow(factory);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
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
