using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class Negate : Generator
    {
        public Negate()
            : base(1)
        {
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            return Vector4.One - GetInputPixel(0, textureCoordinate, texelSize);
        }
    }
}
