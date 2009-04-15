using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class Multiply : Generator
    {
        public Multiply()
            : base(2)
        {
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector4 input1 = GetInputPixel(0, textureCoordinate, texelSize);
            Vector4 input2 = GetInputPixel(1, textureCoordinate, texelSize);
            return input1 * input2;
        }
    }
}
