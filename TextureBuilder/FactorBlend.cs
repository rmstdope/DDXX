using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class FactorBlend : Generator
    {
        private float factor;

        [TweakStep(0.1f)]
        public float Factor
        {
            get { return factor; }
            set { factor = value; }
        }

        public FactorBlend()
            : base(2)
        {
            factor = 0.5f;
        }

        protected override Vector4 GetPixel()
        {
            return GetInputPixel(1, 0, 0) * factor +
                GetInputPixel(0, 0, 0) * (1 - factor);
        }
    }
}
