using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class Gradient : Generator
    {
        private bool gradientOnX;
        private float noiseAmount;
        private PerlinNoise noise;

        [TweakStep(0.01f)]
        public float NoiseAmount
        {
            get { return noiseAmount; }
            set { noiseAmount = value; }
        }

        public bool GradientOnX
        {
            get { return gradientOnX; }
            set { gradientOnX = value; }
        }

        public Gradient()
            : base(0)
        {
            gradientOnX = false;
            noiseAmount = 0.0f;
            noise = new PerlinTurbulence();
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float pos;
            float random;
            if (gradientOnX)
                random = 0.5f - noise.GetPixel(new Vector2(textureCoordinate.Y, 0), texelSize).X;
            else
                random = 0.5f - noise.GetPixel(new Vector2(textureCoordinate.X, 0), texelSize).X;
            random *= noiseAmount;
            random += 0.5f;
            if (gradientOnX)
                pos = textureCoordinate.X;
            else
                pos = textureCoordinate.Y;
            if (pos <= random)
                return Vector4FromFloat(0.5f * pos / random);
            return Vector4FromFloat(0.5f + 0.5f * (pos - random) / (1.0f - random));
        }
    }
}
