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

        public static float Float(double max)
        {
            return Float(0, max);
        }

        public static float Float(double min, double max)
        {
            double range = max - min;
            return (float)(random.NextDouble() * range + min);
        }

        public static int Int(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
