using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public abstract class Convolution : Generator
    {
        protected float[,] Kernel { get; set; }
        public bool Normalize { get; set; }
        public bool Absolute { get; set; }

        public Convolution(float [,] kernel, bool normalize, bool absolute)
            : base(1)
        {
            Kernel = kernel;
            Normalize = normalize;
            Absolute = absolute;
        }

        protected override Vector4 GetPixel()
        {
            float weightSum = 0;
            Vector4 value = Vector4.Zero;
            for (int y = 0; y < Kernel.GetLength(1); y++)
            {
                for (int x = 0; x < Kernel.GetLength(0); x++)
                {
                    int newX = X + (x - Kernel.GetLength(1) / 2);
                    int newY = Y + (y - Kernel.GetLength(0) / 2);
                    if (newX >= 0 &&
                        newX < Width &&
                        newY >= 0 &&
                        newY < Height)
                    {
                        value += Kernel[x, y] * GetInputPixel(0, (x - Kernel.GetLength(0) / 2), (y - Kernel.GetLength(1) / 2));
                        weightSum += Kernel[x, y];
                    }
                }
            }
            if (Normalize)
                value /= weightSum;
            if (Absolute)
                return Vector4.Max(value, -value);
            else
                return value;
        }
    }
}
