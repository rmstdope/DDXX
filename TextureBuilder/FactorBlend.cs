using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class FactorBlend : Generator
    {
        private float factor;

        [TweakStep(0.1f)]
        public float Factor
        {
            get { return factor; }
            set { factor = value; }
        }

        public FactorBlend()
            : base(2)
        {
            factor = 0.5f;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            return GetInputPixel(1, textureCoordinate, texelSize) * factor +
                GetInputPixel(0, textureCoordinate, texelSize) * (1 - factor);
        }
    }
}
