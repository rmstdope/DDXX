using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoEffects
{
    public class TextFadingEffect : BaseDemoEffect
    {
        private ISprite sprite;
        private IFont font;
        private string fontName;
        private int fontHeight;
        private Vector2 textPosition;
        private string text;
        private Color textColor;
        private float fadeInLength;
        private float fadeOutLength;

        public float FadeOutLength
        {
            get { return fadeOutLength; }
            set { fadeOutLength = value; }
        }

        public float FadeInLength
        {
            get { return fadeInLength; }
            set { fadeInLength = value; }
        }

        public Color TextColor
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Vector2 TextPosition
        {
            get { return textPosition; }
            set { textPosition = value; }
        }

        public int FontHeight
        {
            get { return fontHeight; }
            set 
            {
                if (font != null && fontHeight != value)
                {
                    fontHeight = value;
                    CreateFont();
                }
                fontHeight = value;
            }
        }

        public string FontName
        {
            get { return fontName; }
            set
            {
                if (font != null && fontName != value)
                {
                    fontName = value;
                    CreateFont();
                }
                fontName = value;
            }
        }

        public TextFadingEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
            TextPosition = new Vector2(0.5f, 0.5f);
            Text = "Default";
            FontHeight = 50;
            FontName = "Arial";
            textColor = Color.White;
        }

        protected override void Initialize()
        {
            CreateFont();
            sprite = GraphicsFactory.CreateSprite(Device);
        }

        private void CreateFont()
        {
            FontDescription description = new FontDescription();
            description.FaceName = fontName;
            description.Height = fontHeight;
            description.Quality = FontQuality.AntiAliased;

            font = GraphicsFactory.CreateFont(Device, description);
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            int alpha = GetFadeAlpha();
            Rectangle textRectangle = GetTextRectangle();
            sprite.Begin(SpriteFlags.AlphaBlend);
            font.DrawText(sprite, text, textRectangle, 
                DrawTextFormat.Center | DrawTextFormat.VerticalCenter, Color.FromArgb(alpha, textColor));
            sprite.End();
        }

        private Rectangle GetTextRectangle()
        {
            int x = (int)(textPosition.X * Device.Viewport.Width - 500);
            int y = (int)(textPosition.Y * Device.Viewport.Height - 500);
            Rectangle textRectangle = new Rectangle(x, y, 1000, 1000);
            return textRectangle;
        }

        private int GetFadeAlpha()
        {
            int alpha = 255;
            float time = Time.CurrentTime - StartTime;
            if (time < FadeInLength)
                alpha = (int)(255 * (time / FadeInLength));
            time = Time.CurrentTime - (EndTime - FadeOutLength);
            if (time > 0)
                alpha = 254 - (int)(255 * (time / FadeOutLength));
            return alpha;
        }
    }
}
