using System;
using System.Collections.Generic;
using System.Text;

using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.DemoEffects
{
    public class GlowPostEffect : BaseDemoPostEffect
    {
        public GlowPostEffect(float startTime, float endTime)
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
                temp[1] = TextureID.FULLSIZE_TEXTURE_3;
            }
            else if (PostProcessor.OutputTextureID == TextureID.FULLSIZE_TEXTURE_2)
            {
                temp[0] = TextureID.FULLSIZE_TEXTURE_1;
                temp[1] = TextureID.FULLSIZE_TEXTURE_3;
            }
            else
            {
                temp[0] = TextureID.FULLSIZE_TEXTURE_1;
                temp[1] = TextureID.FULLSIZE_TEXTURE_2;
            }
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.Process("DownSample4x", PostProcessor.OutputTextureID, temp[0]);
            PostProcessor.Process("DownSample4x", temp[0], temp[1]);
            PostProcessor.Process("Brighten", temp[1], temp[0]);
            PostProcessor.Process("HorizontalBloom", temp[0], temp[1]);
            PostProcessor.Process("VerticalBloom", temp[1], temp[0]);
            PostProcessor.Process("UpSample4x", temp[0], temp[1]);
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.One, Color.Black);
            PostProcessor.Process("UpSample4x", temp[1], startTexture);
            //PostProcessor.Process("Copy", temp[0], startTexture);
        }
    }
}
