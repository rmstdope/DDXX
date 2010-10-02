using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class Circle : Generator
    {
        private float solidRadius;
        private float gradientRadius1;
        private float gradientRadius2;
        private float gradientBreak;
        private Vector2 center;

        [TweakStep(0.01f)]
        public float SolidRadius
        {
            get { return solidRadius; }
            set 
            {
                if (value > GradientRadius1)
                    GradientRadius1 = value;
                solidRadius = value; 
            }
        }

        [TweakStep(0.01f)]
        public float GradientRadius1
        {
            get { return gradientRadius1; }
            set
            {
                if (value < SolidRadius) 
                    SolidRadius = value;
                if (value > GradientRadius2)
                    GradientRadius2 = value;
                gradientRadius1 = value; 
            }
        }

        [TweakStep(0.01f)]
        public float GradientRadius2
        {
            get { return gradientRadius2; }
            set
            {
                if (value < GradientRadius1)
                    GradientRadius1 = value;
                gradientRadius2 = value;
            }
        }

        [TweakStep(0.01f)]
        public float GradientBreak
        {
            get { return gradientBreak; }
            set { gradientBreak = value; }
        }

        [TweakStep(0.01f)]
        public Vector2 Center
        {
            get { return center; }
            set { center = value; }
        }

        public Circle()
            : base(0)
        {
            solidRadius = 0.5f;
            gradientRadius1 = 0.5f;
            gradientRadius2 = 0.5f;
            gradientBreak = 0.0f;
            center = new Vector2(0.5f, 0.5f);
        }

        protected override Vector4 GetPixel()
        {
            Vector2 recenteredCoordinate = textureCoordinate - center;// new Vector2(0.5f, 0.5f);
            if (recenteredCoordinate.X > 0.5f)
                recenteredCoordinate.X = 1 - recenteredCoordinate.X;
            if (recenteredCoordinate.Y > 0.5f)
                recenteredCoordinate.Y = 1 - recenteredCoordinate.Y;

            if (recenteredCoordinate.Length() <= solidRadius)
                return new Vector4(1, 1, 1, 1);
            if (recenteredCoordinate.Length() <= gradientRadius1)
            {
                float v = 1 - (1 - gradientBreak) * (recenteredCoordinate.Length() - solidRadius) / (gradientRadius1 - solidRadius);
                return new Vector4(v, v, v, v);
            }
            else if (recenteredCoordinate.Length() <= gradientRadius2)
            {
                float v = gradientBreak * (1 - (recenteredCoordinate.Length() - gradientRadius1) / (gradientRadius2 - gradientRadius1));
                return new Vector4(v, v, v, v);
            }
            return new Vector4();
        }
    }
}
