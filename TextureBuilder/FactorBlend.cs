using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.TextureBuilder
{
    public class FactorBlend : Generator
    {
        private float factor;

        public FactorBlend(float factor)
            : base(2)
        {
            this.factor = factor;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            return GetInput(1, textureCoordinate) * factor +
                GetInput(0, textureCoordinate) * (1 - factor);
        }
    }
}
