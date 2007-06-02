using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public class Rand
    {
        private static Random random = new Random();

        public static Random Random
        {
            set { Rand.random = value; }
        }

        public static float Float(float min, float max)
        {
            float range = max - min;
            return (float)random.NextDouble() * range + min;
        }

        public static int Int(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
