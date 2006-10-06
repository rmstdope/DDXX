using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NMock2;

namespace Dope.DDXX.Graphics
{
    public class D3DMockTest
    {
        protected Mockery mockery;
        protected IGraphicsFactory factory;
        protected IDevice device;
        protected ITexture texture;
        protected ISurface surface;
        protected IManager manager;
        protected IRenderStateManager renderStateManager;
        protected IPrerequisits prerequisits;
        protected DisplayMode displayMode = new DisplayMode();
        protected PresentParameters presentParameters;

        public virtual void SetUp()
        {
            displayMode.Width = 800;
            displayMode.Height = 600;
            displayMode.Format = Format.R8G8B8;

            presentParameters = new PresentParameters();
            presentParameters.BackBufferFormat = Format.R32F;
            presentParameters.BackBufferHeight = 200;
            presentParameters.BackBufferWidth = 400;

            mockery = new Mockery();
            factory = mockery.NewMock<IGraphicsFactory>();
            device = mockery.NewMock<IDevice>();
            manager = mockery.NewMock<IManager>();
            texture = mockery.NewMock<ITexture>();
            surface = mockery.NewMock<ISurface>();
            renderStateManager = mockery.NewMock<IRenderStateManager>();
            prerequisits = mockery.NewMock<IPrerequisits>();

            Stub.On(factory).
                GetProperty("Manager").
                Will(Return.Value(manager));
            Stub.On(factory).
                Method("CreateDevice").
                WithAnyArguments().
                Will(Return.Value(device));
            Stub.On(factory).
                Method("CreateTexture").
                With(device, displayMode.Width, displayMode.Height, 1, Usage.RenderTarget, displayMode.Format, Pool.Default).
                Will(Return.Value(texture));
            Stub.On(manager).
                Method("CurrentDisplayMode").
                With(0).
                Will(Return.Value(displayMode));
            Stub.On(device).
                Method("Dispose");
            Stub.On(texture).
                Method("GetSurfaceLevel").
                With(0).
                Will(Return.Value(surface));
            Stub.On(surface).
                GetProperty("Description").
                Will(Return.Value(new SurfaceDescription()));
            Stub.On(device).
                GetProperty("PresentationParameters").
                Will(Return.Value(presentParameters));
            Stub.On(device).
                GetProperty("RenderState").
                Will(Return.Value(renderStateManager));

            D3DDriver.Factory = factory;
        }

        public virtual void TearDown()
        {
            D3DDriver.DestroyInstance();
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        private DeviceDescription CreateDescription()
        {
            DeviceDescription desc = new DeviceDescription();
            desc.width = 800;
            desc.height = 600;
            desc.colorFormat = Format.X8R8G8B8;
            return desc;
        }

        public void SetupD3DDriver()
        {
            DeviceDescription desc = CreateDescription();
            PresentParameters param = new PresentParameters();
            desc.deviceType = DeviceType.Reference;
            desc.windowed = false;
            param.Windowed = false;
            param.SwapEffect = SwapEffect.Flip;
            param.BackBufferCount = 2;
            param.BackBufferWidth = desc.width;
            param.BackBufferHeight = desc.height;
            param.BackBufferFormat = desc.colorFormat;
            Expect.Once.On(prerequisits).Method("CheckPrerequisits").With(0, desc.deviceType);
            D3DDriver.GetInstance().Initialize(null, desc, prerequisits);
        }
    }
}
