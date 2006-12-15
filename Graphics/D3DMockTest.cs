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
        protected IGraphicsFactory factory;
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
            displayMode.Width = 800;
            displayMode.Height = 600;
            displayMode.Format = Format.R8G8B8;

            presentParameters = new PresentParameters();
            presentParameters.BackBufferFormat = Format.R32F;
            presentParameters.BackBufferHeight = 200;
            presentParameters.BackBufferWidth = 400;

            mockery = new Mockery();
            factory = mockery.NewMock<IGraphicsFactory>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            effectFactory = mockery.NewMock<IEffectFactory>();
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

        //public void SetupD3DDriver()
        //{
        //    DeviceDescription desc = CreateDescription();
        //    PresentParameters param = new PresentParameters();
        //    desc.deviceType = DeviceType.Reference;
        //    desc.windowed = false;
        //    param.Windowed = false;
        //    param.SwapEffect = SwapEffect.Flip;
        //    param.BackBufferCount = 2;
        //    param.BackBufferWidth = desc.width;
        //    param.BackBufferHeight = desc.height;
        //    param.BackBufferFormat = desc.colorFormat;
        //    Expect.Once.On(prerequisits).Method("CheckPrerequisits").With(0, desc.deviceType);
        //    D3DDriver.GetInstance().Initialize(null, desc, prerequisits);
        //}

    //    public void ExpectBaseDemoEffects(int num)
    //    {
    //        effectFactory = new EffectFactory(device, factory);
    //        textureFactory = new TextureFactory(device, factory, presentParameters);
    //        D3DDriver.EffectFactory = effectFactory;
    //        D3DDriver.TextureFactory = textureFactory;
    //        effect = mockery.NewMock<IEffect>();

    //        Expect.Once.On(factory).
    //            Method("EffectFromFile").
    //            Will(Return.Value(effect));
    //        for (int i = 0; i < num; i++)
    //        {
    //            Expect.Once.On(effect).
    //                Method("GetParameter").
    //                With(null, "LightDiffuseColor").
    //                Will(Return.Value(EffectHandle.FromString("1")));
    //            Expect.Once.On(effect).
    //                Method("GetParameter").
    //                With(null, "LightSpecularColor").
    //                Will(Return.Value(EffectHandle.FromString("1")));
    //            Expect.Once.On(effect).
    //                Method("GetParameter").
    //                With(null, "LightPosition").
    //                Will(Return.Value(EffectHandle.FromString("1")));
    //            Expect.Once.On(effect).
    //                Method("GetParameter").
    //                With(null, "EyePosition").
    //                Will(Return.Value(EffectHandle.FromString("1")));
    //        }
    //    }
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
