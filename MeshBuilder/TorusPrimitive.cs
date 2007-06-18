using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class TorusPrimitive : IPrimitive
    {
        private float smallRadius;
        private float largeRadius;
        private int segments;
        private int sides;

        public float SmallRadius
        {
            get { return smallRadius; }
            set { smallRadius = value; }
        }

        public float LargeRadius
        {
            get { return largeRadius; }
            set { largeRadius = value; }
        }

        public int Segments
        {
            get { return segments; }
            set { segments = value; }
        }

        public int Sides
        {
            get { return sides; }
            set { sides = value; }
        }

        public void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
        {
            if (segments < 3)
                throw new DDXXException("Torus must have at least three segments.");
            if (sides < 3)
                throw new DDXXException("Torus must have at least three segments.");

            vertices = new Vertex[segments * sides];
            indices = new short[6 * segments * sides];
            body = null;

            FillVertices(vertices);

            FillIndices(indices);
        }

        private void FillIndices(short[] indices)
        {
            int index = 0;
            for (int i = 0; i < segments; i++)
            {
                for (int j = 0; j < sides; j++)
                {
                    indices[index++] = GetIndex(i + 0, j + 0);
                    indices[index++] = GetIndex(i + 0, j + 1);
                    indices[index++] = GetIndex(i + 1, j + 1);
                    indices[index++] = GetIndex(i + 0, j + 0);
                    indices[index++] = GetIndex(i + 1, j + 1);
                    indices[index++] = GetIndex(i + 1, j + 0);
                }
            }
        }

        private short GetIndex(int segment, int side)
        {
            if (segment >= segments)
                segment -= segments;
            if (side >= sides)
                side -= sides;
            return (short)(segment * sides + side);
        }

        private void FillVertices(Vertex[] vertices)
        {
            int vertex = 0;
            for (int i = 0; i < segments; i++)
            {
                float rho = i * 2 * (float)Math.PI / segments;
                Matrix rotation = Matrix.RotationY(rho);
                for (int j = 0; j < sides; j++)
                {
                    float phi = j * 2 * (float)Math.PI / sides;
                    Vector3 position = new Vector3((float)Math.Sin(phi), (float)Math.Cos(phi), 0);
                    Vector3 normal = position;
                    position *= smallRadius;
                    position += new Vector3(largeRadius, 0, 0);
                    position.TransformCoordinate(rotation);
                    normal.TransformNormal(rotation);
                    //normal += new Vector3(Rand.Float(-0.4, 0.4), Rand.Float(-0.4, 0.4), Rand.Float(-0.4, 0.4));
                    normal.Normalize();
                    vertices[vertex].Position = position;
                    vertices[vertex].Normal = normal;
                    vertices[vertex].U = 10 * i / (float)segments;
                    vertices[vertex].V = j / (float)sides;
                    //vertices[vertex].V = 0.3f + 0.5f * vertices[vertex].V;
                    vertex++;
                }
            }
        }
    }
}
