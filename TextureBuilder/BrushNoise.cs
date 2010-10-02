using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class BrushNoise : Generator
    {
        private int pointsPerLine;

        public int PointsPerLine
        {
            get { return pointsPerLine; }
            set { pointsPerLine = value; }
        }

        public BrushNoise()
            : base(0)
        {
        }

        protected override Vector4 GetPixel()
        {
            float[] points = new float[pointsPerLine + 2];
            float[] colors = new float[pointsPerLine + 2];
            Random rand = new Random((int)(textureCoordinate.X * 1000));
            points[0] = 0;
            for (int i = 0; i < pointsPerLine; i++)
                points[i + 1] = (float)rand.NextDouble();
            points[pointsPerLine + 1] = 1;
            Array.Sort(points);
            for (int i = 0; i < pointsPerLine + 2; i++)
                colors[i] = (float)rand.NextDouble();
            for (int i = 1; i < pointsPerLine + 2; i++)
            {
                if (textureCoordinate.Y <= points[i])
                {
                    float delta = (textureCoordinate.Y - points[i - 1]) / (points[i] - points[i - 1]);
                    float color = colors[i - 1] + delta * (colors[i] - colors[i - 1]);
                    return new Vector4(color);
                }
            }
            throw new Exception("Should never happen");
        }
    }
}
