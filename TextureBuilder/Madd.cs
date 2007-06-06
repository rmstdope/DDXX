using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.TextureBuilder
{
    public class Madd : Generator
    {
        private float mul;
        private float add;

        public Madd(float mul, float add)
            : base(1)
        {
            this.add = add;
            this.mul = mul;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            return GetInput(0, textureCoordinate) * mul + 
                new Vector4(add, add, add, add);
        }
    }
}
