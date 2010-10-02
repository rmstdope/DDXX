using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class Multiply : Generator
    {
        public Multiply()
            : base(2)
        {
        }

        protected override Vector4 GetPixel()
        {
            Vector4 input1 = GetInputPixel(0, 0, 0);
            Vector4 input2 = GetInputPixel(1, 0, 0);
            return input1 * input2;
        }
    }
}
