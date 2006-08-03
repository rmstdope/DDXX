using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Direct3D
{
    public class DeviceManager : IManager
    {
        #region IManager Members

        public DisplayMode CurrentDisplayMode(int adapter)
        {
            return Manager.Adapters[adapter].CurrentDisplayMode;
        }

        public int NumAdapters()
        {
            return Manager.Adapters.Count;
        }

        public DisplayMode[] SupportedDisplayModes(int adapter)
        {
            DisplayModeCollection collection = Manager.Adapters[adapter].SupportedDisplayModes;
            DisplayMode[] modes = new DisplayMode[collection.Count];
            int i = 0;
            foreach (DisplayMode mode in collection)
            {
                modes[i++] = mode;
            }
            return modes;
        }

        public bool CheckDepthStencilMatch(int adapter, DeviceType deviceType, Format adapterFormat, Format renderTargetFormat, DepthFormat depthStencilFormat)
        {
            return Manager.CheckDepthStencilMatch(adapter, deviceType, adapterFormat, renderTargetFormat, depthStencilFormat);
        }

        #endregion
    }
}
