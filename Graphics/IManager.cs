using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Graphics
{
    public interface IManager
    {
        DisplayMode CurrentDisplayMode(int adapter);
        int NumAdapters();
        DisplayMode[] SupportedDisplayModes(int adapter);
        bool CheckDepthStencilMatch(int adapter, DeviceType deviceType, Format adapterFormat, Format renderTargetFormat, DepthFormat depthStencilFormat);
    }
}
