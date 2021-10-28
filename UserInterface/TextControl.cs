using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.UserInterface
{
    public class TextControl : BaseControl
    {
        private string text;
        private byte alpha;
        private Color color;
        private int shadowOffset = 2;
        private Positioning format;

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

        public Positioning Format
        {
            get { return format; }
            set { format = value; }
        }

        public TextControl(string text, Vector4 rectangle, Positioning format, byte alpha, Color color, BaseControl parent)
            : base(rectangle, parent)
        {
            this.text = text;
            this.alpha = alpha;
            this.color = color;
            this.format = format;
        }

        public TextControl(string text, Vector2 point, Positioning format, byte alpha, Color color, BaseControl parent)
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
            if (((int)format & (int)Positioning.Bottom) != 0)
                y -= height;
            if (((int)format & (int)Positioning.VerticalCenter) != 0)
                y -= height / 2;
            if (((int)format & (int)Positioning.Right) != 0)
                x -= width;
            if (((int)format & (int)Positioning.Center) != 0)
                x -= width / 2;
            rectangle = new Vector4(x, y, width, height);
        }

        public Vector2 BoundingBox(IDrawResources resources)
        {
            Vector2 bb = resources.GetSpriteFont(FontSize.Medium).MeasureString(text);
            bb.X /= resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            bb.Y /= resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
            return bb;
        }

        public override void Draw(IDrawResources resources)
        {
            Vector2 size = resources.GetSpriteFont(FontSize.Medium).MeasureString(text);
            Vector2 pos = new Vector2(GetX1(resources), GetY1(resources));
            if (((int)format & (int)Positioning.Bottom) != 0)
                pos.Y += GetHeight(resources);
            if (((int)format & (int)Positioning.VerticalCenter) != 0)
                pos.Y += GetHeight(resources) / 2;
            if (((int)format & (int)Positioning.Right) != 0)
                pos.X += GetWidth(resources);
            if (((int)format & (int)Positioning.Center) != 0)
                pos.X += GetWidth(resources) / 2;

            pos.X *= resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            pos.Y *= resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;

            if (((int)format & (int)Positioning.Bottom) != 0)
                pos.Y -= size.Y;
            if (((int)format & (int)Positioning.VerticalCenter) != 0)
                pos.Y -= size.Y / 2;
            if (((int)format & (int)Positioning.Right) != 0)
                pos.X -= size.X;
            if (((int)format & (int)Positioning.Center) != 0)
                pos.X -= size.X / 2;

            Color col1 = new Color(color.R, color.G, color.B, alpha);
            Color col2 = new Color((byte)0, (byte)0, (byte)0, alpha);
            resources.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            resources.SpriteBatch.DrawString(resources.GetSpriteFont(FontSize.Medium), text, pos + new Vector2(ShadowOffset, ShadowOffset), col2);
            resources.SpriteBatch.DrawString(resources.GetSpriteFont(FontSize.Medium), text, pos, col1);
            resources.SpriteBatch.End();
        }
    }
}
