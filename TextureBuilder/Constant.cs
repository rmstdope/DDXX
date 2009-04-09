using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class Constant : Generator
    {
        private Vector4 color;

        [TweakStep(0.1f)]
        public Vector4 Color
        {
            get { return color; }
            set { color = value; }
        }

        public Constant()
            : base(0)
        {
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            return color;
        }
    }
}
