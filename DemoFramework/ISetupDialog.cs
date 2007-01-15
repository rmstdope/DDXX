using System;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoFramework
{
    public interface ISetupDialog
    {
        //Format ColorFormat { get; }
        DeviceDescription DeviceDescription { get; }
        bool OK { get; }
        bool REF { get; }
        bool Windowed { get; }
        bool Bit16 { get; }
        bool Bit32 { get; }
        //bool Bit16FP { get; }
        //bool Bit32FP { get; }
        string SelectedResolution { get; }
    }
}
