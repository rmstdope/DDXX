using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoEffects
{
    public class DepthOfFieldPostEffect : BaseDemoPostEffect
    {
        private int numPasses;

        public int NumPasses
        {
            get { return numPasses; }
            set { numPasses = value; }
        }


        public DepthOfFieldPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        public override void Render()
        {
            int current = 0;
            int numTextures = 1;
            if (numPasses > 1)
                numTextures = 2;
            List<ITexture> textures = PostProcessor.GetTemporaryTextures(numTextures, false);

            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.Process("DepthOfField", PostProcessor.OutputTexture, textures[0]);
            for (int i = 0; i < numPasses - 1; i++)
            {
                PostProcessor.Process("DepthOfField", textures[current], textures[1 - current]);
                current = 1 - current;
            }
        }

        protected override void Initialize()
        {
        }
    }
}
