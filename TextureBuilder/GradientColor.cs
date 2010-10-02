using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class GradientColor : Generator
    {
        private Vector4 color1;
        private Vector4 color2;
        private Vector4 color3;
        private float color2Position;

        [TweakStep(0.1f)]
        public Vector4 Color1
        {
            get { return color1; }
            set { color1 = value; }
        }

        [TweakStep(0.1f)]
        public Vector4 Color2
        {
            get { return color2; }
            set { color2 = value; }
        }

        [TweakStep(0.1f)]
        public Vector4 Color3
        {
            get { return color3; }
            set { color3 = value; }
        }

        [TweakStep(0.1f)]
        public float Color2Position
        {
            get { return color2Position; }
            set { color2Position = value; }
        }

        public GradientColor()
            : base(1)
        {
            color1 = Color.Black.ToVector4();
            color2 = Color.BlueViolet.ToVector4();
            color3 = Color.WhiteSmoke.ToVector4();
            color2Position = 0.5f;
        }

        protected override Vector4 GetPixel()
        {
            float value = GetInputPixel(0, 0, 0).X;
            if (value <= color2Position)
            {
                float d = value / color2Position;
                return color1 * (1 - d) + color2 * d;
            }
            else
            {
                float d = (value - color2Position) / (1.0f - color2Position);
                return color2 * (1 - d) + color3 * d;
            }
        }
    }
}
