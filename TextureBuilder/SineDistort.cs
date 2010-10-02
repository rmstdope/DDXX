using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class SineDistort : Generator
    {
        private float multiplier;

        [TweakStep(0.05f)]
        public float Multiplier
        {
            get { return multiplier; }
            set { multiplier = value; }
        }

        public SineDistort()
            : base(2)
        {
            multiplier = 0.1f;
        }

        protected override Vector4 GetPixel()
        {

            Vector4 sample = GetInputPixel(1, textureCoordinate, texelSize);
            Vector2 offset = new Vector2((float)Math.Sin(sample.X * MathHelper.TwoPi),
                0);//(float)Math.Cos(sample.Y * MathHelper.TwoPi));
            offset *= multiplier;
            return GetInputPixel(0, textureCoordinate + offset, texelSize);
        }
    }
}
