using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class ColorBlend : Generator
    {
        private Vector4 zeroColor;
        private Vector4 oneColor;

        [TweakStep(0.1f)]
        public Vector4 ZeroColor
        {
            get { return zeroColor; }
            set { zeroColor = value; }
        }

        [TweakStep(0.1f)]
        public Vector4 OneColor
        {
            get { return oneColor; }
            set { oneColor = value; }
        }

        public ColorBlend()
            : base(1)
        {
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector4 colorDiff = oneColor - zeroColor;
            return zeroColor + colorDiff * GetInputPixel(0, textureCoordinate, texelSize).X;
        }
    }
}
