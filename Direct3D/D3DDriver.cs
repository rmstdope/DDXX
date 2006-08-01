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
    public struct DeviceDescription
    {
        public int width;
        public int height;
        public Format colorFormat;
        public bool windowed;
        public DepthFormat depthFormat;
        public DeviceType deviceType;
    }

    public class D3DDriver
    {
        private static D3DDriver instance;
        private IDevice device;
        private DisplayMode displayMode;
        private static IFactory factory = new D3DFactory();

        private D3DDriver()
        {
            GetDisplayMode();
        }

        public static void SetFactory(IFactory d3dFactory)
        {
            factory = d3dFactory;
        }

        public static D3DDriver GetInstance()
        {
            if (instance == null)
            {
                instance = new D3DDriver();
            }
            return instance;
        }

        public void Init(Control control, DeviceDescription desc)
        {
            if (device != null)
                Reset();

            PresentParameters present = GetPresentParameters(desc);
            CreateFlags createFlags = GetCreateFlags(desc);

            device = factory.CreateDevice(0, desc.deviceType, control, createFlags, present);
        }

        private void GetDisplayMode()
        {
            AdapterInformation ai = Manager.Adapters[0];
            displayMode = ai.CurrentDisplayMode;
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
            if (desc.depthFormat != DepthFormat.Unknown)
            {
                present.EnableAutoDepthStencil = true;
                present.AutoDepthStencilFormat = desc.depthFormat;
            }
            else
            {
                present.EnableAutoDepthStencil = false;
            }
        }


        public IDevice GetDevice()
        {
            return device;
        }

        public int NumAdapters
        {
            get
            {
                return Manager.Adapters.Count;
            }
        }

        public delegate bool ValidMode(DisplayMode mode);
        private IEnumerable<DisplayMode> EnumerateDisplayModes(int adapter, ValidMode validMode)
        {
            foreach (DisplayMode mode in Manager.Adapters[adapter].SupportedDisplayModes)
            {
                if (validMode(mode))
                {
                    yield return mode;
                }
            }
        }
        public DisplayMode[] GetDisplayModes(int adapter, ValidMode validMode)
        {
            IEnumerable<DisplayMode> modes = EnumerateDisplayModes(adapter, validMode);
            int num = 0;
            foreach (DisplayMode m in modes)
            {
                num++;
            }
            DisplayMode[] ret = new DisplayMode[num];
            num = 0;
            foreach (DisplayMode m in modes)
            {
                ret[num] = m;
                num++;
            }

            return ret;
        }
    }
}
