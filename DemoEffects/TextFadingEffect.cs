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
        private float fontHeight;
        private Vector3 textPosition;
        private string text;
        private Color textColor;
        private float fadeInLength;
        private float fadeOutLength;
        private Vector3 velocity;

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

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

        public Vector3 TextPosition
        {
            get { return textPosition; }
            set { textPosition = value; }
        }

        public float FontHeight
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

        public TextFadingEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            TextPosition = new Vector3(0.5f, 0.5f, 0);
            Text = "Default";
            FontHeight = 0.1f;
            FontName = "Arial";
            textColor = Color.White;
            SetStepSize(GetTweakableNumber("TextPosition"), 0.01f);
            SetStepSize(GetTweakableNumber("FontHeight"), 0.01f);
            SetStepSize(GetTweakableNumber("FadeInLength"), 0.1f);
            SetStepSize(GetTweakableNumber("FadeOutLength"), 0.1f);
            SetStepSize(GetTweakableNumber("Velocity"), 0.01f);
        }

        protected override void Initialize()
        {
            CreateFont();
            sprite = GraphicsFactory.CreateSprite(Device);
        }

        private void CreateFont()
        {
            FontDescription description = new FontDescription();
            description.OutputPrecision = Precision.Raster;
            description.FaceName = fontName;
            description.Height = (int)(fontHeight * Device.Viewport.Height);
            description.Quality = FontQuality.AntiAliased | FontQuality.ClearType;
            description.Weight = FontWeight.UltraLight;

            font = GraphicsFactory.CreateFont(Device, description);
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            if (Time.StepTime > EndTime || Time.StepTime < StartTime)
                return;
            int alpha = GetFadeAlpha();
            Rectangle textRectangle = GetTextRectangle();
            sprite.Begin(SpriteFlags.AlphaBlend);
            font.DrawText(sprite, text, textRectangle,
                DrawTextFormat.Center | DrawTextFormat.VerticalCenter, Color.FromArgb(alpha, textColor));
            sprite.End();
        }

        private Rectangle GetTextRectangle()
        {
            Vector3 pos = textPosition + velocity * (Time.StepTime - StartTime);
            int x = (int)(pos.X * Device.Viewport.Width - 5000);
            int y = (int)(pos.Y * Device.Viewport.Height - 5000);
            Rectangle textRectangle = new Rectangle(x, y, 10000, 10000);
            return textRectangle;
        }

        private int GetFadeAlpha()
        {
            int alpha = 255;
            float time = Time.StepTime - StartTime;
            if (time < FadeInLength)
                alpha = (int)(255 * (time / FadeInLength));
            time = Time.StepTime - (EndTime - FadeOutLength);
            if (time > 0)
                alpha = 254 - (int)(255 * (time / FadeOutLength));
            return alpha;
        }
    }
}
