using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.UserInterface
{
    public enum TextFormatting
    {
        Center = 0x01,
        VerticalCenter = 0x02,
        Right = 0x04,
        Bottom = 0x08,
        Left = 0x10,
        Top = 0x20
    }

    public class TextControl : BaseControl
    {
        private string text;
        private byte alpha;
        private Color color;
        private int shadowOffset = 2;
        private TextFormatting format;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        public byte Alpha
        {
            get { return alpha; }
            set { alpha = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public int ShadowOffset
        {
            get { return shadowOffset; }
            set { shadowOffset = value; }
        }

        public TextFormatting Format
        {
            get { return format; }
            set { format = value; }
        }

        public TextControl(string text, Vector4 rectangle, TextFormatting format, byte alpha, Color color, BaseControl parent)
            : base(rectangle, parent)
        {
            this.text = text;
            this.alpha = alpha;
            this.color = color;
            this.format = format;
        }

        public TextControl(string text, Vector2 point, TextFormatting format, byte alpha, Color color, BaseControl parent)
            : base(new Vector4(), parent)
        {
            this.text = text;
            this.alpha = alpha;
            this.color = color;
            this.format = format;
            float x = point.X;
            float y = point.Y;

            float height = 1;
            float width = 1;
            if (((int)format & (int)TextFormatting.Bottom) != 0)
                y -= height;
            if (((int)format & (int)TextFormatting.VerticalCenter) != 0)
                y -= height / 2;
            if (((int)format & (int)TextFormatting.Right) != 0)
                x -= width;
            if (((int)format & (int)TextFormatting.Center) != 0)
                x -= width / 2;
            rectangle = new Vector4(x, y, width, height);
        }

        public override void Draw(ISpriteBatch spriteBatch, ISpriteFont spriteFont, ITexture2D whiteTexture)
        {
            Vector2 size = spriteFont.MeasureString(text);
            Vector2 pos = new Vector2(GetX1(), GetY1());
            if (((int)format & (int)TextFormatting.Bottom) != 0)
                pos.Y += GetHeight();
            if (((int)format & (int)TextFormatting.VerticalCenter) != 0)
                pos.Y += GetHeight() / 2;
            if (((int)format & (int)TextFormatting.Right) != 0)
                pos.X += GetWidth();
            if (((int)format & (int)TextFormatting.Center) != 0)
                pos.X += GetWidth() / 2;

            pos.X *= spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            pos.Y *= spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;

            if (((int)format & (int)TextFormatting.Bottom) != 0)
                pos.Y -= size.Y;
            if (((int)format & (int)TextFormatting.VerticalCenter) != 0)
                pos.Y -= size.Y / 2;
            if (((int)format & (int)TextFormatting.Right) != 0)
                pos.X -= size.X;
            if (((int)format & (int)TextFormatting.Center) != 0)
                pos.X -= size.X / 2;

            Color col1 = new Color(color.R, color.G, color.B, alpha);
            Color col2 = new Color(0, 0, 0, alpha);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            spriteBatch.DrawString(spriteFont, text, pos + new Vector2(ShadowOffset, ShadowOffset), col2);
            spriteBatch.DrawString(spriteFont, text, pos, col1);
            spriteBatch.End();
        }
    }
}
