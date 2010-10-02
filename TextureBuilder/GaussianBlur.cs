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
        private bool wrap;

        public bool Wrap
        {
            get { return wrap; }
            set { wrap = value; }
        }

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

        protected override Vector4 GetPixel()
        {
            float weightSum = 0;
            Vector4 value = Vector4.Zero;
            for (int y = 0; y < kernelSize; y++)
            {
                for (int x = 0; x < kernelSize; x++)
                {
                    if (wrap)
                    {
                        value += kernel[x, y] * GetInputPixel(0, (x - kernelSize / 2), (y - kernelSize / 2));
                        weightSum += kernel[x, y];
                    }
                    else
                    {
                        int newX = X + (x - kernelSize / 2);
                        int newY = Y + (y - kernelSize / 2);
                        if (newX >= 0 &&
                            newX < Width &&
                            newY >= 0 &&
                            newY < Height)
                        {
                            value += kernel[x, y] * GetInputPixel(0, (x - kernelSize / 2), (y - kernelSize / 2));
                            weightSum += kernel[x, y];
                        }
                    }
                }
            }
            value /= weightSum;
            return value;
        }
    }
}
