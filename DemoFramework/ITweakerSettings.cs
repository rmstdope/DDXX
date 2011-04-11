using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

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
        byte Alpha { get; }
        Color SelectedColor { get; set; }
        void NextColorSchema();
        void PreviousColorSchema();
        void SetTransparency(Transparency transparency);
        byte TextAlpha { get; }
        Color TimeColor { get; set; }
        Color TitleColor { get; set; }
        Color UnselectedColor { get; set; }
        Keys ScreenshotKey { get; set; }
        Keys RegenerateKey { get; set; }
    }
}
