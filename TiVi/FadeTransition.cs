using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;

namespace TiVi
{
    public class FadeTransition : BaseDemoTransition
    {
        public FadeTransition(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override ITexture Combine(ITexture fromTexture, ITexture toTexture)
        {
            //if (PostProcessor.
            //PostProcessor.StartFrame(fromTexture);
            //PostProcessor.Process("Copy", fromTexture, TextureID.FULLSIZE_TEXTURE_1
            //return fromTexture;
            return toTexture;
        }
    }
}
