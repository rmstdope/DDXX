using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NMock2;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public class D3DMockTest
    {
        protected Mockery mockery;
        protected IGraphicsFactory graphicsFactory;
        protected IDevice device;
        protected ITexture texture;
        protected ISurface surface;
        protected IManager manager;
        protected IRenderStateManager renderStateManager;
        protected IPrerequisits prerequisits;
        protected DisplayMode displayMode = new DisplayMode();
        protected PresentParameters presentParameters;
        protected IEffectFactory effectFactory;
        protected ITextureFactory textureFactory;
        protected IEffect effect;

        public virtual void SetUp()
        {
            Caps caps;

            displayMode.Width = 800;
            displayMode.Height = 600;
            displayMode.Format = Format.R8G8B8;

            presentParameters = new PresentParameters();
            presentParameters.BackBufferFormat = Format.R32F;
            presentParameters.BackBufferHeight = 200;
            presentParameters.BackBufferWidth = 400;

            mockery = new Mockery();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            effectFactory = mockery.NewMock<IEffectFactory>();
            device = mockery.NewMock<IDevice>();
            manager = mockery.NewMock<IManager>();
            texture = mockery.NewMock<ITexture>();
            surface = mockery.NewMock<ISurface>();
            renderStateManager = mockery.NewMock<IRenderStateManager>();
            prerequisits = mockery.NewMock<IPrerequisits>();
            effect = mockery.NewMock<IEffect>();

            Stub.On(graphicsFactory).
                GetProperty("Manager").
                Will(Return.Value(manager));
            Stub.On(graphicsFactory).
                Method("CreateDevice").
                WithAnyArguments().
                Will(Return.Value(device));
            Stub.On(manager).
                Method("GetDeviceCaps").With(0, DeviceType.Hardware).
                Will(Return.Value(caps));
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
            Stub.On(surface).
                Method("Dispose");

            D3DDriver.GraphicsFactory = graphicsFactory;
            D3DDriver.TextureFactory = textureFactory;
            D3DDriver.EffectFactory = effectFactory;
            D3DDriver.GetInstance().Device = device;
        }

        public virtual void TearDown()
        {
            D3DDriver.DestroyInstance();
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        private DeviceDescription CreateDescription()
        {
            DeviceDescription desc = new DeviceDescription();
            desc.width = 400;
            desc.height = 200;
            desc.colorFormat = Format.R32F;
            return desc;
        }

        public void SetupTime()
        {
            Time.Initialize();
        }
    }

    public class FloatMatcher : Matcher
    {
        public float value;
        public float epsilon;

        public FloatMatcher(float value, float epsilon)
        {
            this.value = value;
            this.epsilon = epsilon;
        }

        public FloatMatcher(float value)
        {
            this.value = value;
            this.epsilon = 0.00001f;
        }

        public override void DescribeTo(System.IO.TextWriter writer)
        {
            writer.WriteLine("Matching to " + value + " with epsilon " + epsilon);
        }

        public override bool Matches(object o)
        {
            if (!(o is float))
                return false;
            float f = (float)o;

            if (f > value + epsilon || f < value - epsilon)
                return false;
            return true;
        }
    }
}