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
    public class GlowPostEffect : BaseDemoPostEffect
    {
        private float luminance;
        private float exposure;
        private float whiteCutoff;
        private float bloomScale;
        private int downSamples;
        private bool advancedGlow;
        private BlendState addBlend;

        [TweakStep(0.1f)]
        public float BloomScale
        {
            get { return bloomScale; }
            set { bloomScale = value; }
        }

        [TweakStep(0.01f)]
        public float WhiteCutoff
        {
            get { return whiteCutoff; }
            set { whiteCutoff = value; }
        }

        [TweakStep(0.01f)]
        public float Exposure
        {
            get { return exposure; }
            set { exposure = value; }
        }

        [TweakStep(0.01f)]
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
            Exposure = 0.1f;
            WhiteCutoff = 0.1f;
            BloomScale = 1.4f;
            DownSamples = 1;
            addBlend = new BlendState();
            addBlend.ColorBlendFunction = addBlend.AlphaBlendFunction = BlendFunction.Add;
            addBlend.ColorSourceBlend = addBlend.AlphaSourceBlend = Blend.One;
            addBlend.ColorDestinationBlend = addBlend.AlphaDestinationBlend = Blend.One;
        }

        public override void Render()
        {
            RenderTarget2D startTexture = PostProcessor.OutputTexture;
            List<RenderTarget2D> textures = PostProcessor.GetTemporaryTextures(2, true);

            PostProcessor.SetValue("Luminance", luminance);
            PostProcessor.SetValue("Exposure", exposure);
            PostProcessor.SetValue("WhiteCutoff", whiteCutoff);
            PostProcessor.SetValue("BloomScale", bloomScale);
            PostProcessor.BlendState = BlendState.Opaque;
            RenderTarget2D afterDownSample = textures[1];
            if (downSamples == 1)
                PostProcessor.Process("DownSample4x", startTexture, textures[1]);
            else if (downSamples > 1)
            {
                PostProcessor.Process("DownSample4x", startTexture, textures[0]);
                //textures[0].GetTexture().Save("save0.jpg", ImageFileFormat.Jpg);
                PostProcessor.Process("DownSample4x", textures[0], textures[1]);
            }
            else 
                afterDownSample = startTexture;
            //textures[1].GetTexture().Save("save1.jpg", ImageFileFormat.Jpg);
            if (advancedGlow)
                PostProcessor.Process("AdvancedBrighten", afterDownSample, textures[0]);
            else
                PostProcessor.Process("Brighten", afterDownSample, textures[0]);
            //textures[0].GetTexture().Save("save2.jpg", ImageFileFormat.Jpg);
            PostProcessor.Process("HorizontalBloom", textures[0], textures[1]);
            PostProcessor.Process("VerticalBloom", textures[1], textures[0]);
            PostProcessor.Process("HorizontalBloom", textures[0], textures[1]);
            if (downSamples == 0)
            {
                PostProcessor.BlendState = addBlend;
                PostProcessor.Process("VerticalBloom", textures[1], startTexture);
            }
            else
                PostProcessor.Process("VerticalBloom", textures[1], textures[0]);
            //textures[0].GetTexture().Save("save3.jpg", ImageFileFormat.Jpg);
            if (downSamples == 1)
            {
                PostProcessor.BlendState = addBlend;
                PostProcessor.Process("UpSample4x", textures[0], startTexture);
            }
            else if (downSamples > 1)
            {
                PostProcessor.Process("UpSample4x", textures[0], textures[1]);
                //textures[1].GetTexture().Save("save4.jpg", ImageFileFormat.Jpg);
                PostProcessor.BlendState = addBlend;
                PostProcessor.Process("UpSample4x", textures[1], startTexture);
                //t.Save("save9.jpg", ImageFileFormat.Jpg);
            }
            //startTexture.GetTexture().Save("save5.jpg", ImageFileFormat.Jpg);
        }

        protected override void Initialize()
        {
        }
    }
}
