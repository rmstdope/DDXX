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

        protected override Vector4 GetPixel()
        {
            Vector4 input1 = GetInputPixel(0, 0, 0);
            Vector4 input2 = GetInputPixel(1, 0, 0);
            if (aMinusB)
                return input1 - input2;
            else
                return input2 - input1;
        }
    }
}
