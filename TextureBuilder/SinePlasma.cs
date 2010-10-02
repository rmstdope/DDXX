using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class SinePlasma : Generator
    {
        private float offsetX;
        private float offsetY;
        private int numSinesX;
        private int numSinesY;

        [TweakStep(0.1f)]
        public float OffsetX
        {
            get { return offsetX; }
            set { offsetX = value; }
        }

        [TweakStep(0.1f)]
        public float OffsetY
        {
            get { return offsetY; }
            set { offsetY = value; }
        }

        public int NumSinesX
        {
            get { return numSinesX; }
            set { numSinesX = value; }
        }

        public int NumSinesY
        {
            get { return numSinesY; }
            set { numSinesY = value; }
        }

        public SinePlasma()
            : base(0)
        {
            numSinesX = 5;
            numSinesY = 6;
        }

        protected override Vector4 GetPixel()
        {
            float value;
            float x = 0.25f * (float)Math.Sin((offsetX + textureCoordinate.X) * (MathHelper.TwoPi * numSinesX));
            float y = 0.25f * (float)Math.Cos((offsetY + textureCoordinate.Y) * (MathHelper.TwoPi * numSinesY));
            value = 0.5f + x + y;
            return new Vector4(value, value, value, value);
        }
    }
}
