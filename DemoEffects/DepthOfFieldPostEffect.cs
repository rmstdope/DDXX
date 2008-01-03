using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

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
            //numTextures = 2;
            List<IRenderTarget2D> textures = PostProcessor.GetTemporaryTextures(numTextures, false);

            PostProcessor.SetBlendParameters(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
            //PostProcessor.Process("DepthOfFieldVertical", PostProcessor.OutputTexture, textures[0]);
            //PostProcessor.Process("DepthOfFieldHorizontal", textures[0], textures[1]);
            //PostProcessor.Process("DepthOfFieldVertical", textures[1], textures[0]);
            //PostProcessor.Process("DepthOfFieldHorizontal", textures[0], textures[1]);
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
