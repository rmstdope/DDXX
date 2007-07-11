using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoEffects
{
    public class DepthOfFieldPostEffect : BaseDemoPostEffect
    {
        public DepthOfFieldPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        public override void Render()
        {
            List<ITexture> textures = PostProcessor.GetTemporaryTextures(1, false);

            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.Process("DepthOfField", PostProcessor.OutputTexture, textures[0]);
        }

        protected override void Initialize()
        {
        }
    }
}
