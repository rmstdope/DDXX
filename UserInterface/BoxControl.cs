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
        private Color Color;
        private Color ShadowColor;
        private Color OutlineColor;
        private Texture2D Texture;

        public BoxControl(Vector4 rectangle, byte alpha, Color color, BaseControl parent)
            : base(rectangle, parent)
        {
            Color = new Color((color.R * alpha) / 255, (color.G * alpha) / 255, (color.B * alpha) / 255, alpha);
            ShadowColor = new Color((byte)0, (byte)0, (byte)0, alpha);
            OutlineColor = new Color(alpha, alpha, alpha, alpha);
        }

        public BoxControl(Vector4 rectangle, byte alpha, Texture2D texture, BaseControl parent)
            : base(rectangle, parent)
        {
            Texture = texture;
            Color = new Color(alpha, alpha, alpha, alpha);
            ShadowColor = new Color((byte)0, (byte)0, (byte)0, alpha);
            OutlineColor = new Color(alpha, alpha, alpha, alpha);
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
            if (Color.A == 255)
                resources.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            else
                resources.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            if (Texture != null)
                resources.SpriteBatch.Draw(Texture, new Rectangle(x1, y1, width, height), Color);
            else
                resources.SpriteBatch.Draw(resources.WhiteTexture, new Rectangle(x1, y1, width, height), Color);
            resources.SpriteBatch.End();

            resources.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
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
