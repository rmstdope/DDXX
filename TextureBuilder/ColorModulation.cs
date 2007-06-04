using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;

namespace TextureBuilder
{
    public class ColorModulation : Generator
    {
        private Vector4 color;

        public ColorModulation(Vector4 color)
            : base(1)
        {
            this.color = color;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector4 input = GetInput(0, textureCoordinate);
            return new Vector4(color.X * input.X, color.Y * input.Y, color.Z * input.Z, color.W * input.W);
        }
    }
}
