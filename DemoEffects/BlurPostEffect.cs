using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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

        public BlurPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        public override void Render()
        {
            List<RenderTarget2D> textures = PostProcessor.GetTemporaryTextures(2, false);

            PostProcessor.BlendState = BlendState.Opaque;
            PostProcessor.SetValue("BloomScale", 1.0f);
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
