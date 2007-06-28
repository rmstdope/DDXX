using System;
using System.Collections.Generic;
using System.Text;

using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

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

        public GlowPostEffect(float startTime, float endTime)
            : base(startTime, endTime)
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
            TextureID[] temp = new TextureID[2];
            TextureID startTexture = PostProcessor.OutputTextureID;
            if (PostProcessor.OutputTextureID == TextureID.FULLSIZE_TEXTURE_1)
            {
                temp[0] = TextureID.FULLSIZE_TEXTURE_2;
                temp[1] = TextureID.FULLSIZE_TEXTURE_3;
            }
            else if (PostProcessor.OutputTextureID == TextureID.FULLSIZE_TEXTURE_2)
            {
                temp[0] = TextureID.FULLSIZE_TEXTURE_1;
                temp[1] = TextureID.FULLSIZE_TEXTURE_3;
            }
            else
            {
                temp[0] = TextureID.FULLSIZE_TEXTURE_1;
                temp[1] = TextureID.FULLSIZE_TEXTURE_2;
            }
            PostProcessor.SetValue("Luminance", luminance);
            PostProcessor.SetValue("Exposure", exposure);
            PostProcessor.SetValue("WhiteCutoff", whiteCutoff);
            PostProcessor.SetValue("BloomScale", bloomScale);
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            if (downSamples == 1)
                PostProcessor.Process("DownSample4x", PostProcessor.OutputTextureID, temp[1]);
            else
            {
                PostProcessor.Process("DownSample4x", PostProcessor.OutputTextureID, temp[0]);
                PostProcessor.Process("DownSample4x", temp[0], temp[1]);
            }
            if (advancedGlow)
                PostProcessor.Process("AdvancedBrighten", temp[1], temp[0]);
            else
                PostProcessor.Process("Brighten", temp[1], temp[0]);
            PostProcessor.Process("HorizontalBloom", temp[0], temp[1]);
            PostProcessor.Process("VerticalBloom", temp[1], temp[0]);
            PostProcessor.Process("HorizontalBloom", temp[0], temp[1]);
            PostProcessor.Process("VerticalBloom", temp[1], temp[0]);
            if (downSamples == 1)
            {
                PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.One, Color.Black);
                PostProcessor.Process("UpSample4x", temp[0], startTexture);
            }
            else
            {
                PostProcessor.Process("UpSample4x", temp[0], temp[1]);
                PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.One, Color.Black);
                PostProcessor.Process("UpSample4x", temp[1  ], startTexture);
            }
            //PostProcessor.Process("Copy", temp[0], startTexture);
        }

        protected override void Initialize()
        {
        }
    }
}
