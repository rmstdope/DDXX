using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class ColorModulation : Generator
    {
        private Vector4 color;

        [TweakStep(0.1f)]
        public Vector4 Color
        {
            get { return color; }
            set { color = value; }
        }
        
        public ColorModulation()
            : base(1)
        {
        }

        protected override Vector4 GetPixel()
        {
            Vector4 input = GetInputPixel(0, 0, 0);
            return new Vector4(color.X * input.X, color.Y * input.Y, color.Z * input.Z, color.W * input.W);
        }
    }
}
