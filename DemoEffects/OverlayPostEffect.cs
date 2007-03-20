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

        public bool SubtractNoise
        {
            get { return subtractNoise; }
            set { subtractNoise = value; }
        }

        public bool AddNoise
        {
            get { return addNoise; }
            set { addNoise = value; }
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

        public OverlayPostEffect(float start, float end)
            : base(start, end)
        {
        }

        public override void Initialize(IPostProcessor postProcessor)
        {
            base.Initialize(postProcessor);

            if (filename == null)
                throw new DDXXException("OverlayPostEffect.Filename must be set before Initialize is called.");

            if (!addNoise && !subtractNoise)
                throw new DDXXException("Either AddNoise or SubtractNoise must be set to true for OverlayPostEffect.");

            if (addNoise && subtractNoise)
                throw new DDXXException("AddNoise and SubtractNoise cen not both be set for OverlayPostEffect.");

            texture = TextureFactory.CreateFromFile(filename);
        }

        public override void Render()
        {
            if (addNoise)
                PostProcessor.SetBlendParameters(BlendOperation.Add, Blend.One, Blend.InvSourceColor, Color.White);
            if (subtractNoise)
                PostProcessor.SetBlendParameters(BlendOperation.RevSubtract, Blend.One, Blend.One, Color.White);
            PostProcessor.SetValue("Color", new float[] { BlendFactor, BlendFactor, BlendFactor, BlendFactor });

            PostProcessor.Process("Blend", texture, PostProcessor.OutputTextureID);
        }
    }
}
