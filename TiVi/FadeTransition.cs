using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Utility;

namespace TiVi
{
    public class FadeTransition : BaseDemoTransition
    {
        public FadeTransition(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        public override ITexture Combine(ITexture fromTexture, ITexture toTexture)
        {
            PostProcessor.StartFrame(fromTexture);
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.BlendFactor, Blend.InvBlendFactor, GetFactor());
            PostProcessor.Process("Copy", fromTexture, toTexture);
            return toTexture;
        }

        private Color GetFactor()
        {
            float d = 1 - (Time.StepTime - StartTime) / (EndTime - StartTime);
            byte b = (byte)(d * 255);
            int i = (b << 24) + (b << 16) + (b << 8) + (b << 0);
            return Color.FromArgb(i);
        }
    }
}
