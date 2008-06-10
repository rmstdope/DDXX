using System;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.UserInterface
{
    public interface IDrawResources
    {
        ISpriteBatch SpriteBatch { get; }
        ISpriteFont GetSpriteFont(FontSize size);
        ITexture2D WhiteTexture { get; }
        float AspectRatio { get; }
    }
}
