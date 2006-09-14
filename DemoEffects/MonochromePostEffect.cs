using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoEffects
{
    public class MonochromePostEffect : BaseDemoPostEffect
    {
        public MonochromePostEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override void Render()
        {
            if (PostProcessor.OutputTextureID != TextureID.FULLSIZE_TEXTURE_1)
                PostProcessor.Process("Monochrome", PostProcessor.OutputTextureID, TextureID.FULLSIZE_TEXTURE_1);
            else
                PostProcessor.Process("Monochrome", PostProcessor.OutputTextureID, TextureID.FULLSIZE_TEXTURE_2);
        }
    }
}
