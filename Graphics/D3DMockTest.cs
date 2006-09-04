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
        protected DisplayMode displayMode = new DisplayMode();

        public virtual void SetUp()
        {
            displayMode.Width = 800;
            displayMode.Height = 600;
            displayMode.Format = Format.R8G8B8;

            mockery = new Mockery();
            factory = mockery.NewMock<IGraphicsFactory>();
            device = mockery.NewMock<IDevice>();
            manager = mockery.NewMock<IManager>();
            texture = mockery.NewMock<ITexture>();
            surface = mockery.NewMock<ISurface>();

            Stub.On(factory).
                Method("CreateManager").
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
                Will(Return.Value(new PresentParameters()));

            D3DDriver.Factory = factory;
        }

        public virtual void TearDown()
        {
            D3DDriver.DestroyInstance();
            mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
