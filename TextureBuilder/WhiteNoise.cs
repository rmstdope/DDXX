using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class WhiteNoise : RandomGenerator
    {
        public WhiteNoise()
            : base(0)
        {
        }

        protected override Vector4 GetPixel()
        {
            int x = (int)(10 * textureCoordinate.X / texelSize.X);
            int y = (int)(10 * textureCoordinate.Y / texelSize.Y);
            float value = PseudoRandom(x, y, 65536);
            value = (value + 1) * 0.5f;
            return new Vector4(value, value, value, value);
        }
    }
}
