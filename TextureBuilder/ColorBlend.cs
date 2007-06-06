using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.TextureBuilder
{
    public class ColorBlend : Generator
    {
        private Vector4 baseColor;
        private Vector4 colorDiff;

        public ColorBlend(Vector4 zeroColor, Vector4 oneColor)
            : base(1)
        {
            baseColor = zeroColor;
            colorDiff = oneColor - zeroColor;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            return baseColor + colorDiff * GetInput(0, textureCoordinate).X;
        }
    }
}
