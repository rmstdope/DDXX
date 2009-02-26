using System;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface ISetupDialog
    {
        bool Reference { get; set; }
        bool Windowed { get; set; }
        bool Multisampling { get; set; }
        string SelectedResolution { get; set; }
        string[] Resolution { set; }
        bool EnableRadio4_3 { set; }
        bool EnableRadio16_9 { set; }
        bool EnableRadio16_10 { set; }
        bool CheckedRadio4_3 { set; }
        bool CheckedRadio16_9 { set; }
        bool CheckedRadio16_10 { set; }
    }
}
