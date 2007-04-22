using System;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    public enum Transparency
    {
        High,
        Medium,
        Low
    }

    public enum ColorSchema
    {
        Blue,
        Gray,
        Original
    }

    public interface ITweakerSettings
    {
        float Alpha { get; }
        Color SelectedColor { get; set; }
        void NextColorSchema();
        void PreviousColorSchema();
        void SetTransparency(Transparency transparency);
        float TextAlpha { get; }
        Color TimeColor { get; set; }
        Color TitleColor { get; set; }
        Color UnselectedColor { get; set; }
    }
}
