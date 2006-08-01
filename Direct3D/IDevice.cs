using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Direct3D
{
    public interface IDevice
    {
        void Dispose();
        void Clear(ClearFlags flags, Color color, float zdepth, int stencil);
        void Present();
    }
}
