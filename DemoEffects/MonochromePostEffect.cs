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
            PostProcessor.Monochrome(PostProcessor.OutputTextureID, PostProcessor.TextureID.FULLSIZE_TEXTURE_1);
        }
    }
}
