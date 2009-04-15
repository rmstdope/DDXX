using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class Divide : Generator
    {
        private bool aDividedByB;

        public bool ADividedByB
        {
            get { return aDividedByB; }
            set { aDividedByB = value; }
        }

        public Divide()
            : base(2)
        {
            aDividedByB = true;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector4 input1 = GetInputPixel(0, textureCoordinate, texelSize);
            Vector4 input2 = GetInputPixel(1, textureCoordinate, texelSize);
            if (aDividedByB)
                return input1 / input2;
            return input2 / input1;
        }
    }
}
