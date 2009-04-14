using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class Subtract : Generator
    {
        private bool aMinusB;

        public bool AMinusB
        {
            get { return aMinusB; }
            set { aMinusB = value; }
        }

        public Subtract()
            : base(2)
        {
            aMinusB = true;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector4 input1 = GetInputPixel(0, textureCoordinate, texelSize);
            Vector4 input2 = GetInputPixel(1, textureCoordinate, texelSize);
            if (aMinusB)
                return input1 - input2;
            else
                return input2 - input1;
        }
    }
}
