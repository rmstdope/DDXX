using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.TextureBuilder
{
    public class Marble : Generator
    {
        private IGenerator perlinGenerator;
        private float veinPeriodX;
        private float veinPeriodY;
        private float turbSize;
        private float turbPower;

        public Marble(float veinPeriodX, float veinPeriodY, float turbulenceSize, float turbulencePower)
            : base(0)
        {
            this.turbSize = turbulenceSize;
            this.veinPeriodX = veinPeriodX;
            this.veinPeriodY = veinPeriodY;
            this.turbPower = turbulencePower;
            perlinGenerator = new PerlinTurbulence(6, turbSize, 0.5f);
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
