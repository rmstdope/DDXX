using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoWindowTest : D3DMockTest
    {
        DemoWindow window;
        IInputFactory iFactory;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            iFactory = mockery.NewMock<Dope.DDXX.Input.IInputFactory>();
            InputDriver.Factory = iFactory;
            Stub.On(iFactory).
                GetProperty("Keyboard").
                Will(Return.Value(new Microsoft.DirectX.DirectInput.Device(SystemGuid.Keyboard)));
            Stub.On(iFactory).
                Method("SetCooperativeLevel");
            Stub.On(iFactory).
                Method("Acquire"); 

            window = new DemoWindow();
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
            desc.deviceType = Microsoft.DirectX.Direct3D.DeviceType.Hardware;
            desc.width = 800;
            desc.height = 600;

            Expect.Once.On(prerequisits).Method("CheckPrerequisits").WithAnyArguments();
            window.Initialize(WindowText, desc, prerequisits);
            Assert.AreEqual(window.Text, WindowText);
            Assert.AreEqual(window.ClientSize.Height, 600);
            Assert.AreEqual(window.ClientSize.Width, 800);
            Assert.AreEqual(window.CompanyName, "Dope Demo Production");
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
            desc.deviceType = Microsoft.DirectX.Direct3D.DeviceType.Hardware;
            desc.width = 800;
            desc.height = 450;

            Expect.Once.On(prerequisits).Method("CheckPrerequisits").WithAnyArguments();
            window.Initialize(WindowText, desc, prerequisits);
            Assert.AreEqual(window.Text, WindowText);
            Assert.AreEqual(window.ClientSize.Height, 450);
            Assert.AreEqual(window.ClientSize.Width, 800);
            Assert.AreEqual(window.CompanyName, "Dope Demo Production");
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
            desc.deviceType = Microsoft.DirectX.Direct3D.DeviceType.Hardware;
            desc.height = 601;

            window.Initialize("nisse", desc, prerequisits);
        }

        [Test]
        public void TestCleanUp()
        {
            TestInitialize_16_9();
            // Dispose is stubbed
            window.CleanUp();
        }
    }
}
