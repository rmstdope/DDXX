using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoEffects
{
    public class RadialBlurPostEffect : BaseDemoPostEffect
    {
        public RadialBlurPostEffect(float startTime, float endTime)
            : base(startTime, endTime)
        {
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
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.Zero, Color.Black);
            //PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.SourceAlpha, Blend.Zero, Color.Black);
            PostProcessor.SetValue("ZoomFactor", 0.20f);
            //if (Time.StepTime > 25.0f)
            //    PostProcessor.GetTexture(PostProcessor.OutputTextureID).Save("before.dds", ImageFileFormat.Dds);
            PostProcessor.Process("ZoomAdd", PostProcessor.OutputTextureID, temp[0]);
            //if (Time.StepTime > 25.0f)
            //    PostProcessor.GetTexture(temp[0]).Save("after.dds", ImageFileFormat.Dds);
            float invZoomFactor = 0.8f;
            int source = 0;
            for (int i = 0; i < 6; i++)
            {
                invZoomFactor /= 2;
                PostProcessor.SetValue("ZoomFactor", 1 - invZoomFactor);
                PostProcessor.Process("ZoomAdd", temp[source], temp[1 - source]);
                source = 1 - source;
            }
            PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.One, Color.Black);
            PostProcessor.Process("Copy", temp[source], startTexture);
        }

        protected override void Initialize()
        {
        }
    }
}
