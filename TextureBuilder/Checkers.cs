using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class Checkers : Generator
    {
        private int numRepeatsX;
        private int numRepeatsY;

        public int NumRepeatsY
        {
            get { return numRepeatsY; }
            set { numRepeatsY = value; }
        }

        public int NumRepeatsX
        {
            get { return numRepeatsX; }
            set { numRepeatsX = value; }
        }

        public Checkers()
            : base(0)
        {
            numRepeatsX = 1;
            numRepeatsY = 1;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float xWidth = 1.0f / (numRepeatsX * 2);
            float yWidth = 1.0f / (numRepeatsY * 2);
            int wrapX = (int)(textureCoordinate.X / xWidth);
            int wrapY = (int)(textureCoordinate.Y / yWidth);
            if (((wrapX + wrapY) & 1) == 1)
                return Vector4.One;
            return Vector4.Zero;
        }
    }
}
