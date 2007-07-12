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

        public float Add
        {
            get { return add; }
            set { add = value; }
        }

        public float Mul
        {
            get { return mul; }
            set { mul = value; }
        }

        public Madd()
            : base(1)
        {
            mul = 1;
            add = 0;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            return GetInput(0, textureCoordinate) * mul + 
                new Vector4(add, add, add, add);
        }
    }
}
