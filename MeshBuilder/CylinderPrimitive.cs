using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class CylinderPrimitive : IModifier
    {
        private float height;
        private float radius;
        private int segments;
        private int heightSegments;

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

        public Primitive Generate()
        {
            if (segments < 3)
                throw new DDXXException("Cylinder must have at least three segments.");
            if (heightSegments < 1)
                throw new DDXXException("Cylinder must have at least three segments.");

            Vertex[] vertices = new Vertex[segments * (heightSegments + 1) + 2];
            short[] indices = new short[3 * 2 * segments + 6 * segments * heightSegments];

            FillVertices(vertices);

            FillIndices(vertices, indices);
            return new Primitive(vertices, indices);
        }

        private void FillIndices(Vertex[] vertices, short[] indices)
        {
            int index = 0;

            for (int i = 0; i < segments; i++)
            {
                indices[index++] = 0;
                indices[index++] = GetIndex(0, i);
                indices[index++] = GetIndex(0, i + 1);
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

            for (int i = 0; i < segments; i++)
            {
                indices[index++] = (short)(vertices.Length - 1);
                indices[index++] = GetIndex(heightSegments, i + 1);
                indices[index++] = GetIndex(heightSegments, i);
            }

        }

        private short GetIndex(int segment, int side)
        {
            if (side >= segments)
                side -= segments;
            return (short)(1 + segment * segments + side);
        }

        private void FillVertices(Vertex[] vertices)
        {
            int vertex = 0;
            vertices[vertex].Position = new Vector3(0, height / 2, 0);
            vertices[vertex].Normal = new Vector3(0, 1, 0);
            vertices[vertex].U = 0;
            vertices[vertex++].V = 0;
            for (int i = 0; i < heightSegments + 1; i++)
            {
                float yPos = height / 2 - height * i / (float)heightSegments;
                for (int j = 0; j < segments; j++)
                {
                    float phi = j * 2 * (float)Math.PI / segments;
                    Vector3 position = new Vector3((float)Math.Sin(phi), 0, (float)Math.Cos(phi));
                    Vector3 normal = position;
                    position *= radius;
                    position.Y = yPos;
                    vertices[vertex].Position = position;
                    vertices[vertex].U = j / (float)(segments - 1);
                    vertices[vertex].V = i / (float)heightSegments;
                    vertices[vertex].Normal = normal;
                    vertex++;
                }
            }
            vertices[vertex].Position = new Vector3(0, -height / 2, 0);
            vertices[vertex].Normal = new Vector3(0, -1, 0);
            vertices[vertex].U = 0;
            vertices[vertex++].V = 10;
        }
    }
}
