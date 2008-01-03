using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoEffects
{
    public class RingsPostEffect : SinglePassPostEffect
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

        public RingsPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void SetParameters()
        {
            PostProcessor.SetBlendParameters(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
            PostProcessor.SetValue("RingsDistance", distance);
            PostProcessor.SetValue("RingsScale", scale);
            float t = Time.CurrentTime;
            float time1 = (float)(t - (int)t);
            t *= 1.75847f;
            float time2 = (float)(t - (int)t);
            PostProcessor.SetValue("WaveTime", new Vector2(time1, time2));
        }

        protected override string  TechniqueName
        {
        	get { return "Rings"; }
        }

        protected override void Initialize()
        {
        }
    }
}
