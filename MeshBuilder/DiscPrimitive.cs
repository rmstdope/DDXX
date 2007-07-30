using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class DiscPrimitive : IModifier
    {
        private float radius;
        private float innerRadius;
        private int segments;

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public float InnerRadius
        {
            get { return innerRadius; }
            set { innerRadius = value; }
        }

        public int Segments
        {
            get { return segments; }
            set { segments = value; }
        }

        public Primitive Generate()
        {
            int innerSegments = 0;
            if (innerRadius > 0)
                innerSegments = segments;
            Vertex[] vertices = new Vertex[segments + innerSegments + 1];
            short[] indices = new short[3 * (segments + innerSegments)];
            FillVertices(vertices);
            FillIndices(indices);
            return new Primitive(vertices, indices);
        }

        private void FillIndices(short[] indices)
        {
            int index = 0;
            if (innerRadius > 0)
            {
                for (int i = 0; i < segments; i++)
                {
                    short i1 = (short)(i + 1);
                    short i2 = (short)(i + 2);
                    short i3 = (short)(i + 1 + segments);
                    short i4 = (short)(i + 2 + segments);
                    if (i2 > segments)
                        i2 -= (short)segments;
                    if (i4 > segments * 2)
                        i4 -= (short)segments;

                    indices[index++] = i1;
                    indices[index++] = i2;
                    indices[index++] = i4;
                    indices[index++] = i1;
                    indices[index++] = i4;
                    indices[index++] = i3;
                }
            }
            else
            {
                for (int i = 0; i < segments; i++)
                {
                    indices[index++] = 0;
                    indices[index++] = (short)i;
                    if (i == segments - 1)
                        indices[index++] = (short)1;
                    else

                        indices[index++] = (short)(i + 1);
                }
            }
        }

        private void FillVertices(Vertex[] vertices)
        {
            vertices[0].Position = new Vector3(0, 0, 0);
            int vertex = 1;
            for (int i = 0; i < segments; i++)
            {
                Vertex v = new Vertex();
                float phi = i * 2 * (float)Math.PI / segments;
                v.Position = new Vector3((float)Math.Sin(phi), 0, (float)Math.Cos(phi));
                v.U = v.Position.X;// (v.Position.X + 1) / 2.0f;
                v.V = v.Position.Z;// (v.Position.Z + 1) / 2.0f;
                v.Position *= Radius;
                v.Normal = new Vector3(0, 1, 0);
                vertices[vertex++] = v;
            }
            if (innerRadius > 0)
            {
                for (int i = 0; i < segments; i++)
                {
                    Vertex v = new Vertex();
                    float phi = i * 2 * (float)Math.PI / segments;
                    v.Position = new Vector3((float)Math.Sin(phi), 0, (float)Math.Cos(phi));
                    v.Position *= InnerRadius;
                    v.Normal = new Vector3(0, 1, 0);
                    vertices[vertex++] = v;
                }
                for (int i = 0; i < segments; i++)
                {
                    vertices[i + 1].U = 100 * i / (float)segments;
                    vertices[i + 1].V = 0;
                    vertices[i + 1 + segments].U = 0;
                    vertices[i + 1 + segments].V = 1;
                }
            }
        }
    }
}
