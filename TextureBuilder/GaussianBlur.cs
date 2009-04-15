using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class GaussianBlur : Generator
    {
        private int kernelSize;
        private float[,] kernel;

        [TweakStep(2)]
        public int KernelSize
        {
            get { return kernelSize; }
            set 
            { 
                kernelSize = value;
                if ((kernelSize & 1) == 0)
                    kernelSize++;
                CalculateKernel();
            }
        }

        private void CalculateKernel()
        {
            int maxDistance = (kernelSize + 1) / 2;
            float standardDeviation = (float)(maxDistance / 3.0f);
            kernel = new float[kernelSize, kernelSize];
            float sum = 0;
            for (int y = 0; y < kernelSize; y++)
            {
                for (int x = 0; x < kernelSize; x++)
                {
                    int pointX = x - maxDistance + 1;
                    int pointY = y - maxDistance + 1;
                    kernel[x, y] = (float)Math.Pow(Math.E, -(pointX * pointX + pointY * pointY) / (2 * standardDeviation * standardDeviation));
                    sum += kernel[x, y];
                }
            }
            for (int y = 0; y < kernelSize; y++)
            {
                for (int x = 0; x < kernelSize; x++)
                {
                    kernel[x, y] /= sum;
                }
            }
        }

        public GaussianBlur()
            : base(1)
        {
            kernelSize = 5;
            CalculateKernel();
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float weightSum = 0;
            Vector4 value = Vector4.Zero;
            for (int y = 0; y < kernelSize; y++)
            {
                for (int x = 0; x < kernelSize; x++)
                {
                    Vector2 newCoordinate = textureCoordinate +
                        new Vector2(texelSize.X * (x - kernelSize / 2),
                                    texelSize.Y * (y - kernelSize / 2));
                    if (newCoordinate.X >= 0 &&
                        newCoordinate.X <= 1 &&
                        newCoordinate.Y >= 0 &&
                        newCoordinate.Y <= 1)
                    {
                        value += kernel[x, y] * GetInputPixel(0, newCoordinate, texelSize);
                        weightSum += kernel[x, y];
                    }
                }
            }
            value /= weightSum;
            return value;
        }
    }
}
