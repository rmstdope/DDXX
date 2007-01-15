using System;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoFramework
{
    public interface ISetupDialog
    {
        bool REF { get; }
        bool Windowed { get; }
        string SelectedResolution { get; }
        string[] Resolution { set; }
        bool Enable16Bit { set; get; }
        bool Enable32Bit { set; get; }
        bool EnableRadio4_3 { set; }
        bool EnableRadio16_9 { set; }
        bool EnableRadio16_10 { set; }
        bool Checked16Bit { set; get; }
        bool Checked32Bit { set; get; }
        bool CheckedRadio3_4 { set; }
    }
}
