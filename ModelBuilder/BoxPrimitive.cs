using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class BoxPrimitive : ModifierBase
    {
        private float width;
        private float height;
        private float length;

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Length
        {
            get { return length; }
            set { length = value; }
        }

        public BoxPrimitive()
            : base(0)
        {
        }

        public override IPrimitive Generate()
        {
            Vertex[] vertices = new Vertex[24];
            for (int w = 0; w < 24; w++)
                vertices[w] = new Vertex();
            short[] indices = new short[36];
            short v = 0;
            short i = 0;
            // Front
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v].U = 0;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v].U = 1;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v].U = 0;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v].U = 1;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            // Back
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v].U = 0;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v].U = 1;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v].U = 0;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v].U = 1;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            // Top
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v].U = 0;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v].U = 1;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v].U = 0;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v].U = 1;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            // Bottom
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v].U = 0;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v].U = 1;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v].U = 0;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v].U = 1;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            // Left
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v].U = 0;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v].U = 1;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v].U = 0;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v].U = 1;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            // Right
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v].U = 0;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v].U = 1;
            vertices[v].V = 0;
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v].U = 0;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v].U = 1;
            vertices[v].V = 1;
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);

            return new Primitive(vertices, indices);
        }

        internal static short BoxAddIndicesForSide(short v, short i,
            short[] indices, int widthSegments, int heightSegments)
        {
            for (int y = 0; y < heightSegments; y++)
            {
                for (int x = 0; x < widthSegments; x++)
                {
                    int vertex = v + x + y * (widthSegments + 1);
                    indices[i++] = (short)vertex;
                    indices[i++] = (short)(vertex + 1);
                    indices[i++] = (short)(vertex + (widthSegments + 1));
                    indices[i++] = (short)(vertex + 1);
                    indices[i++] = (short)(vertex + 1 + +(widthSegments + 1));
                    indices[i++] = (short)(vertex + +(widthSegments + 1));
                }
            }
            return i;
        }

    }
}
