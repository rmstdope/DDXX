using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class Negate : Generator
    {
        public Negate()
            : base(1)
        {
        }

        protected override Vector4 GetPixel()
        {
            return Vector4.One - GetInputPixel(0, 0, 0);
        }
    }
}
