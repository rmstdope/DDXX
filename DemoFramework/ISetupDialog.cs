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
        bool EnableRadio4_3 { set; get; }
        bool EnableRadio16_9 { set; get; }
        bool EnableRadio16_10 { set; get; }
        bool Checked16Bit { set; get; }
        bool Checked32Bit { set; get; }
        bool CheckedRadio3_4 { set; }
        bool CheckedRadio16_9 { set; }
        bool CheckedRadio16_10 { set; }
    }
}
