using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class Square : Generator
    {
        private float size;

        [TweakStep(0.01f)]
        public float Size
        {
            get { return size; }
            set { size = value; }
        }

        public Square()
            : base(0)
        {
        }

        protected override Vector4 GetPixel()
        {
            if (textureCoordinate.X < (1 - size) / 2)
                return Vector4.Zero;
            if (textureCoordinate.Y < (1 - size) / 2)
                return Vector4.Zero;
            if (textureCoordinate.X > 1 - (1 - size) / 2)
                return Vector4.Zero;
            if (textureCoordinate.Y > 1 - (1 - size) / 2)
                return Vector4.Zero;
            return Vector4.One;
        }
    }
}
