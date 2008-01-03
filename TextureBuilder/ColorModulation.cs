using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class ColorModulation : Generator
    {
        private Vector4 color;

        public Vector4 Color
        {
            get { return color; }
            set { color = value; }
        }
        
        public ColorModulation()
            : base(1)
        {
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector4 input = GetInputPixel(0, textureCoordinate, texelSize);
            return new Vector4(color.X * input.X, color.Y * input.Y, color.Z * input.Z, color.W * input.W);
        }
    }
}
