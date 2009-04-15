using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class Sine : Generator
    {
        private float period;

        [TweakStep(0.1f)]
        public float Period
        {
            get { return period; }
            set { period = value; }
        }

        public Sine()
            : base(1)
        {
            period = MathHelper.TwoPi;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector4 value = GetInputPixel(0, textureCoordinate, texelSize);
            Vector4 newValue = new Vector4((float)Math.Sin(value.X * period), (float)Math.Sin(value.Y * period),
                (float)Math.Sin(value.Z * period), (float)Math.Sin(value.W * period));
            return (Vector4.One + newValue) * 0.5f;
        }
    }
}
