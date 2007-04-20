using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoEffects
{
    public class PerturbationPostEffect : BaseDemoPostEffect
    {
        private Vector4 strength = new Vector4(30.0f, 20.0f, 25.0f, 27.0f);

        public Vector4 Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        public PerturbationPostEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override void Render()
        {
            TextureID startTexture = PostProcessor.OutputTextureID;
            TextureID endTexture = TextureID.FULLSIZE_TEXTURE_1;
            if (startTexture == endTexture)
                endTexture = TextureID.FULLSIZE_TEXTURE_2;

            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.SetValue("PerturbationStrength", strength);
            float t = Time.CurrentTime;
            float time1 = (float)(t - Math.Truncate(t));
            t *= 1.75847f;
            float time2 = (float)(t - Math.Truncate(t));
            PostProcessor.SetValue("PerturbationTime", new Vector2(time1, time2));
            PostProcessor.Process("Perturbate", startTexture, endTexture);
        }
    }
}
