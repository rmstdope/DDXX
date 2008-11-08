using System;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.UserInterface
{
    public interface IWindowControl : IControl
    {
        byte Alpha { get; set; }
        void Draw(IDrawResources resources);
        Color SelectedTextColor { get; set; }
        string SubTitle { get; set; }
        Color TextColor { get; set; }
        ITexture2D Texture { get; set; }
        string Title { get; set; }
    }
}
