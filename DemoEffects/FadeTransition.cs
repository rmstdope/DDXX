using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoEffects
{
    public class FadeTransition : BaseDemoTransition
    {
        public FadeTransition(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        public override IRenderTarget2D Combine(IRenderTarget2D fromTexture, IRenderTarget2D toTexture)
        {
            PostProcessor.StartFrame(fromTexture);
            PostProcessor.SetBlendParameters(BlendFunction.Add, Blend.BlendFactor, Blend.InverseBlendFactor, GetFactor());
            PostProcessor.Process("Copy", fromTexture, toTexture);
            return toTexture;
        }

        private Color GetFactor()
        {
            float d = 1 - (Time.CurrentTime - StartTime) / (EndTime - StartTime);
            byte b = (byte)(d * 255);
            return new Color(b, b, b, b);
        }
    }
}