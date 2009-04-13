using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class OldMarble : Generator
    {
        private PerlinTurbulence perlinGenerator;
        private float veinPeriodX;
        private float veinPeriodY;
        private float turbSize;
        private float turbPower;

        [TweakStep(0.1f)]
        public float VeinPeriodX
        {
            get { return veinPeriodX; }
            set { veinPeriodX = value; }
        }

        [TweakStep(0.1f)]
        public float VeinPeriodY
        {
            get { return veinPeriodY; }
            set { veinPeriodY = value; }
        }

        [TweakStep(1.0f)]
        public float TurbSize
        {
            get { return turbSize; }
            set { turbSize = value; }
        }

        [TweakStep(1.0f)]
        public float TurbPower
        {
            get { return turbPower; }
            set { turbPower = value; }
        }

        public OldMarble()
            : base(0)
        {
            this.turbSize = 8;
            this.veinPeriodX = 1.0f;
            this.veinPeriodY = 1.0f;
            this.turbPower = 32;
            perlinGenerator = new PerlinTurbulence();
            perlinGenerator.NumOctaves = 6;
            perlinGenerator.BaseFrequency = (int)turbSize;
            perlinGenerator.Persistence = 0.5f;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float value = perlinGenerator.GetPixel(textureCoordinate, texelSize).X;
            value = value * 2 - 1;
            value = (float)Math.Cos((textureCoordinate.X * veinPeriodX +
                textureCoordinate.Y * veinPeriodY + value * turbPower) * (float)Math.PI);
            value = (value + 1) / 2.0f;
            Vector4 hsla = new Vector4(0.5f, 0.4f, 0.0f + 0.04f / value, value);
            return HslaToRgba(hsla);
            //return new Vector4(value, value, value, value);
        }
    }
}
