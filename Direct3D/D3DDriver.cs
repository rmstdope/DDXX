using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Direct3D
{
    public struct DeviceDescription
    {
        public bool windowed;
        public bool useDepth;
        public bool useStencil;
        public DeviceType deviceType;
    }

    public class D3DDriver
    {
        private static D3DDriver instance = null;
        private Device device = null;

        private D3DDriver()
        {
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
            PresentParameters present = GetPresentParameters(desc);
            CreateFlags createFlags = GetCreateFlags(desc);

            device = new Device(0, desc.deviceType, control, createFlags, present);
        }

        private static CreateFlags GetCreateFlags(DeviceDescription desc)
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
                throw new Exception("Only Hardware and Reference drivers are supported.");
            }
            return createFlags;
        }

        private static PresentParameters GetPresentParameters(DeviceDescription desc)
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
            }

            return present;
        }


        public Device GetDevice()
        {
            return device;
        }
    }
}
