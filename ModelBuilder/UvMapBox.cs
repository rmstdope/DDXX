using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class UvMapBox : ModifierBase
    {
        public UvMapBox()
            : base(1)
        {
        }

        public override IPrimitive Generate()
        {
            IPrimitive primitive = GetInput(0);
            float minX = primitive.Vertices[0].Position.X;
            float maxX = primitive.Vertices[0].Position.X;
            float minY = primitive.Vertices[0].Position.Y;
            float maxY = primitive.Vertices[0].Position.Y;
            float minZ = primitive.Vertices[0].Position.Z;
            float maxZ = primitive.Vertices[0].Position.Z;
            foreach (Vertex vertex in primitive.Vertices)
            {
                if (vertex.Position.X > maxX)
                    maxX = vertex.Position.X;
                if (vertex.Position.X < minX)
                    minX = vertex.Position.X;
                if (vertex.Position.Y > maxY)
                    maxY = vertex.Position.Y;
                if (vertex.Position.Y < minY)
                    minY = vertex.Position.Y;
                if (vertex.Position.Z > maxZ)
                    maxZ = vertex.Position.Z;
                if (vertex.Position.Z < minZ)
                    minZ = vertex.Position.Z;
            }
            Vector3[] side = new Vector3[primitive.Vertices.Length];
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                float ax = Math.Abs(primitive.Vertices[i].Normal.X);
                float ay = Math.Abs(primitive.Vertices[i].Normal.Y);
                float az = Math.Abs(primitive.Vertices[i].Normal.Z);
                if (ax > ay && ax > az)
                    side[i] = new Vector3(1, 0, 0);
                else if (ay > ax && ay > az)
                    side[i] = new Vector3(0, 1, 0);
                else
                    side[i] = new Vector3(0, 0, 1);
            }
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                if (side[i] == new Vector3(1, 0, 0))
                {
                    primitive.Vertices[i].U = (primitive.Vertices[i].Position.Z - minZ) / (maxZ - minZ);
                    primitive.Vertices[i].V = (primitive.Vertices[i].Position.Y - minY) / (maxY - minY);
                }
                else if (side[i] == new Vector3(0, 0, 1))
                {
                    primitive.Vertices[i].U = (primitive.Vertices[i].Position.X - minX) / (maxX - minX);
                    primitive.Vertices[i].V = (primitive.Vertices[i].Position.Y - minY) / (maxY - minY);
                }
                else
                {
                    primitive.Vertices[i].U = (primitive.Vertices[i].Position.X - minX) / (maxX - minX);
                    primitive.Vertices[i].V = (primitive.Vertices[i].Position.Z - minZ) / (maxZ - minZ);
                }
            }
            return primitive;
        }
    }
}
