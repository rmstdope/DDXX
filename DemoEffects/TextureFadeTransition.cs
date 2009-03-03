using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoEffects
{
    public class TextureFadeTransition : BaseDemoTransition
    {
        //private string textureName;
        private ITexture2D texture;
        private float fadeDelay;

        //public string TextureName
        //{
        //    get { return textureName; }
        //    set { textureName = value; }
        //}

        public ITexture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        [TweakStep(0.1f)]
        public float FadeDelay
        {
            get { return fadeDelay; }
            set { fadeDelay = value; }
        }

        public TextureFadeTransition(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            fadeDelay = (endTime - startTime) / 2;
        }

        protected override void Initialize()
        {
            //texture = TextureFactory.CreateFromName(textureName);
        }

        public override IRenderTarget2D Combine(IRenderTarget2D fromTexture, IRenderTarget2D toTexture)
        {
            PostProcessor.StartFrame(fromTexture);
            PostProcessor.SetBlendParameters(BlendFunction.Add, Blend.SourceAlpha, Blend.InverseSourceAlpha, Color.Black);
            PostProcessor.SetValue("MiscTexture", texture);
            PostProcessor.SetValue("Time", Time.CurrentTime - StartTime);
            PostProcessor.SetValue("TextureAlphaFadeDelay", fadeDelay);
            PostProcessor.SetValue("TextureAlphaFadeLength", EndTime - StartTime - fadeDelay);
            PostProcessor.Process("TextureAlpha", fromTexture, toTexture);
            return toTexture;
        }
    }
}
