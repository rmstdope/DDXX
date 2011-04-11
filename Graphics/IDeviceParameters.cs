using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IDeviceParameters
    {
        GraphicsProfile GraphicsProfile { get; }
        bool ReferenceDevice { get; }
        SurfaceFormat BackBufferFormat { get; }
        int BackBufferWidth { get; }
        int BackBufferHeight { get; }
        bool FullScreen { get; }
        int MultiSampleCount { get; }
        DepthFormat DepthStencilFormat { get; }
        SurfaceFormat RenderTargetFormat { get; }
    }
}
