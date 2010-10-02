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

        protected override Vector4 GetPixel()
        {
            float weightSum = 0;
            Vector4 value = Vector4.Zero;
            for (int y = 0; y < kernel.GetLength(1); y++)
            {
                for (int x = 0; x < kernel.GetLength(0); x++)
                {
                    int newX = X + (x - kernel.GetLength(1) / 2);
                    int newY = Y + (y - kernel.GetLength(0) / 2);
                    if (newX >= 0 &&
                        newX < Width &&
                        newY >= 0 &&
                        newY >= Height)
                    {
                        value += kernel[x, y] * GetInputPixel(0, (x - kernel.GetLength(0) / 2), (y - kernel.GetLength(1) / 2));
                        weightSum += kernel[x, y];
                    }
                }
            }
            value /= weightSum;
            return value;
        }
    }
}
