using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoEffects;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.TextureBuilder;
using Microsoft.DirectX;

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

        public float Rounding
        {
            get { return rounding; }
            set { rounding = value; CreateTexture(); }
        }

        public AspectilizerPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            AddNoise = true;
            SetStepSize(GetTweakableNumber("Rounding"), 0.001f);
        }

        private void CreateTexture()
        {
            if (Device != null)
            {
                Viewport viewport = Device.Viewport;
                float ratio = (height / width) * (viewport.Width / (float)viewport.Height);
                IGenerator rect = new RoundedRectangle(new Vector2(0.95f, ratio * 0.95f), new Vector2(0.5f, 0.5f), rounding);
                Texture = TextureBuilder.Generate(rect, viewport.Width, viewport.Height, 1, Format.A8R8G8B8);
            }
        }

        protected override void Initialize()
        {
            CreateTexture();
            base.Initialize();
        }
    }
}
