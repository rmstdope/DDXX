using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class Constant : Generator
    {
        private Vector4 color;

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
