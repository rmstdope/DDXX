using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using Utility;

namespace Direct3D
{
    class DeviceAdapter : IDevice
    {
        Device device;

        public DeviceAdapter(int adapter, DeviceType deviceType, Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters)
        {
            try
            {   
                device = new Device(adapter, deviceType, renderWindow, behaviorFlags, presentationParameters);
            }
            catch (InvalidCallException exception)
            {
                throw new DDXXException(exception.Message);
            }
        }

        public void Dispose()
        {
            device.Dispose();
        }
        public void Clear(ClearFlags flags, Color color, float zdepth, int stencil)
        {
            device.Clear(flags, color, zdepth, stencil);
        }
        public void Present()
        {
            device.Present();
        }

    }
}
