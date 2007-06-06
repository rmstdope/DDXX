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
    public class WavePostEffect : SinglePassPostEffect
    {
        private Vector4 strength = new Vector4(30.0f, 20.0f, 25.0f, 27.0f);
        private float scale = 1.0f;

        public float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        public Vector4 Strength
        {
            get { return strength; }
            set { strength = value; }
        }

        public WavePostEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        protected override void SetParameters()
        {
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.SetValue("WaveStrength", strength);
            PostProcessor.SetValue("WaveScale", scale);
            float t = Time.CurrentTime;
            float time1 = (float)(t - Math.Truncate(t));
            t *= 1.75847f;
            float time2 = (float)(t - Math.Truncate(t));
            PostProcessor.SetValue("WaveTime", new Vector2(time1, time2));
        }

        protected override string TechniqueName
        {
            get { return "Wave"; }
        }

        protected override void Initialize()
        {
        }
    }
}
