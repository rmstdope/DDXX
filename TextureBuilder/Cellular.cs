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
            maxDistance = 0.0f;
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = i + 1; j < points.Count; j++)
                {
                    float distance = Distance(points[i], points[j]) / 2;
                    if (distance > maxDistance)
                        maxDistance = distance;
                }
            }
        }

        private float Distance(Vector2 vector1, Vector2 vector2)
        {
            Vector2 delta = vector1 - vector2;
            if (delta.X < 0)
                delta.X = -delta.X;
            if (delta.Y < 0)
                delta.Y = -delta.Y;
            if (delta.X > 0.5f)
                delta.X -= 0.5f;
            if (delta.Y > 0.5f)
                delta.Y -= 0.5f;
            return delta.Length();
        }

        public Cellular()
            : base(0)
        {
            numPoints = 25;
            randomSeed = Rand.Int(0, 65535);
            Recalculate();
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float dist = DistanceToClosestPoint(textureCoordinate);
            return Vector4FromFloat(dist / maxDistance);
        }

        private float DistanceToClosestPoint(Vector2 textureCoordinate)
        {
            float min = 1.0f;
            foreach (Vector2 vector in points)
            {
                float distance = (textureCoordinate - vector).Length();
                if (distance < min)
                    min = distance;
            }
            return min;
        }
    }
}
