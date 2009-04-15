using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class OffsetDistort : Generator
    {
        private float multiplier;

        [TweakStep(0.05f)]
        public float Multiplier
        {
            get { return multiplier; }
            set { multiplier = value; }
        }

        public OffsetDistort()
            : base(2)
        {
            multiplier = 0.1f;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector4 sample = GetInputPixel(1, textureCoordinate, texelSize);
            Vector2 offset = new Vector2((sample.X * 2 - 1) * multiplier, (sample.Y * 2 - 1) * multiplier);
            return GetInputPixel(0, textureCoordinate + offset, texelSize);
        }
    }
}
