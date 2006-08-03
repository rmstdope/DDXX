using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;

namespace Direct3D
{
    public class D3DFactory : IFactory
    {
        #region IFactory Members

        public IDevice CreateDevice(int adapter, DeviceType deviceType, Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters)
        {
            return new DeviceAdapter(adapter, deviceType, renderWindow, behaviorFlags, presentationParameters);
        }

        public IManager CreateManager()
        {
            return new DeviceManager();
        }

        #endregion
    }
}
