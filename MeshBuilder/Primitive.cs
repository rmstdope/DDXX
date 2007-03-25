using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;

namespace MeshBuilder
{
    public class Primitive : IPrimitive
    {
        private Vertex[] vertices;
        private int[] indices;

        public Primitive(Vertex[] vertices, int[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;
        }

        /// <summary>
        /// Create a primitive that consist of a box.
        /// </summary>
        /// <param name="length">The length of the box (z)</param>
        /// <param name="width">The width of the box (x)</param>
        /// <param name="height">The height of the box (y)</param>
        /// <param name="lengthSegments">The number of segments the box has in z.</param>
        /// <param name="widthSegments">The number of segments the box has in x.</param>
        /// <param name="heightSegments">The number of segments the box has in y.</param>
        /// <returns></returns>
        public static Primitive BoxPrimitive(float length, float width, float height, 
            int lengthSegments, int widthSegments, int heightSegments)
        {
            int v = 0;
            int i = 0;
            Vertex[] vertices = new Vertex[24];
            int[] indices = new int[36];
            // Front
            i = BoxAddIndicesForSide(v, i, indices);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            // Back
            i = BoxAddIndicesForSide(v, i, indices);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            // Top
            i = BoxAddIndicesForSide(v, i, indices);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            // Bottom
            i = BoxAddIndicesForSide(v, i, indices);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            // Left
            i = BoxAddIndicesForSide(v, i, indices);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            // Right
            i = BoxAddIndicesForSide(v, i, indices);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            return new Primitive(vertices, indices);
        }

        private static int BoxAddIndicesForSide(int v, int i, int[] indices)
        {
            indices[i++] = v;
            indices[i++] = v + 1;
            indices[i++] = v + 2;
            indices[i++] = v;
            indices[i++] = v + 2;
            indices[i++] = v + 3;
            return i;
        }

        public Vertex[] Vertices
        {
            get
            {
                return vertices;
            }
        }

        public int[] Indices
        {
            get
            {
                return indices;
            }
        }

        public IMesh CreateMesh(IGraphicsFactory factory)
        {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
