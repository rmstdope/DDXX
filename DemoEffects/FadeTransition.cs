using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoEffects
{
    public class FadeTransition : BaseDemoTransition
    {
        public FadeTransition(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        public override RenderTarget2D Combine(RenderTarget2D fromTexture, RenderTarget2D toTexture)
        {
            PostProcessor.StartFrame(fromTexture);
            BlendState blend = new BlendState();
            blend.ColorBlendFunction = blend.AlphaBlendFunction = BlendFunction.Add;
            blend.ColorSourceBlend = blend.AlphaSourceBlend = Blend.BlendFactor;
            blend.ColorDestinationBlend = blend.AlphaDestinationBlend = Blend.InverseBlendFactor;
            blend.BlendFactor = GetFactor();
            PostProcessor.BlendState = blend;
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
