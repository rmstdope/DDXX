using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;

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
            PostProcessor.Process("DownSample4x", PostProcessor.OutputTextureID, temp[0]);
            PostProcessor.Process("DownSample4x", temp[0], temp[1]);
        }
    }
}
