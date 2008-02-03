using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    public class OverlayPostEffect : BaseDemoPostEffect
    {
        private ITexture2D texture;

        private string filename;
        private float blendFactor = 1.0f;
        private bool addNoise = false;
        private bool subtractNoise = false;
        private float fadeInLength;
        private float fadeOutLength;
        private string technique;

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

        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }

        public ITexture2D Texture
        {
            set { texture = value; }
        }

        public OverlayPostEffect(string name, float start, float end)
            : base(name, start, end)
        {
            technique = "Blend";
        }

        protected override void Initialize()
        {
            if (texture == null && filename == null)
                throw new DDXXException("OverlayPostEffect.Filename or OverlayPostEffect.Texture must be set before Initialize is called.");

            if (!addNoise && !subtractNoise)
                throw new DDXXException("Either AddNoise or SubtractNoise must be set to true for OverlayPostEffect.");

            if (addNoise && subtractNoise)
                throw new DDXXException("AddNoise and SubtractNoise cen not both be set for OverlayPostEffect.");

            if (texture == null && filename != "")
                texture = TextureFactory.CreateFromName(filename);
            else
                texture = TextureFactory.WhiteTexture;
        }

        public override void Render()
        {
            if (addNoise)
                PostProcessor.SetBlendParameters(BlendFunction.Add, Blend.One, Blend.InverseSourceColor, Color.White);
            if (subtractNoise)
                PostProcessor.SetBlendParameters(BlendFunction.ReverseSubtract, Blend.One, Blend.One, Color.White);
            float factor = BlendFactor * GetFadeAlpha();
            PostProcessor.SetValue("Color", new float[] { factor, factor, factor, factor});

            //PostProcessor.OutputTexture.GetTexture().Save("before.jpg", ImageFileFormat.Jpg);
            PostProcessor.Process(technique, texture, PostProcessor.OutputTexture);
            //PostProcessor.OutputTexture.GetTexture().Save("after.jpg", ImageFileFormat.Jpg);
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
