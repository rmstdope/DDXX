using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class GaussianBlur : Generator
    {
        public GaussianBlur()
            : base(1)
        {
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float[] kernel = new float[] { 
                0.002216f, 0.008764f, 0.026995f, 0.064759f, 0.120985f, 0.176033f, 0.199471f, 
                0.176033f, 0.120985f, 0.064759f, 0.026995f, 0.008764f, 0.002216f,
            };
            float weightSum = 0;
            Vector4 value = Vector4.Zero;
            for (int y = 0; y < kernel.Length; y++)
            {
                for (int x = 0; x < kernel.Length; x++)
                {
                    Vector2 newCoordinate = textureCoordinate +
                        new Vector2(texelSize.X * (x - kernel.Length / 2),
                                    texelSize.Y * (y - kernel.Length / 2));
                    if (newCoordinate.X >= 0 &&
                        newCoordinate.X <= 1 &&
                        newCoordinate.Y >= 0 &&
                        newCoordinate.Y <= 1)
                    {
                        value += kernel[x] * kernel[y] * GetInputPixel(0, newCoordinate, texelSize);
                        weightSum += kernel[x] * kernel[y];
                    }
                }
            }
            value /= weightSum;
            return value;
        }
    }
}
