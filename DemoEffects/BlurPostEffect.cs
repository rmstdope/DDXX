using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoEffects
{
    public class BlurPostEffect : BaseDemoPostEffect
    {
        private int numPasses = 1;

        public int NumPasses
        {
            get { return numPasses; }
            set { numPasses = value; }
        }

        public BlurPostEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override void Render()
        {
            List<ITexture> textures = PostProcessor.GetTemporaryTextures(2, false);

            PostProcessor.SetValue("BloomScale", 1.0f);
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.Process("HorizontalBloom", PostProcessor.OutputTexture, textures[0]);
            PostProcessor.Process("VerticalBloom", textures[0], textures[1]);
            for (int i = 0; i < numPasses - 1; i++)
            {
                PostProcessor.Process("HorizontalBloom", textures[1], textures[0]);
                PostProcessor.Process("VerticalBloom", textures[0], textures[1]);
            }
        }

        protected override void Initialize()
        {
        }
    }
}
