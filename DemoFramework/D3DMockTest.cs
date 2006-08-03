using System;
using System.Collections.Generic;
using System.Text;
using Direct3D;
using Microsoft.DirectX.Direct3D;
using NMock2;

namespace DemoFramework
{
    public class D3DMockTest
    {
        protected Mockery mockery;
        protected IFactory factory;
        protected IDevice device;
        protected IManager manager;
        protected DisplayMode displayMode = new DisplayMode();

        public virtual void SetUp()
        {
            displayMode.Width = 800;
            displayMode.Height = 600;
            displayMode.Format = Format.R8G8B8;

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
            Stub.On(manager).
                Method("CurrentDisplayMode").
                With(0).
                Will(Return.Value(displayMode));
            Stub.On(device).
                Method("Dispose");

            D3DDriver.SetFactory(factory);
        }

        public virtual void TearDown()
        {
            D3DDriver.DestroyInstance();
            mockery.VerifyAllExpectationsHaveBeenMet();
        }
    }
}
