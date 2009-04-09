using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class Madd : Generator
    {
        private float mul;
        private float add;

        [TweakStep(0.1f)]
        public float Add
        {
            get { return add; }
            set { add = value; }
        }

        [TweakStep(0.1f)]
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
            return GetInputPixel(0, textureCoordinate, texelSize) * mul + 
                new Vector4(add, add, add, add);
        }
    }
}
