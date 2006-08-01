using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace Direct3D
{
    public interface IFactory
    {
        IDevice CreateDevice(int adapter, DeviceType deviceType, Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters);
    }
}
