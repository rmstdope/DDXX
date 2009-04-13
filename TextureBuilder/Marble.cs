using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.TextureBuilder
{
    public class Marble : PerlinTurbulence
    {
        private float veinPeriodX;
        private float veinPeriodY;
        private float turbPower;
        private float hue;
        private float saturation;
        private float luminance;

        [TweakStep(0.01f)]
        public float Saturation
        {
            get { return saturation; }
            set { saturation = value; }
        }
        [TweakStep(0.01f)]
        public float Hue
        {
            get { return hue; }
            set { hue = value; }
        }
        [TweakStep(0.01f)]
        public float Luminance
        {
            get { return luminance; }
            set { luminance = value; }
        }

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
        public float TurbPower
        {
            get { return turbPower; }
            set { turbPower = value; }
        }

        public Marble()
            : base()
        {
            this.veinPeriodX = 1.0f;
            this.veinPeriodY = 1.0f;
            this.turbPower = 32;
            hue = 0.5f;
            saturation = 0.4f;
            luminance = 0.04f;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float value = base.GetPixel(textureCoordinate, texelSize).X;
            value = value * 2 - 1;
            value = (float)Math.Cos((textureCoordinate.X * veinPeriodX +
                textureCoordinate.Y * veinPeriodY + value * turbPower) * (float)Math.PI);
            value = (value + 1) / 2.0f;
            //return Color.White.ToVector4() * value +
            //    Color.CornflowerBlue.ToVector4() * (1 - value);
            Vector4 hsla = new Vector4(hue + value, saturation, 0.0f + luminance * value, value);
            return HslaToRgba(hsla);
        }

        public float Undulate(float x) 
        {
            if (x < -0.4f) 
	            return 0.15f + 2.857f * (float)Math.Sqrt(x + 0.75f);
            else if (x < 0.4f)
                return 0.95f - 2.8125f * (float)Math.Sqrt(x);
            else
                return 0.26f + 2.66f * (float)Math.Sqrt(x - 0.7f);
        }

    }
}
