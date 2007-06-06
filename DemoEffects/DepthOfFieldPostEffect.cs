using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.DemoEffects
{
    public class DepthOfFieldPostEffect : BaseDemoPostEffect
    {
        public DepthOfFieldPostEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override void Render()
        {
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            //if (PostProcessor.OutputTextureID != TextureID.FULLSIZE_TEXTURE_1)
            //    PostProcessor.Process("DepthOfField", PostProcessor.OutputTextureID, TextureID.FULLSIZE_TEXTURE_1);
            //else
            //    PostProcessor.Process("DepthOfField", PostProcessor.OutputTextureID, TextureID.FULLSIZE_TEXTURE_2);
            if (PostProcessor.OutputTextureID != TextureID.FULLSIZE_TEXTURE_1)
            {
                PostProcessor.Process("DepthOfField", PostProcessor.OutputTextureID, TextureID.FULLSIZE_TEXTURE_1);
                //PostProcessor.Process("DepthOfField", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
                //PostProcessor.Process("DepthOfField", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
                //PostProcessor.Process("DepthOfField", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
            }
            else
            {
                PostProcessor.Process("DepthOfField", PostProcessor.OutputTextureID, TextureID.FULLSIZE_TEXTURE_2);
                //PostProcessor.Process("DepthOfField", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
                //PostProcessor.Process("DepthOfField", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
                //PostProcessor.Process("DepthOfField", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
            }
        }

        protected override void Initialize()
        {
        }
    }
}
