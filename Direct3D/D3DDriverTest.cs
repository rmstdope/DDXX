using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Utility;
using NUnit.Framework;
using NMock2;

namespace Direct3D
{

    public class IsEqualPP : Matcher
    {
        private PresentParameters param1;

        public IsEqualPP(PresentParameters param)
        {
            param1 = param;
        }

        public override bool Matches(object val)
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

        public override void DescribeTo(TextWriter writer)
        {
            writer.Write(param1.ToString());
        }
    }

    [TestFixture]
    public class D3DDriverTest
    {
        private D3DDriver driver;
        private Mockery mockery;
        private IFactory factory;
        private IDevice device;

        [SetUp]
        public void Setup()
        {
            mockery = new Mockery();
            factory = mockery.NewMock<IFactory>();
            device = mockery.NewMock<IDevice>();
            D3DDriver.SetFactory(factory);
            driver = D3DDriver.GetInstance();
        }

        [TearDown]
        public void Teardown()
        {
            driver.Reset();
            driver = null;
            mockery.VerifyAllExpectationsHaveBeenMet();
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
                Expect.Once.On(factory).
                    Method("CreateDevice").
                    With(Is.EqualTo(0), Is.EqualTo(desc.deviceType), Is.EqualTo(null), Is.EqualTo(CreateFlags.SoftwareVertexProcessing), new IsEqualPP(param)).
                    Will(Throw.Exception(new DirectXException()));
                driver.Init(null, desc);
                Assert.Fail();
            }
            catch (DirectXException)
            {
                Assert.AreEqual(null, driver.GetDevice());
            }
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
            Expect.Once.On(factory).
                Method("CreateDevice").
                With(Is.EqualTo(0), Is.EqualTo(desc.deviceType), Is.EqualTo(null), Is.EqualTo(CreateFlags.SoftwareVertexProcessing), new IsEqualPP(param)).
                Will(Return.Value(device));
            driver.Init(null, desc);
            Assert.AreEqual(device, driver.GetDevice());

            Expect.Once.On(device).
                Method("Dispose");
            driver.Reset();
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
            Expect.Once.On(factory).
                Method("CreateDevice").
                With(Is.EqualTo(0), Is.EqualTo(desc.deviceType), Is.EqualTo(null), Is.EqualTo(CreateFlags.HardwareVertexProcessing), new IsEqualPP(param)).
                Will(Return.Value(device));
            //mockFactory.ExpectAndReturn("CreateDevice", (IDevice)mockDevice.MockInstance, 0, desc.deviceType, null, CreateFlags.HardwareVertexProcessing, new IsEqualPP(param));
            driver.Init(null, desc);
            Assert.AreEqual(device, driver.GetDevice());

            Expect.Once.On(device).
                Method("Dispose");
            driver.Reset();
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
            Expect.Once.On(factory).
                Method("CreateDevice").
                With(Is.EqualTo(0), Is.EqualTo(desc.deviceType), Is.EqualTo(null), Is.EqualTo(CreateFlags.HardwareVertexProcessing), new IsEqualPP(param)).
                Will(Return.Value(device));
            //mockFactory.ExpectAndReturn("CreateDevice", (IDevice)mockDevice.MockInstance, 0, desc.deviceType, null, CreateFlags.HardwareVertexProcessing, new IsEqualPP(param));
            driver.Init(null, desc);
            Assert.AreEqual(device, driver.GetDevice());

            param.EnableAutoDepthStencil = true;

            // 32 bit depth, no stencil
            desc.depthFormat = DepthFormat.D32;
            param.AutoDepthStencilFormat = desc.depthFormat;
            Expect.Once.On(factory).
                Method("CreateDevice").
                With(Is.EqualTo(0), Is.EqualTo(desc.deviceType), Is.EqualTo(null), Is.EqualTo(CreateFlags.HardwareVertexProcessing), new IsEqualPP(param)).
                Will(Return.Value(device));
            Expect.Once.On(device).
                Method("Dispose");
            //mockFactory.ExpectAndReturn("CreateDevice", (IDevice)mockDevice.MockInstance, 0, desc.deviceType, null, CreateFlags.HardwareVertexProcessing, new IsEqualPP(param));
            //mockDevice.Expect("Dispose");
            driver.Init(null, desc);
            Assert.AreEqual(device, driver.GetDevice());

            Expect.Once.On(device).
                Method("Dispose");
            driver.Reset();
        }
    }
}
