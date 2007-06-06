using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.TextureBuilder
{
    public class Modulate : Generator
    {
        public Modulate()
            : base(2)
        {
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector4 input1 = GetInput(0, textureCoordinate);
            Vector4 input2 = GetInput(1, textureCoordinate);
            return new Vector4(input1.X * input2.X, input1.Y * input2.Y,
                input1.Z * input2.Z, input1.W * input2.W);
        }
    }
}
