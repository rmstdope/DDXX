using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public abstract class RandomGenerator : Generator
    {
        private int randomSeed;

        public int RandomSeed
        {
            set { randomSeed = value; }
            get { return randomSeed; }
        }

        public RandomGenerator(int inputPins)
            : base(inputPins)
        {
        }

        protected float PseudoRandom(int x, int y, int frequency)
        {
            if (x < 0)
                x += frequency;
            if (x >= frequency)
                x -= frequency;
            if (y < 0)
                y += frequency;
            if (y >= frequency)
                y -= frequency;
            int n = x + randomSeed + y * (randomSeed + 521);// + frequency);
            n = (n << 13) ^ n;
            float v = (float)(((n * (n * n * 15731 + 789221) + 1376312589) & 0x7fffffff) / 1073741824.0);
            v = 1.0f - v;
            return v;
        }

    }
}
