using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;

namespace Dope.DDXX.TextureBuilder
{
    public class Median : Generator
    {
        public int FilterSize { get; set; }

        public Median()
            : base(1)
        {
            FilterSize = 3;
        }

        protected override Vector4 GetPixel()
        {
            List<Vector4> samples = new List<Vector4>();
            for (int y = 0; y < FilterSize; y++)
            {
                for (int x = 0; x < FilterSize; x++)
                {
                    int newX = X + (x - FilterSize / 2);
                    int newY = Y + (y - FilterSize / 2);
                    if (newX >= 0 &&
                        newX < Width &&
                        newY >= 0 &&
                        newY < Height)
                    {
                        samples.Add(GetInputPixel(0, (x - FilterSize / 2), (y - FilterSize / 2)));
                    }
                }
            }
            return CalculateMedian(samples);
        }

        private Vector4 CalculateMedian(List<Vector4> samples)
        {
            samples.Sort(delegate(Vector4 a, Vector4 b) {
                return a.LengthSquared().CompareTo(b.LengthSquared()); 
            });
            return samples[samples.Count / 2];
        }
    }
}
