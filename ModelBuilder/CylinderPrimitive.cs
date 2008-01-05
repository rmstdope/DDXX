using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class CylinderPrimitive : ModifierBase
    {
        private float height;
        private float radius;
        private int segments;
        private int heightSegments;
        private bool lid;

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public int Segments
        {
            get { return segments; }
            set { segments = value; }
        }

        public int HeightSegments
        {
            get { return heightSegments; }
            set { heightSegments = value; }
        }

        public bool Lid
        {
            get { return lid; }
            set { lid = value; }
        }

        public CylinderPrimitive()
            : base(0)
        {
        }

        public override IPrimitive Generate()
        {
            if (segments < 3)
                throw new DDXXException("Cylinder must have at least three segments.");
            if (heightSegments < 1)
                throw new DDXXException("Cylinder must have at least one segments.");

            int numVertices = (segments + 1) * (heightSegments + 1);
            int numIndices = 6 * segments * heightSegments;
            if (lid)
            {
                numVertices += 2;
                numIndices += 3 * 2 * segments;
            }

            Vertex[] vertices = new Vertex[numVertices];
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = new Vertex();
            short[] indices = new short[numIndices];

            FillVertices(vertices);

            FillIndices(vertices, indices);
            return new Primitive(vertices, indices);
        }

        private void FillIndices(Vertex[] vertices, short[] indices)
        {
            int index = 0;

            if (lid)
            {
                for (int i = 0; i < segments; i++)
                {
                    indices[index++] = 0;
                    indices[index++] = GetIndex(0, i);
                    indices[index++] = GetIndex(0, i + 1);
                }
            }

            for (int i = 0; i < heightSegments; i++)
            {
                for (int j = 0; j < segments; j++)
                {
                    indices[index++] = GetIndex(i + 0, j + 0);
                    indices[index++] = GetIndex(i + 1, j + 1);
                    indices[index++] = GetIndex(i + 0, j + 1);
                    indices[index++] = GetIndex(i + 0, j + 0);
                    indices[index++] = GetIndex(i + 1, j + 0);
                    indices[index++] = GetIndex(i + 1, j + 1);
                }
            }

            if (lid)
            {
                for (int i = 0; i < segments; i++)
                {
                    indices[index++] = (short)(vertices.Length - 1);
                    indices[index++] = GetIndex(heightSegments, i + 1);
                    indices[index++] = GetIndex(heightSegments, i);
                }
            }

        }

        private short GetIndex(int segment, int side)
        {
            short num = (short)(segment * (segments + 1) + side);
            if (lid)
                num += (short)1;
            return num;
        }

        private void FillVertices(Vertex[] vertices)
        {
            int vertex = 0;
            if (lid)
            {
                vertices[vertex].Position = new Vector3(0, height / 2, 0);
                vertices[vertex].Normal = new Vector3(0, 1, 0);
                vertices[vertex].U = 0;
                vertices[vertex++].V = 0;
            }
            for (int i = 0; i < heightSegments + 1; i++)
            {
                float yPos = height / 2 - height * i / (float)heightSegments;
                for (int j = 0; j < segments + 1; j++)
                {
                    float phi = j * 2 * (float)Math.PI / segments;
                    Vector3 position = new Vector3((float)Math.Sin(phi), 0, -(float)Math.Cos(phi));
                    Vector3 normal = position;
                    position *= radius;
                    position.Y = yPos;
                    vertices[vertex].Position = position;
                    vertices[vertex].U = j / (float)(segments);
                    vertices[vertex].V = i / (float)heightSegments;
                    vertices[vertex].Normal = normal;
                    vertex++;
                }
            }
            if (lid)
            {
                vertices[vertex].Position = new Vector3(0, -height / 2, 0);
                vertices[vertex].Normal = new Vector3(0, -1, 0);
                vertices[vertex].U = 0;
                vertices[vertex++].V = 10;
            }
        }
    }
}
