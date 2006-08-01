using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Utility;

namespace Direct3D
{
    using NUnit.Framework;
    using NMock;

    public class IsEqualPP : NMock.Constraints.BaseConstraint
    {
        PresentParameters param1;

        public IsEqualPP(PresentParameters param)
        {
            param1 = param;
        }

        public override bool Eval(object val)
        {
            PresentParameters param2 = (PresentParameters)val;
            return
                param1.AutoDepthStencilFormat == param2.AutoDepthStencilFormat &&
                param1.BackBufferCount == param2.BackBufferCount &&
                param1.BackBufferFormat == param2.BackBufferFormat &&
                param1.BackBufferHeight == param2.BackBufferHeight &&
                param1.BackBufferWidth == param2.BackBufferWidth &&
                param1.DeviceWindow == param2.DeviceWindow &&
                param1.DeviceWindowHandle == param2.DeviceWindowHandle &&
                param1.EnableAutoDepthStencil == param2.EnableAutoDepthStencil &&
                param1.ForceNoMultiThreadedFlag == param2.ForceNoMultiThreadedFlag &&
                param1.FullScreenRefreshRateInHz == param2.FullScreenRefreshRateInHz &&
                param1.MultiSample == param2.MultiSample &&
                param1.MultiSampleQuality == param2.MultiSampleQuality &&
                param1.PresentationInterval == param2.PresentationInterval &&
                param1.PresentFlag == param2.PresentFlag &&
                param1.SwapEffect == param2.SwapEffect &&
                param1.Windowed == param2.Windowed;
        }

        public override string Message
        {
            get { return "<" + param1 + ">"; }
        }
    }

    [TestFixture]
    public class D3DDriverTest
    {
        D3DDriver driver;
        DynamicMock mockFactory;
        DynamicMock mockDevice;

        [SetUp]
        public void Setup()
        {
            mockFactory = new DynamicMock(typeof(IFactory));
            mockDevice = new DynamicMock(typeof(IDevice));
            //mockFactory.SetupResult("CreateDevice", (IDevice)mockDevice.MockInstance, typeof(int), typeof(DeviceType), typeof(Control), typeof(CreateFlags), typeof(PresentParameters));
            D3DDriver.SetFactory((IFactory)mockFactory.MockInstance);
            driver = D3DDriver.GetInstance();
        }

        [TearDown]
        public void Teardown()
        {
            driver.Reset();
            driver = null;
        }

        [Test]
        public void SingletonTest()
        {
            D3DDriver driver2 = D3DDriver.GetInstance();

            Assert.AreSame(driver, driver2);
        }

        [Test]
        public void AdapterTest()
        {
            DisplayMode[] modes = driver.GetDisplayModes(0, delegate(DisplayMode mode) { return true; });
            Assert.AreEqual(2, driver.NumAdapters);
        }

        private DeviceDescription CreateDescription()
        {
            DeviceDescription desc = new DeviceDescription();
            desc.width = 800;
            desc.height = 600;
            desc.colorFormat = Format.X8R8G8B8;
            return desc;
        }

        [Test]
        public void InitTestFail1()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();

            // Test undefined Software device type
            try
            {
                desc.deviceType = DeviceType.Software;
                desc.windowed = true;
                param.Windowed = true;
                param.SwapEffect = SwapEffect.Discard;
                driver.Init(null, desc);
                Assert.Fail();
            }
            catch (DDXXException)
            {
                Assert.AreEqual(null, driver.GetDevice());
            }
            mockFactory.Verify();
        }

        [Test]
        public void InitTestFail2()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();

            // Test faked DX exception
            try
            {
                desc.deviceType = DeviceType.Reference;
                desc.windowed = true;
                param.Windowed = true;
                param.SwapEffect = SwapEffect.Discard;
                mockFactory.ExpectAndThrow("CreateDevice", new DirectXException(), 0, desc.deviceType, null, CreateFlags.SoftwareVertexProcessing, new IsEqualPP(param));
                driver.Init(null, desc);
                Assert.Fail();
            }
            catch (DirectXException)
            {
                Assert.AreEqual(null, driver.GetDevice());
            }
            mockFactory.Verify();

        }

        [Test]
        public void InitTestOK1()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();

            // Test a valid call with full screen reference
            desc.deviceType = DeviceType.Reference;
            desc.windowed = false;
            param.Windowed = false;
            param.SwapEffect = SwapEffect.Flip;
            param.BackBufferCount = 2;
            param.BackBufferWidth = desc.width;
            param.BackBufferHeight = desc.height;
            param.BackBufferFormat = desc.colorFormat;
            mockFactory.ExpectAndReturn("CreateDevice", (IDevice)mockDevice.MockInstance, 0, desc.deviceType, null, CreateFlags.SoftwareVertexProcessing, new IsEqualPP(param));
            driver.Init(null, desc);
            Assert.AreEqual((IDevice)mockDevice.MockInstance, driver.GetDevice());
            mockFactory.Verify();

            mockDevice.Expect("Dispose");
            driver.Reset();
            mockDevice.Verify();
        }

        [Test]
        public void InitTestOK2()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();

            // Test a valid call with windowed HAL
            desc.deviceType = DeviceType.Hardware;
            desc.windowed = true;
            param.Windowed = true;
            param.SwapEffect = SwapEffect.Discard;
            mockFactory.ExpectAndReturn("CreateDevice", (IDevice)mockDevice.MockInstance, 0, desc.deviceType, null, CreateFlags.HardwareVertexProcessing, new IsEqualPP(param));
            driver.Init(null, desc);
            Assert.AreEqual((IDevice)mockDevice.MockInstance, driver.GetDevice());
            mockFactory.Verify();

            mockDevice.Expect("Dispose");
            driver.Reset();
            mockDevice.Verify();
        }

        [Test]
        public void TestInitDepthStencil()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();
            desc.deviceType = DeviceType.Hardware;
            param.SwapEffect = SwapEffect.Flip;
            param.BackBufferCount = 2;
            param.BackBufferWidth = desc.width;
            param.BackBufferHeight = desc.height;
            param.BackBufferFormat = desc.colorFormat;

            desc.depthFormat = DepthFormat.Unknown;
            mockFactory.ExpectAndReturn("CreateDevice", (IDevice)mockDevice.MockInstance, 0, desc.deviceType, null, CreateFlags.HardwareVertexProcessing, new IsEqualPP(param));
            driver.Init(null, desc);
            Assert.AreEqual((IDevice)mockDevice.MockInstance, driver.GetDevice());
            mockFactory.Verify();

            param.EnableAutoDepthStencil = true;

            // 32 bit depth, no stencil
            desc.depthFormat = DepthFormat.D32;
            param.AutoDepthStencilFormat = desc.depthFormat;
            mockFactory.ExpectAndReturn("CreateDevice", (IDevice)mockDevice.MockInstance, 0, desc.deviceType, null, CreateFlags.HardwareVertexProcessing, new IsEqualPP(param));
            mockDevice.Expect("Dispose");
            driver.Init(null, desc);
            Assert.AreEqual((IDevice)mockDevice.MockInstance, driver.GetDevice());
            mockFactory.Verify();
            mockDevice.Verify();

            mockDevice.Expect("Dispose");
            driver.Reset();
            mockDevice.Verify();
        }
    }
}
