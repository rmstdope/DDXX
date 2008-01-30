using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoEffects;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoEffects
{
    public class AspectilizerPostEffect : OverlayPostEffect
    {
        private float width = 16.0f;
        private float height = 10.0f;
        private float rounding = 0.02f;

        public float Width
        {
            get { return width; }
            set { width = value; CreateTexture();  }
        }

        public float Height
        {
            get { return height; }
            set { height = value; CreateTexture(); }
        }

        [TweakStep(0.001f)]
        public float Rounding
        {
            get { return rounding; }
            set { rounding = value; CreateTexture(); }
        }

        public AspectilizerPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            AddNoise = true;
        }

        private void CreateTexture()
        {
            if (GraphicsDevice != null)
            {
                float ratio = (height / width) *
                    (GraphicsDevice.PresentationParameters.BackBufferWidth / (float)GraphicsDevice.PresentationParameters.BackBufferHeight);
                ITextureGenerator rect = new RoundedRectangle(new Vector2(0.95f, ratio * 0.95f), new Vector2(0.5f, 0.5f), rounding);
                Texture = TextureFactory.CreateFromGenerator(GraphicsDevice.PresentationParameters.BackBufferWidth,
                    GraphicsDevice.PresentationParameters.BackBufferHeight, 1, TextureUsage.None, SurfaceFormat.Color, rect);
            }
        }

        protected override void Initialize()
        {
            CreateTexture();
            base.Initialize();
        }
    }
}
