using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class Ellipse : Generator
    {
        private float radiusX;
        private float radiusY;
        private float gradientStart;

        [TweakStep(0.05f)]
        public float RadiusX
        {
            get { return radiusX; }
            set { radiusX = value; }
        }

        [TweakStep(0.05f)]
        public float RadiusY
        {
            get { return radiusY; }
            set { radiusY = value; }
        }

        [TweakStep(0.05f)]
        public float GradientStart
        {
            get { return gradientStart; }
            set { gradientStart = value; }
        }

        public Ellipse()
            : base(0)
        {
            radiusX = 0.4f;
            radiusY = 0.3f;
            gradientStart = 0.5f;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector2 newCoords = textureCoordinate - new Vector2(0.5f, 0.5f);
            float value = newCoords.X * newCoords.X / (radiusX * radiusX) +
                newCoords.Y * newCoords.Y / (radiusY * radiusY);
            if (value <= gradientStart)
                return Vector4.One;
            else if (value <= 1.0f)
            {
                float newValue = 1 - (value - gradientStart) / (1.0f - gradientStart);
                return new Vector4(newValue, newValue, newValue, newValue);
            }
            return Vector4.Zero;
        }
    }
}
