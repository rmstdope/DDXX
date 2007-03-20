using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

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
            TextureID[] temp = new TextureID[2];
            TextureID startTexture = PostProcessor.OutputTextureID;
            if (PostProcessor.OutputTextureID == TextureID.FULLSIZE_TEXTURE_1)
            {
                temp[0] = TextureID.FULLSIZE_TEXTURE_2;
                temp[1] = TextureID.FULLSIZE_TEXTURE_1;
            }
            else
            {
                temp[0] = TextureID.FULLSIZE_TEXTURE_1;
                temp[1] = TextureID.FULLSIZE_TEXTURE_2;
            }
            PostProcessor.SetValue("BloomScale", 1.0f);
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.Process("HorizontalBloom", PostProcessor.OutputTextureID, temp[0]);
            PostProcessor.Process("VerticalBloom", temp[0], temp[1]);
            for (int i = 0; i < numPasses - 1; i++)
            {
                PostProcessor.Process("HorizontalBloom", temp[1], temp[0]);
                PostProcessor.Process("VerticalBloom", temp[0], temp[1]);
            }
        }
    }
}
