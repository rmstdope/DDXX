using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class Primitive : IPrimitive
    {
        private Vertex[] vertices;
        private short[] indices;
        private IBody body;

        public Vertex[] Vertices
        {
            get { return vertices; }
            set { vertices = value; }
        }

        public short[] Indices
        {
            get { return indices; }
            set { indices = value; }
        }

        public IBody Body
        {
            get { return body; }
            set { body = value; }
        }


        public Primitive(Vertex[] vertices, short[] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }

        public Primitive(Vertex[] vertices, short[] indices, IBody body)
        {
            Vertices = vertices;
            Indices = indices;
            Body = body;
        }

        public void Calculate()
        {
            Vector3[] cumulativeTangents = new Vector3[Vertices.Length];

            for (int i = 0; i < Indices.Length / 3; i++)
            {
                short i1 = Indices[i * 3 + 0];
                short i2 = Indices[i * 3 + 1];
                short i3 = Indices[i * 3 + 2];

                float x1 = Vertices[i2].Position.X - Vertices[i1].Position.X;
                float x2 = Vertices[i3].Position.X - Vertices[i1].Position.X;
                float y1 = Vertices[i2].Position.Y - Vertices[i1].Position.Y;
                float y2 = Vertices[i3].Position.Y - Vertices[i1].Position.Y;
                float z1 = Vertices[i2].Position.Z - Vertices[i1].Position.Z;
                float z2 = Vertices[i3].Position.Z - Vertices[i1].Position.Z;
                Vector3 v1 = Vertices[i2].Position - Vertices[i1].Position;
                Vector3 v2 = Vertices[i3].Position - Vertices[i1].Position;

                float s1 = Vertices[i2].U - Vertices[i1].U;
                float s2 = Vertices[i3].U - Vertices[i1].U;
                float t1 = Vertices[i2].V - Vertices[i1].V;
                float t2 = Vertices[i3].V - Vertices[i1].V;

                float r = (s1 * t2 - s2 * t1);
                if (r < 0.0000001f && r > -0.0000001f)
                    r = 1000000.0f;
                else
                    r = 1.0f / r;

                Vector3 sdir = (v1 * t2 - v2 * t1) * r;
                if (sdir != Vector3.Zero)
                    sdir.Normalize();

                cumulativeTangents[i1] += sdir;
                cumulativeTangents[i2] += sdir;
                cumulativeTangents[i3] += sdir;
            }

            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector3 tangent = cumulativeTangents[i] - Vertices[i].Normal * Vector3.Dot(Vertices[i].Normal, cumulativeTangents[i]);
                if (tangent == new Vector3())
                    tangent = new Vector3(1, 0, 0);
                else
                    tangent.Normalize();
                Vertices[i].Tangent = tangent;
                Vertices[i].BiNormal = Vector3.Cross(tangent, Vertices[i].Normal);
            }

        }
    }
}
