using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    public abstract class SinglePassPostEffect : BaseDemoPostEffect
    {
        public SinglePassPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected abstract void SetParameters();
        protected abstract string TechniqueName { get; }

        public override void Render()
        {
            List<RenderTarget2D> textures = PostProcessor.GetTemporaryTextures(1, false);

            SetParameters();
            PostProcessor.Process(TechniqueName, PostProcessor.OutputTexture, textures[0]);
        }
    }
}
