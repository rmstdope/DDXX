using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.DemoEffects
{
    public class OverlayPostEffect : BaseDemoPostEffect
    {
        private ITexture texture;

        private string filename;
        private float blendFactor = 1.0f;
        private bool addNoise = false;
        private bool subtractNoise = false;
        private float fadeInLength;
        private float fadeOutLength;

        public float FadeOutLength
        {
            get { return fadeOutLength; }
            set { fadeOutLength = value; }
        }

        public float FadeInLength
        {
            get { return fadeInLength; }
            set { fadeInLength = value; }
        }
        public bool SubtractNoise
        {
            get { return subtractNoise; }
            set { subtractNoise = value; addNoise = !value; }
        }

        public bool AddNoise
        {
            get { return addNoise; }
            set { addNoise = value; subtractNoise = !value; }
        }

        public float BlendFactor
        {
            get { return blendFactor; }
            set { blendFactor = value; }
        }

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public ITexture Texture
        {
            set { texture = value; }
        }

        public OverlayPostEffect(string name, float start, float end)
            : base(name, start, end)
        {
            SetStepSize(GetTweakableNumber("BlendFactor"), 0.01f);
            SetStepSize(GetTweakableNumber("FadeInLength"), 0.1f);
            SetStepSize(GetTweakableNumber("FadeOutLength"), 0.1f);
        }

        protected override void Initialize()
        {
            if (texture == null && filename == null)
                throw new DDXXException("OverlayPostEffect.Filename or OverlayPostEffect.Texture must be set before Initialize is called.");

            if (!addNoise && !subtractNoise)
                throw new DDXXException("Either AddNoise or SubtractNoise must be set to true for OverlayPostEffect.");

            if (addNoise && subtractNoise)
                throw new DDXXException("AddNoise and SubtractNoise cen not both be set for OverlayPostEffect.");

            if (texture == null)
                texture = TextureFactory.CreateFromFile(filename);

        }

        public override void Render()
        {
            if (addNoise)
                PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.InvSourceColor, Color.White);
            if (subtractNoise)
                PostProcessor.SetBlendParameters(BlendOperation.RevSubtract, Blend.One, Blend.One, Color.White);
            float factor = BlendFactor * GetFadeAlpha();
            PostProcessor.SetValue("Color", new float[] { factor, factor, factor, factor});

            PostProcessor.Process("Blend", texture, PostProcessor.OutputTexture);
        }

        private float GetFadeAlpha()
        {
            float alpha = 1;
            float time = Time.StepTime - StartTime;
            if (time < FadeInLength)
                alpha = time / FadeInLength;
            time = Time.StepTime - (EndTime - FadeOutLength);
            if (time > 0)
                alpha = 1 - time / FadeOutLength;
            return alpha;
        }
    }
}
