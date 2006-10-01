using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    public struct DeviceDescription
    {
        public int width;
        public int height;
        public Format colorFormat;
        public bool windowed;
        public bool useDepth;
        public bool useStencil;
        public DeviceType deviceType;
    }

    public class D3DDriver
    {
        private static D3DDriver instance;
        private static IGraphicsFactory factory = new D3DFactory();
        private static EffectFactory effectFactory;
        private static ModelFactory meshFactory;
        private static TextureFactory textureFactory;

        private IManager manager;
        private IDevice device;

        private DisplayMode displayMode;

        private D3DDriver()
        {
            if (factory == null)
            {
                throw new DDXXException("The DX factory needs to be set before the D3DDriver can be created.");
            }

            manager = factory.Manager;
            GetDisplayMode();
        }

        public static IGraphicsFactory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        public static EffectFactory EffectFactory
        {
            get { return effectFactory; }
            set { effectFactory = value; }
        }

        public static ModelFactory ModelFactory
        {
            get { return meshFactory; }
            set { meshFactory = value; }
        }

        public static TextureFactory TextureFactory
        {
            get { return textureFactory; }
            set { textureFactory = value; }
        }

        public static D3DDriver GetInstance()
        {
            if (instance == null)
            {
                instance = new D3DDriver();
            }
            return instance;
        }

        public void Initialize(Control control, DeviceDescription desc, IPrerequisits prerequisits)
        {
            if (device != null)
                Reset();

            PresentParameters present = GetPresentParameters(desc);
            CreateFlags createFlags = GetCreateFlags(desc);

            prerequisits.CheckPrerequisits(0, desc.deviceType);

            device = factory.CreateDevice(0, desc.deviceType, control, createFlags, present);

            effectFactory = new EffectFactory(device, factory);
            textureFactory = new TextureFactory(device, factory, present);
            meshFactory = new ModelFactory(device, factory, textureFactory);
        }

        private void GetDisplayMode()
        {
            displayMode = manager.CurrentDisplayMode(0);
        }

        public void Reset()
        {
            if (device != null)
                device.Dispose();
            device = null;
        }

        private CreateFlags GetCreateFlags(DeviceDescription desc)
        {
            CreateFlags createFlags;
            if (desc.deviceType == DeviceType.Reference)
            {
                createFlags = CreateFlags.SoftwareVertexProcessing;
            }
            else if (desc.deviceType == DeviceType.Hardware)
            {
                createFlags = CreateFlags.HardwareVertexProcessing;
            }
            else
            {
                throw new DDXXException("Only Hardware and Reference drivers are supported.");
            }
            return createFlags;
        }
    
        private PresentParameters GetPresentParameters(DeviceDescription desc)
        {
            PresentParameters present = new PresentParameters();
            if (desc.windowed)
            {
                present.Windowed = true;
                present.BackBufferCount = 1;
                present.SwapEffect = SwapEffect.Discard;
            }
            else
            {
                present.Windowed = false;
                present.SwapEffect = SwapEffect.Flip;
                present.BackBufferCount = 2;
                present.BackBufferWidth = desc.width;
                present.BackBufferHeight = desc.height;
                present.BackBufferFormat = desc.colorFormat;
            }

            SetDepthStencil(desc, present);

            return present;
        }

        private void SetDepthStencil(DeviceDescription desc, PresentParameters present)
        {
            DepthFormat[] depthFormats = { DepthFormat.D32, DepthFormat.D24X8, DepthFormat.D16 };
            DepthFormat[] stencilFormats = { DepthFormat.D24S8, DepthFormat.D24X4S4, DepthFormat.D15S1 };
            DepthFormat[] formats = depthFormats;
            
            if (desc.useDepth)
            {
                if (desc.useStencil)
                    formats = stencilFormats;

                present.EnableAutoDepthStencil = true;
                present.AutoDepthStencilFormat = DepthFormat.Unknown;

                foreach (DepthFormat format in formats)
                {
                    if (manager.CheckDeviceFormat(0, desc.deviceType, desc.colorFormat, Usage.DepthStencil, ResourceType.Surface, format) &&
                        manager.CheckDepthStencilMatch(0, desc.deviceType,
                        displayMode.Format, displayMode.Format, format))
                    {
                        present.AutoDepthStencilFormat = format;
                        break;
                    }
                }
            }
            else if (desc.useStencil)
            {
                throw new DDXXException("Can not initialize device with stencil but no depth buffer.");
            }
            else
            {
                present.EnableAutoDepthStencil = false;
            }

            if (present.EnableAutoDepthStencil && present.AutoDepthStencilFormat == DepthFormat.Unknown)
                throw new DDXXException("Could not find a depth/stencil buffer to use.");
        }


        public IDevice GetDevice()
        {
            return device;
        }

        public int NumAdapters
        {
            get
            {
                return manager.NumAdapters();
            }
        }

        public delegate bool ValidMode(DisplayMode mode);
        public DisplayMode[] GetDisplayModes(int adapter, ValidMode validMode)
        {
            DisplayMode[] modes = manager.SupportedDisplayModes(adapter);
            int num = 0;
            foreach (DisplayMode m in modes)
            {
                if (validMode(m))
                    num++;
            }
            DisplayMode[] ret = new DisplayMode[num];
            num = 0;
            foreach (DisplayMode m in modes)
            {
                if (validMode(m))
                {
                    ret[num] = m;
                    num++;
                }
            }

            return ret;
        }

        public static void DestroyInstance()
        {
            if (instance != null)
            {
                instance.Reset();
            }
            instance = null;
        }
    }
}
