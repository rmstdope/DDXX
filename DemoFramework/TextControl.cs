using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public class TextControl : BaseControl
    {
        private string text;
        private DrawTextFormat format;
        private float alpha;
        private Color color;
        private int shadowOffset = 1;

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

        public TextControl(string text, RectangleF rectangle, DrawTextFormat format, float alpha, Color color, BaseControl parent)
            : base(rectangle, parent)
        {
            this.text = text;
            this.format = format;
            this.alpha = alpha;
            this.color = color;
        }

        public TextControl(string text, PointF point, DrawTextFormat format, float alpha, Color color, BaseControl parent)
            : base(new RectangleF(), parent)
        {
            this.text = text;
            this.format = format;
            this.alpha = alpha;
            this.color = color;
            float x = point.X;
            float y = point.Y;

            float height = 100.0f;
            float width = 100.0f;
            if (((int)format & (int)DrawTextFormat.Bottom) != 0)
                y -= height;
            if (((int)format & (int)DrawTextFormat.VerticalCenter) != 0)
                y -= height / 2;
            if (((int)format & (int)DrawTextFormat.Right) != 0)
                x -= width;
            if (((int)format & (int)DrawTextFormat.Center) != 0)
                x -= width / 2;
            rectangle = new RectangleF(x, y, width, height);
        }

        internal override void Draw(ISprite sprite, ILine line, IFont font, ITexture whiteTexture)
        {
            Rectangle rect1 = new Rectangle((int)(GetX1()), (int)(GetY1()), (int)(GetWidth()), (int)(GetHeight()));
            Rectangle rect2 = new Rectangle((int)(GetX1()) + ShadowOffset, (int)(GetY1()) + ShadowOffset, (int)(GetWidth()), (int)(GetHeight()));
            font.DrawText(null, text, rect2, format, Color.FromArgb((int)(255 * alpha), Color.Black));
            font.DrawText(null, text, rect1, format, Color.FromArgb((int)(255 * alpha), color));
        }
    }
}
