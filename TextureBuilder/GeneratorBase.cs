using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;

namespace TextureBuilder
{
    public abstract class GeneratorBase : IGenerator
    {
        protected Vector4 ColorToRgba(Color color1)
        {
            return new Vector4(color1.R / 255f, color1.G / 255f, color1.B / 255f, color1.A / 255f);
        }

        protected Vector4 HslaToRgba(Vector4 hsla)
        {
            float r = 0, g = 0, b = 0;
            float temp1, temp2;
            if (hsla.Z == 0)
            {
                r = g = b = 0;
            }
            else
            {
                if (hsla.Y == 0)
                {
                    r = g = b = hsla.Z;
                }
                else
                {
                    temp2 = ((hsla.Z <= 0.5f) ? hsla.Z * (1.0f + hsla.Y) : hsla.Z + hsla.Y - (hsla.Z * hsla.Y));
                    temp1 = 2.0f * hsla.Z - temp2;
                    float[] t3 = new float[] { hsla.X + 1.0f / 3.0f, hsla.X, hsla.X - 1.0f / 3.0f };
                    float[] clr = new float[] { 0, 0, 0 };
                    for (int i = 0; i < 3; i++)
                    {
                        if (t3[i] < 0)
                            t3[i] += 1.0f;
                        if (t3[i] > 1)
                            t3[i] -= 1.0f;
                        if (6.0 * t3[i] < 1.0f)
                            clr[i] = temp1 + (temp2 - temp1) * t3[i] * 6.0f;
                        else if (2.0f * t3[i] < 1.0f)
                            clr[i] = temp2;
                        else if (3.0f * t3[i] < 2.0f)
                            clr[i] = (temp1 + (temp2 - temp1) * ((2.0f / 3.0f) - t3[i]) * 6.0f);
                        else
                            clr[i] = temp1;
                    }
                    r = clr[0];
                    g = clr[1];
                    b = clr[2];
                }
            }
            return new Vector4(r, g, b, hsla.W);
        }

        public abstract Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize);
    }
}
