using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.UserInterface
{
    public enum FontSize
    {
        Small,
        Medium,
        Large
    }
    public interface IUserInterface
    {
        void Initialize(IGraphicsFactory graphicsFactory, TextureFactory textureFactory);
        void DrawControl(IControl control);
        void SetFont(FontSize size, SpriteFont font);
        IDrawResources DrawResources { get; }
    }
}
