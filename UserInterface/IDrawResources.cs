using System;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.UserInterface
{
    public interface IDrawResources
    {
        SpriteBatch SpriteBatch { get; }
        SpriteFont GetSpriteFont(FontSize size);
        Texture2D WhiteTexture { get; }
        float AspectRatio { get; }
    }
}
