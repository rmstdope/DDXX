using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.TextureBuilder
{
    public class PerlinNoise : Generator
    {
        private int numOctaves;
        private float baseFrequency;
        private float persistence;
        private Vector4 color;
        private Vector4 colorDiff;
        protected bool createTurbulence = false;

        public int NumOctaves
        {
            set { numOctaves = value; }
            get { return numOctaves; }
        }

        public float BaseFrequency
        {
            set { baseFrequency= value; }
            get { return baseFrequency; }
        }

        public float Persistence
        {
            set { persistence = value; }
            get { return persistence; }
        }

        public PerlinNoise()
            : base(0)
        {
            this.numOctaves = 6;
            this.baseFrequency = 4;
            this.persistence = 0.5f;
            color = new Vector4(0, 0, 0, 0);
            colorDiff = new Vector4(1, 1, 1, 1);
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float value = CreatePerlinNoise(textureCoordinate);
            //Vector4 hsla = new Vector4(169 / 255.0f, 255 / 255.0f, 0.75f + value / 3, value);
            //return HslaToRgba(hsla);
            return color + colorDiff * value;
        }

        private float Noise(int x, int y)
        {
            int n = x + y * 57;
            n = (n << 13) ^ n;
            float v = (float)(1.0 - ((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
            return v;
        }

        //private Vector2 CubicInterpolation(Vector2 v0, Vector2 v1, Vector2 v2, Vector2 v3, float delta)
        //{
        //    Vector2 P = (v3 - v2) - (v0 - v1);
        //    Vector2 Q = (v0 - v1) - P;
        //    Vector2 R = v2 - v0;
        //    Vector2 S = v1;
        //    return P * delta * delta * delta + Q * delta * delta + R * delta + S;
        //}

        private float Interpolate(float a, float b, float x)
        {
	        float ft = (float)(x * Math.PI);
	        float f = (float)((1 - Math.Cos(ft)) * 0.5);
	        return a * (1 - f) + b * f;
        }

        private float SmoothNoise(int x, int y)
        {
            float corners = (Noise(x - 1, y - 1) + Noise(x + 1, y - 1) +
                Noise(x - 1, y + 1) + Noise(x + 1, y + 1)) / 16;
            float sides = (Noise(x - 1, y) + Noise(x + 1, y) +
                Noise(x, y - 1) + Noise(x, y + 1)) / 8;
            float center = Noise(x, y) / 4;
            return (corners + sides + center);
        }

        private float InterpolatedNoise(float x, float y)
        {
            int integerX = (int)x;
            float fractionalX = x - integerX;
            int integerY = (int)y;
            float fractionalY = y - integerY;

            float v1 = SmoothNoise(integerX, integerY);
            float v2 = SmoothNoise(integerX + 1, integerY);
            float v3 = SmoothNoise(integerX, integerY + 1);
            float v4 = SmoothNoise(integerX + 1, integerY + 1);

            float i1 = Interpolate(v1, v2, fractionalX);
            float i2 = Interpolate(v3, v4, fractionalX);

            return Interpolate(i1, i2, fractionalY);
        }

        private float CreatePerlinNoise(Vector2 position)
        {
            float total = 0;
            for (int i = 0; i < numOctaves; i++)
            {
                //int i = 0;
                float frequency = baseFrequency * (float)Math.Pow(2, i);
                float amplitude = (float)Math.Pow(persistence, i);

                if (createTurbulence)
                    total += (float)Math.Abs(InterpolatedNoise(position.X * frequency, position.Y * frequency) * amplitude);
                else
                    total += InterpolatedNoise(position.X * frequency, position.Y * frequency) * amplitude;
            }
            if (createTurbulence)
                return total;
            else
                return (total + 1) / 2.0f;
        }


        public void SetColors(Color color1, Color color2)
        {
            color = ColorToRgba(color1);
            Vector4 color2Vec = ColorToRgba(color2);
            colorDiff = color2Vec - color;
        }

    }
}
