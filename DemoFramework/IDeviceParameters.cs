using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDeviceParameters
    {
        DeviceType DeviceType { get; }
        SurfaceFormat BackBufferFormat { get; }
        int BackBufferWidth { get; }
        int BackBufferHeight { get; }
        bool FullScreen { get; }
        MultiSampleType MultiSampleType { get; }
        DepthFormat DepthStencilFormat { get; }
        SurfaceFormat RenderTargetFormat { get; }
    }
}
