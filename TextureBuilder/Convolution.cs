using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class Convolution : Generator
    {
        private float[,] kernel;

        public Convolution()
            : base(1)
        {
        }

        public float[,] Kernel
        {
            get { return kernel; }
            set { kernel = value; }
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float weightSum = 0;
            Vector4 value = Vector4.Zero;
            for (int y = 0; y < kernel.GetLength(1); y++)
            {
                for (int x = 0; x < kernel.GetLength(0); x++)
                {
                    Vector2 newCoordinate = textureCoordinate +
                        new Vector2(texelSize.X * (x - kernel.GetLength(0) / 2),
                                    texelSize.Y * (y - kernel.GetLength(1) / 2));
                    if (newCoordinate.X >= 0 &&
                        newCoordinate.X <= 1 &&
                        newCoordinate.Y >= 0 &&
                        newCoordinate.Y <= 1)
                    {
                        value += kernel[x,y] * GetInputPixel(0, newCoordinate, texelSize);
                        weightSum += kernel[x,y];
                    }
                }
            }
            value /= weightSum;
            return value;
        }
    }
}
