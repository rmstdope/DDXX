using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class TorusPrimitive : ModifierBase
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

        public TorusPrimitive()
            : base(0)
        {
        }

        public override IPrimitive Generate()
        {
            if (segments < 3)
                throw new DDXXException("Torus must have at least three segments.");
            if (sides < 3)
                throw new DDXXException("Torus must have at least three segments.");

            Vertex[] vertices = new Vertex[segments * sides];
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = new Vertex();
            short[] indices = new short[6 * segments * sides];

            FillVertices(vertices);
            FillIndices(indices);

            return new Primitive(vertices, indices);
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
                Matrix rotation = Matrix.CreateRotationY(rho);
                for (int j = 0; j < sides; j++)
                {
                    float phi = j * 2 * (float)Math.PI / sides;
                    Vector3 position = new Vector3((float)Math.Sin(phi), (float)Math.Cos(phi), 0);
                    Vector3 normal = position;
                    position *= smallRadius;
                    position += new Vector3(largeRadius, 0, 0);
                    position = Vector3.Transform(position, rotation);
                    normal = Vector3.TransformNormal(normal, rotation);
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
