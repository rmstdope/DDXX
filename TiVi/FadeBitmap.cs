using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace TiVi
{
    public class FadeBitmap : BaseDemoEffect
    {
        private ISprite sprite;
        private ITexture texture;
        private Vector3 textPosition;
        private Color textColor;
        private float fadeInLength;
        private float fadeOutLength;
        private string filename;
        private float textureWidth;
        private float textureHeight;

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

        public Color Color
        {
            get { return textColor; }
            set { textColor = value; }
        }

        public Vector3 Position
        {
            get { return textPosition; }
            set { textPosition = value; }
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public FadeBitmap(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            Position = new Vector3(0.5f, 0.5f, 0);
            Color = Color.White;
        }

        protected override void Initialize()
        {
            sprite = GraphicsFactory.CreateSprite(Device);
            texture = TextureFactory.CreateFromFile(filename);
            using (ISurface surface = texture.GetSurfaceLevel(0))
            {
                textureWidth = surface.Description.Width;
                textureHeight = surface.Description.Height;
            }
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            int alpha = GetFadeAlpha();
            //Rectangle textRectangle = GetTextRectangle();
            sprite.Begin(SpriteFlags.AlphaBlend);
            sprite.Draw2D(texture, Rectangle.Empty, SizeF.Empty, 
                new PointF(Position.X * Device.Viewport.Width - textureWidth / 2, 
                           Position.Y * Device.Viewport.Height - textureHeight / 2), 
                Color.FromArgb(alpha, Color));
            sprite.End();
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
