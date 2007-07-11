using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoEffects
{
    public class RadialBlurPostEffect : BaseDemoPostEffect
    {
        private Color blurColor = Color.White;

        public Color BlurColor
        {
            get { return blurColor; }
            set { blurColor = value; }
        }

        public RadialBlurPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        public override void Render()
        {
            List<ITexture> textures = PostProcessor.GetTemporaryTextures(2, true);
            ITexture startTexture = PostProcessor.OutputTexture;
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.SetValue("ZoomFactor", 0.20f);
            PostProcessor.Process("ZoomAdd", startTexture, textures[0]);
            float invZoomFactor = 0.8f;
            int source = 0;
            for (int i = 0; i < 6; i++)
            {
                invZoomFactor /= 2;
                PostProcessor.SetValue("ZoomFactor", 1 - invZoomFactor);
                PostProcessor.Process("ZoomAdd", textures[source], textures[1 - source]);
                source = 1 - source;
            }
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.BlendFactor, Blend.One, blurColor);
            PostProcessor.Process("Copy", textures[source], startTexture);
        }

        protected override void Initialize()
        {
        }
    }
}
