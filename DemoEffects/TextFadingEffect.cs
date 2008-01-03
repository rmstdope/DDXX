using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    public class TextFadingEffect : BaseDemoEffect
    {
        private ISpriteBatch spriteBatch;
        private ISpriteFont font;
        private string fontName;
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

        public string FontName
        {
            get { return fontName; }
            set { fontName = value; }
        }

        public TextFadingEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            TextPosition = new Vector3(0.5f, 0.5f, 0);
            Text = "Default";
            FontName = "NoFont";
            textColor = Color.White;
            SetStepSize(GetTweakableNumber("TextPosition"), 0.01f);
            SetStepSize(GetTweakableNumber("FadeInLength"), 0.1f);
            SetStepSize(GetTweakableNumber("FadeOutLength"), 0.1f);
            SetStepSize(GetTweakableNumber("Velocity"), 0.01f);
        }

        protected override void Initialize()
        {
            font = GraphicsFactory.SpriteFontFromFile(FontName);
            spriteBatch = GraphicsFactory.CreateSpriteBatch();
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            if (Time.CurrentTime > EndTime || Time.CurrentTime < StartTime)
                return;
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, GetTextPosition(), 
                new Color(textColor.R, textColor.G, textColor.B, GetFadeAlpha()));
            spriteBatch.End();
        }

        private Vector2 GetTextPosition()
        {
            float time = Time.CurrentTime - StartTime;
            Vector2 pos = new Vector2(textPosition.X * GraphicsDevice.PresentationParameters.BackBufferWidth,
                                      textPosition.Y * GraphicsDevice.PresentationParameters.BackBufferHeight);
            pos += new Vector2(velocity.X * GraphicsDevice.PresentationParameters.BackBufferWidth * time,
                               velocity.Y * GraphicsDevice.PresentationParameters.BackBufferHeight * time);
            pos -= font.MeasureString(text) / 2;
            return pos;
        }

        private byte GetFadeAlpha()
        {
            byte alpha = 255;
            float time = Time.CurrentTime - StartTime;
            if (time < FadeInLength)
                alpha = (byte)(255 * (time / FadeInLength));
            time = Time.CurrentTime - (EndTime - FadeOutLength);
            if (time > 0)
                alpha = (byte)(254 - (255 * (time / FadeOutLength)));
            return alpha;
        }
    }
}
