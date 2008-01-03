using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    public class MonochromePostEffect : BaseDemoPostEffect
    {
        public MonochromePostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        public override void Render()
        {
            List<IRenderTarget2D> textures = PostProcessor.GetTemporaryTextures(1, false);

            PostProcessor.SetBlendParameters(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.Process("Monochrome", PostProcessor.OutputTexture, textures[0]);
        }

        protected override void Initialize()
        {
        }
    }
}
