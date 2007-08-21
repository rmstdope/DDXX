using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoEffects
{
    public class GlowPostEffect : BaseDemoPostEffect
    {
        private float luminance;
        private float exposure;
        private float whiteCutoff;
        private float bloomScale;
        private int downSamples;
        private bool advancedGlow;

        public float BloomScale
        {
            get { return bloomScale; }
            set { bloomScale = value; }
        }

        public float WhiteCutoff
        {
            get { return whiteCutoff; }
            set { whiteCutoff = value; }
        }

        public float Exposure
        {
            get { return exposure; }
            set { exposure = value; }
        }

        public float Luminance
        {
            get { return luminance; }
            set { luminance = value; }
        }

        public int DownSamples
        {
            get { return downSamples; }
            set { downSamples = value; }
        }

        public bool AdvancedGlow
        {
            get { return advancedGlow; }
            set { advancedGlow = value; }
        }

        public GlowPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            Luminance = 0.2f;
            SetStepSize(GetTweakableNumber("Luminance"), 0.01f);
            Exposure = 0.1f;
            SetStepSize(GetTweakableNumber("Exposure"), 0.01f);
            WhiteCutoff = 0.1f;
            SetStepSize(GetTweakableNumber("WhiteCutoff"), 0.01f);
            BloomScale = 1.4f;
            SetStepSize(GetTweakableNumber("BloomScale"), 0.1f);
            DownSamples = 1;
        }

        public override void Render()
        {
            IRenderTarget2D startTexture = PostProcessor.OutputTexture;
            List<IRenderTarget2D> textures = PostProcessor.GetTemporaryTextures(2, true);

            PostProcessor.SetValue("Luminance", luminance);
            PostProcessor.SetValue("Exposure", exposure);
            PostProcessor.SetValue("WhiteCutoff", whiteCutoff);
            PostProcessor.SetValue("BloomScale", bloomScale);
            PostProcessor.SetBlendParameters(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
            if (downSamples == 1)
                PostProcessor.Process("DownSample4x", startTexture, textures[1]);
            else
            {
                PostProcessor.Process("DownSample4x", startTexture, textures[0]);
                PostProcessor.Process("DownSample4x", textures[0], textures[1]);
            }
            //textures[1].Save("text.jpg", ImageFileFormat.Jpg);
            if (advancedGlow)
                PostProcessor.Process("AdvancedBrighten", textures[1], textures[0]);
            else
                PostProcessor.Process("Brighten", textures[1], textures[0]);
            PostProcessor.Process("HorizontalBloom", textures[0], textures[1]);
            PostProcessor.Process("VerticalBloom", textures[1], textures[0]);
            PostProcessor.Process("HorizontalBloom", textures[0], textures[1]);
            PostProcessor.Process("VerticalBloom", textures[1], textures[0]);
            if (downSamples == 1)
            {
                PostProcessor.SetBlendParameters(BlendFunction.Add, Blend.One, Blend.One, Color.Black);
                PostProcessor.Process("UpSample4x", textures[0], startTexture);
            }
            else
            {
                PostProcessor.Process("UpSample4x", textures[0], textures[1]);
                PostProcessor.SetBlendParameters(BlendFunction.Add, Blend.One, Blend.One, Color.Black);
                PostProcessor.Process("UpSample4x", textures[1], startTexture);
            }
            //PostProcessor.Process("Copy", temp[0], startTexture);
        }

        protected override void Initialize()
        {
        }
    }
}
