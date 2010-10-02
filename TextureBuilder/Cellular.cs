using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class Cellular : Generator
    {
        private int randomSeed;
        private int numPoints;
        private List<Vector2> points;
        private float maxDistance;
        private int algorithm;
        private const int NumAlgorithms = 3;

        public int Algorithm
        {
            get { return algorithm; }
            set 
            { 
                if (value >= 0 && value < NumAlgorithms)
                    algorithm = value; 
            }
        }

        public int RandomSeed
        {
            get { return randomSeed; }
            set { randomSeed = value; Recalculate(); }
        }

        public int NumPoints
        {
            get { return numPoints; }
            set { numPoints = value; Recalculate(); }
        }

        private void Recalculate()
        {
            Random rand = new Random(randomSeed);
            points = new List<Vector2>();
            for (int i = 0; i < numPoints; i++)
                points.Add(new Vector2((float)rand.NextDouble(), (float)rand.NextDouble()));
            //maxDistance = 0.0f;
            //for (int i = 0; i < points.Count; i++)
            //{
            //    for (int j = i + 1; j < points.Count; j++)
            //    {
            //        float distance = Distance(points[i], points[j]) / 2;
            //        if (distance > maxDistance)
            //            maxDistance = distance;
            //    }
            //}
        }

        private float Distance(Vector2 vector1, Vector2 vector2)
        {
            Vector2 delta = vector1 - vector2;
            if (delta.X < 0)
                delta.X = -delta.X;
            if (delta.Y < 0)
                delta.Y = -delta.Y;
            if (delta.X > 0.5f)
                delta.X = 1.0f - delta.X;
            if (delta.Y > 0.5f)
                delta.Y = 1.0f - delta.Y;
            return delta.Length();
        }

        public Cellular()
            : base(0)
        {
            numPoints = 25;
            randomSeed = Rand.Int(0, 65535);
            Recalculate();
        }

        protected override Vector4 GetPixel()
        {
            Vector2 vec;
            float value;
            switch (algorithm)
            {
                case 0:
                    value = DistanceToClosestPoint(textureCoordinate);
                    break;
                case 1:
                    vec = DistanceToTwoClosestPoints(textureCoordinate);
                    value = vec.Y - vec.X;
                    break;
                default:
                    vec = DistanceToTwoClosestPoints(textureCoordinate);
                    value = vec.Y * vec.X;// / maxDistance;
                    break;
            }
            maxDistance = Math.Max(value, maxDistance);
            return Vector4FromFloat(value);// / maxDistance);
        }

        private Vector2 DistanceToTwoClosestPoints(Vector2 textureCoordinate)
        {
            Vector2 min = Vector2.One;
            foreach (Vector2 vector in points)
            {
                float distance = Distance(textureCoordinate, vector);
                if (distance < min.X)
                {
                    min.Y = min.X;
                    min.X = distance;
                }
                else if (distance < min.Y)
                    min.Y = distance;
            }
            return min;
        }

        private float DistanceToClosestPoint(Vector2 textureCoordinate)
        {
            float min = 1.0f;
            foreach (Vector2 vector in points)
            {
                float distance = Distance(textureCoordinate, vector);
                if (distance < min)
                    min = distance;
            }
            return min;
        }

        protected override void StartGeneration()
        {
            maxDistance = 0.0f;
        }

        protected override void EndGeneration(Vector4[,] pixels, int width, int height)
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixels[x, y] /= maxDistance;
                }
            }
        }

    }
}
