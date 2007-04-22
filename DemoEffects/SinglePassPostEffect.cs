using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoEffects
{
    public abstract class SinglePassPostEffect : BaseDemoPostEffect
    {
        public SinglePassPostEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        protected abstract void SetParameters();
        protected abstract string TechniqueName { get; }

        public override void Render()
        {
            TextureID startTexture = PostProcessor.OutputTextureID;
            TextureID endTexture = TextureID.FULLSIZE_TEXTURE_1;
            if (startTexture == endTexture)
                endTexture = TextureID.FULLSIZE_TEXTURE_2;

            SetParameters();
            PostProcessor.Process(TechniqueName, startTexture, endTexture);
        }
    }
}
