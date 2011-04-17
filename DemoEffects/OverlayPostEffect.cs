using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoEffects
{
    public class OverlayPostEffect : BaseDemoPostEffect
    {
        private Texture2D texture;
        private float blendFactor;
        private bool addNoise;
        private bool subtractNoise;
        private float fadeInLength;
        private float fadeOutLength;
        private string technique;
        private readonly BlendState addBlend;
        private readonly BlendState subBlend;

        public string Technique
        {
            get { return technique; }
            set { technique = value; }
        }

        [TweakStep(0.1f)]
        public float FadeOutLength
        {
            get { return fadeOutLength; }
            set { fadeOutLength = value; }
        }

        [TweakStep(0.1f)]
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

        [TweakStep(0.01f)]
        public float BlendFactor
        {
            get { return blendFactor; }
            set { blendFactor = value; }
        }

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public OverlayPostEffect(string name, float start, float end)
            : base(name, start, end)
        {
            blendFactor = 1.0f;
            addNoise = false;
            subtractNoise = true;
            fadeInLength = 0.0f;
            fadeOutLength = 0.0f;
            technique = "Blend";

            addBlend = new BlendState();
            addBlend.ColorBlendFunction = addBlend.AlphaBlendFunction = BlendFunction.Add;
            addBlend.ColorSourceBlend = addBlend.AlphaSourceBlend = Blend.One;
            addBlend.ColorDestinationBlend = addBlend.AlphaDestinationBlend = Blend.InverseSourceColor;
            subBlend = new BlendState();
            subBlend.ColorBlendFunction = subBlend.AlphaBlendFunction = BlendFunction.ReverseSubtract;
            subBlend.ColorSourceBlend = subBlend.AlphaSourceBlend = Blend.One;
            subBlend.ColorDestinationBlend = subBlend.AlphaDestinationBlend = Blend.One;
        }

        protected override void Initialize()
        {
            if (texture == null)
                throw new DDXXException("OverlayPostEffect.Texture must be set before Initialize is called.");

            if (!addNoise && !subtractNoise)
                throw new DDXXException("Either AddNoise or SubtractNoise must be set to true for OverlayPostEffect.");

            if (addNoise && subtractNoise)
                throw new DDXXException("AddNoise and SubtractNoise cen not both be set for OverlayPostEffect.");
        }

        public override void Render()
        {
            if (addNoise)
                PostProcessor.BlendState = addBlend;
            if (subtractNoise)
                PostProcessor.BlendState = subBlend;
            float factor = BlendFactor * GetFadeAlpha();
            PostProcessor.SetValue("Color", new float[] { factor, factor, factor, factor});

            PostProcessor.Process(technique, texture, PostProcessor.OutputTexture);
        }

        private float GetFadeAlpha()
        {
            float alpha = 1;
            float time = Time.CurrentTime - StartTime;
            if (time < FadeInLength)
                alpha = time / FadeInLength;
            time = Time.CurrentTime - (EndTime - FadeOutLength);
            if (time > 0)
                alpha = 1 - time / FadeOutLength;
            return alpha;
        }
    }
}
