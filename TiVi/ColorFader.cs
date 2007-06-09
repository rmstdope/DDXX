using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX;

namespace TiVi
{
    public class ColorFader : BaseDemoPostEffect
    {
        private float fadeInLength;
        private float fadeOutLength;
        private Color fadeColor;

        public float FadeInLength
        {
            set { fadeInLength = value; }
            get { return fadeInLength; }
        }

        public float FadeOutLength
        {
            set { fadeOutLength = value; }
            get { return fadeOutLength; }
        }

        public Color FadeColor
        {
            set { fadeColor = value; }
            get { return fadeColor; }
        }

        public ColorFader(float startTime, float endTime)
            : base(startTime, endTime)
        {
            fadeInLength = 0.5f;
            fadeOutLength = 2.0f;
            fadeColor = Color.Black;
        }

        protected override void Initialize()
        {
        }

        public override void Render()
        {
            float fillLength = EndTime - StartTime - fadeInLength - fadeOutLength;
            float endTimeIn = StartTime + fadeInLength;
            float startTimeOut = endTimeIn + fillLength;
            float endTimeOut = startTimeOut + fadeOutLength;
            float flashAlpha;
            if (Time.StepTime > startTimeOut)
                flashAlpha = 1.0f - (Time.StepTime - startTimeOut) / fadeOutLength;
            else if (Time.StepTime < endTimeIn)
                flashAlpha = (Time.CurrentTime - StartTime) / fadeInLength;
            else
                flashAlpha = 1;
            flashAlpha = (float)(Math.Max(0, Math.Min(1, flashAlpha)));
            PostProcessor.SetValue("Color", new Vector4(fadeColor.R / 255.0f, fadeColor.G / 255.0f, fadeColor.B / 255.0f, flashAlpha));
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.SourceAlpha, Blend.InvSourceAlpha, Color.Empty);
            PostProcessor.Process("Color", PostProcessor.OutputTextureID, PostProcessor.OutputTextureID);
        }
    }
}
