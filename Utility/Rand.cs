using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

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
            return random.Next(min, max + 1);
        }

        public static char Char(char min, char max)
        {
            return Convert.ToChar(Int(Convert.ToInt32(min), Convert.ToInt32(max)));
        }

        public static Vector3 Vector3(double max)
        {
            return new Vector3(Float(max), Float(max), Float(max));
        }

        public static Vector3 Vector3(double min, double max)
        {
            return new Vector3(Float(min, max), Float(min, max), Float(min, max));
        }

        public static Vector3 Vector3(Vector3 max)
        {
            return new Vector3(Float(max.X), Float(max.Y), Float(max.Z));
        }

        public static Vector3 Vector3(Vector3 min, Vector3 max)
        {
            return new Vector3(Float(min.X, max.X), Float(min.Y, max.Y), Float(min.Z, max.Z));
        }
    }
}
