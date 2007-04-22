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
    public class RingsPostEffect : BaseDemoPostEffect
    {
        private float scale = 1.0f;
        private float distance = 1.0f;

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }

        public RingsPostEffect(float startTime, float endTime)
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
            PostProcessor.SetValue("RingsDistance", distance);
            PostProcessor.SetValue("RingsScale", scale);
            float t = Time.CurrentTime;
            float time1 = (float)(t - Math.Truncate(t));
            t *= 1.75847f;
            float time2 = (float)(t - Math.Truncate(t));
            PostProcessor.SetValue("WaveTime", new Vector2(time1, time2));
            PostProcessor.Process("Rings", startTexture, endTexture);
        }
    }
}
