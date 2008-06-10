using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.UserInterface
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

        public override void Draw(IDrawResources resources)
        {
            if (Color.A == 0)
                return;

            int screenWidth = resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
            int x1 = (int)(screenWidth * GetX1(resources));
            int y1 = (int)(screenHeight * GetY1(resources));
            int width = (int)(screenWidth * GetWidth(resources));
            int height = (int)(screenHeight * GetHeight(resources));
            resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            if (Texture != null)
                resources.SpriteBatch.Draw(Texture, new Rectangle(x1, y1, width, height), new Color(255, 255, 255, 255));
            else
                resources.SpriteBatch.Draw(resources.WhiteTexture, new Rectangle(x1, y1, width, height), Color);
            resources.SpriteBatch.End();

            resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            DrawHorizontalLine(resources.SpriteBatch, resources.WhiteTexture, x1 + 1, y1 + 1, width, ShadowColor);
            DrawHorizontalLine(resources.SpriteBatch, resources.WhiteTexture, x1 + 1, y1 + 1 + height, width, ShadowColor);
            DrawVerticalLine(resources.SpriteBatch, resources.WhiteTexture, x1 + 1, y1 + 1, height, ShadowColor);
            DrawVerticalLine(resources.SpriteBatch, resources.WhiteTexture, x1 + 1 + width, y1 + 1, height, ShadowColor);

            DrawHorizontalLine(resources.SpriteBatch, resources.WhiteTexture, x1, y1, width, OutlineColor);
            DrawHorizontalLine(resources.SpriteBatch, resources.WhiteTexture, x1, y1 + height, width, OutlineColor);
            DrawVerticalLine(resources.SpriteBatch, resources.WhiteTexture, x1, y1, height, OutlineColor);
            DrawVerticalLine(resources.SpriteBatch, resources.WhiteTexture, x1 + width, y1, height, OutlineColor);
            resources.SpriteBatch.End();
        }

    }
}
