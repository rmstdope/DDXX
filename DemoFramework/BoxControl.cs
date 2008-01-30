using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public class BoxControl : BaseControl
    {
        public Color Color;
        public Color ShadowColor;
        public Color OutlineColor;
        public ITexture2D Texture;

        public BoxControl(Vector4 rectangle, byte alpha, Color color, BaseControl parent)
            : base(rectangle, parent)
        {
            Color = new Color(color.R, color.G, color.B, alpha);
            ShadowColor = new Color(0, 0, 0, alpha);
            OutlineColor = new Color(255, 255, 255, alpha);
        }

        public BoxControl(Vector4 rectangle, byte alpha, ITexture2D texture, BaseControl parent)
            : base(rectangle, parent)
        {
            Texture = texture;
            Color = new Color(255, 255, 255, alpha);
            ShadowColor = new Color(0, 0, 0, alpha);
            OutlineColor = new Color(255, 255, 255, alpha);
        }

        public override void Draw(ISpriteBatch spriteBatch, ISpriteFont spriteFont, ITexture2D whiteTexture)
        {
            if (Color.A == 0)
                return;

            int screenWidth = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
            int x1 = (int)(screenWidth * GetX1());
            int y1 = (int)(screenHeight * GetY1());
            int width = (int)(screenWidth * GetWidth());
            int height = (int)(screenHeight * GetHeight());
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            if (Texture != null)
                spriteBatch.Draw(Texture, new Rectangle(x1, y1, width, height), new Color(255, 255, 255, 255));
            else
                spriteBatch.Draw(whiteTexture, new Rectangle(x1, y1, width, height), Color);
            spriteBatch.End();

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            DrawHorizontalLine(spriteBatch, whiteTexture, x1 + 1, y1 + 1, width, ShadowColor);
            DrawHorizontalLine(spriteBatch, whiteTexture, x1 + 1, y1 + 1 + height, width, ShadowColor);
            DrawVerticalLine(spriteBatch, whiteTexture, x1 + 1, y1 + 1, height, ShadowColor);
            DrawVerticalLine(spriteBatch, whiteTexture, x1 + 1 + width, y1 + 1, height, ShadowColor);

            DrawHorizontalLine(spriteBatch, whiteTexture, x1, y1, width, OutlineColor);
            DrawHorizontalLine(spriteBatch, whiteTexture, x1, y1 + height, width, OutlineColor);
            DrawVerticalLine(spriteBatch, whiteTexture, x1, y1, height, OutlineColor);
            DrawVerticalLine(spriteBatch, whiteTexture, x1 + width, y1, height, OutlineColor);
            spriteBatch.End();
        }

    }
}
