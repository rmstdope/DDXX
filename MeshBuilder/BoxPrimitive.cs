using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    public class BoxPrimitive : IPrimitive
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
        {
            width = 1;
            height = 1;
            length = 1;
        }

        public void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
        {
            vertices = new Vertex[24];
            indices = new short[36];
            short v = 0;
            short i = 0;
            // Front
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            AddBoxNormalsForSide(vertices, v, new Vector3(0, 0, -1));
            AddBoxUvForSide(vertices, v);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            // Back
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            AddBoxNormalsForSide(vertices, v, new Vector3(0, 0, 1));
            AddBoxUvForSide(vertices, v);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            // Top
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            AddBoxNormalsForSide(vertices, v, new Vector3(0, 1, 0));
            AddBoxUvForSide(vertices, v);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            // Bottom
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            AddBoxNormalsForSide(vertices, v, new Vector3(0, -1, 0));
            AddBoxUvForSide(vertices, v);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            // Left
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            AddBoxNormalsForSide(vertices, v, new Vector3(-1, 0, 0));
            AddBoxUvForSide(vertices, v);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            // Right
            i = BoxAddIndicesForSide(v, i, indices, 1, 1);
            AddBoxNormalsForSide(vertices, v, new Vector3(1, 0, 0));
            AddBoxUvForSide(vertices, v);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            body = null;
        }

        private static void AddBoxUvForSide(Vertex[] vertices, short v)
        {
            vertices[v].U = 0;
            vertices[v].V = 0;
            vertices[v + 1].U = 1;
            vertices[v + 1].V = 0;
            vertices[v + 2].U = 0;
            vertices[v + 2].V = 1;
            vertices[v + 3].U = 1;
            vertices[v + 3].V = 1;
        }

        private void AddBoxNormalsForSide(Vertex[] vertices, short v, Vector3 normal)
        {
            for (int i = 0; i < 4; i++)
                vertices[v + i].Normal = normal;
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
