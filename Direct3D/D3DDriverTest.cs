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
        private IManager manager;

        private DisplayMode displayMode = new DisplayMode();
        private DisplayMode[] supportedDisplayModes = { new DisplayMode(), new DisplayMode() };

        [SetUp]
        public void Setup()
        {
            displayMode.Width = 800;
            displayMode.Height = 600;
            displayMode.Format = Format.R8G8B8G8;

            supportedDisplayModes[0].Width = 640;
            supportedDisplayModes[0].Height = 480;
            supportedDisplayModes[0].Format = Format.R8G8B8;
            supportedDisplayModes[1].Width = 800;
            supportedDisplayModes[1].Height = 600;
            supportedDisplayModes[1].Format = Format.A16B16G16R16F;

            mockery = new Mockery();
            factory = mockery.NewMock<IFactory>();
            device = mockery.NewMock<IDevice>();
            manager = mockery.NewMock<IManager>();

            Stub.On(factory).
                Method("CreateManager").
                Will(Return.Value(manager));

            Expect.Once.On(manager).
                Method("CurrentDisplayMode").
                With(0).
                Will(Return.Value(displayMode));

            D3DDriver.SetFactory(factory);
            driver = D3DDriver.GetInstance();

        }

        [TearDown]
        public void Teardown()
        {
            D3DDriver.DestroyInstance();
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestCreateFail()
        {
            // No factory set
            try
            {
                D3DDriver.DestroyInstance();
                D3DDriver.SetFactory(null);
                driver = D3DDriver.GetInstance();
                Assert.Fail();
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestCreateOK()
        {
            D3DDriver driver2 = D3DDriver.GetInstance();
            Assert.AreSame(driver, driver2);
        }

        [Test]
        public void TestDisplayModes1()
        {
            Expect.Once.On(manager).
                Method("SupportedDisplayModes").
                With(0).
                Will(Return.Value(supportedDisplayModes));

            DisplayMode[] modes = driver.GetDisplayModes(0, delegate(DisplayMode mode) { return true; });
            Assert.AreEqual(2, modes.Length);
            Assert.AreEqual(640, modes[0].Width);
            Assert.AreEqual(480, modes[0].Height);
            Assert.AreEqual(Format.R8G8B8, modes[0].Format);
            Assert.AreEqual(800, modes[1].Width);
            Assert.AreEqual(600, modes[1].Height);
            Assert.AreEqual(Format.A16B16G16R16F, modes[1].Format);
        }

        [Test]
        public void TestDisplayModes2()
        {
            Expect.Once.On(manager).
                Method("SupportedDisplayModes").
                With(0).
                Will(Return.Value(supportedDisplayModes));

            DisplayMode[] modes = driver.GetDisplayModes(0, delegate(DisplayMode mode) { return mode.Format == Format.R8G8B8; });
            Assert.AreEqual(1, modes.Length);
            Assert.AreEqual(640, modes[0].Width);
            Assert.AreEqual(480, modes[0].Height);
            Assert.AreEqual(Format.R8G8B8, modes[0].Format);
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
            driver.Init(null, desc);
            Assert.AreEqual(device, driver.GetDevice());

            Expect.Once.On(device).
                Method("Dispose");
            driver.Reset();
        }

        [Test]
        public void TestInitDepthFail1()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();
            desc.deviceType = DeviceType.Reference;
            desc.useStencil = true;

            // Can not use only stencil
            try
            {
                driver.Init(null, desc);
                Assert.Fail();
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestInitDepthFail2()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();
            desc.deviceType = DeviceType.Reference;
            desc.useDepth = true;

            Expect.Exactly(3).On(manager).
                Method("CheckDepthStencilMatch").
                WithAnyArguments().
                Will(Return.Value(false));
            // No depth buffer available
            try
            {
                driver.Init(null, desc);
                Assert.Fail();
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestInitDepthFail3()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();
            desc.deviceType = DeviceType.Reference;
            desc.useDepth = true;
            desc.useStencil = true;

            Expect.Exactly(3).On(manager).
                Method("CheckDepthStencilMatch").
                WithAnyArguments().
                Will(Return.Value(false));

            // No depth/stencil buffer available
            try
            {
                driver.Init(null, desc);
                Assert.Fail();
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestInitDepthOK1()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();
            desc.deviceType = DeviceType.Reference;
            desc.windowed = true;
            param.Windowed = true;
            desc.useDepth = true;
            param.SwapEffect = SwapEffect.Discard;
            param.AutoDepthStencilFormat = DepthFormat.D16;
            param.EnableAutoDepthStencil = true;

            // Create device with depth only
            Expect.Exactly(2).On(manager).
                Method("CheckDepthStencilMatch").
                WithAnyArguments().
                Will(Return.Value(false));
            Expect.Once.On(manager).
                Method("CheckDepthStencilMatch").
                WithAnyArguments().
                Will(Return.Value(true));
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
        public void TestInitDepthOK2()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();
            desc.deviceType = DeviceType.Reference;
            desc.windowed = true;
            param.Windowed = true;
            desc.useDepth = true;
            desc.useStencil= true;
            param.SwapEffect = SwapEffect.Discard;
            param.AutoDepthStencilFormat = DepthFormat.D24X4S4;
            param.EnableAutoDepthStencil = true;

            // Create device with depth and stencil
            Expect.Exactly(1).On(manager).
                Method("CheckDepthStencilMatch").
                WithAnyArguments().
                Will(Return.Value(false));
            Expect.Once.On(manager).
                Method("CheckDepthStencilMatch").
                WithAnyArguments().
                Will(Return.Value(true));
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

    }
}
