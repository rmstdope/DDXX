using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.MeshBuilder
{
    public class Primitive : IPrimitive
    {
        private Vertex[] vertices;
        private short[] indices;

        public Primitive(Vertex[] vertices, short[] indices)
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
            short v = 0;
            short i = 0;
            Vertex[] vertices = new Vertex[24];
            short[] indices = new short[36];
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

        private static short BoxAddIndicesForSide(short v, short i, short[] indices)
        {
            indices[i++] = v;
            indices[i++] = (short)(v + 1);
            indices[i++] = (short)(v + 2);
            indices[i++] = v;
            indices[i++] = (short)(v + 2);
            indices[i++] = (short)(v + 3);
            return i;
        }

        public Vertex[] Vertices
        {
            get
            {
                return vertices;
            }
        }

        public short[] Indices
        {
            get
            {
                return indices;
            }
        }

        public IMesh CreateMesh(IGraphicsFactory factory, IDevice device)
        {
            VertexElementArray declaration = new VertexElementArray();
            declaration.AddPositions();
            IMesh mesh = factory.CreateMesh(indices.Length / 3, vertices.Length, MeshFlags.Managed, 
                declaration.VertexElements, device);
            IGraphicsStream stream = mesh.LockVertexBuffer(LockFlags.Discard);
            for (int i = 0; i < vertices.Length; i++)
                stream.Write(vertices[i].Position);
            mesh.UnlockVertexBuffer();
            stream = mesh.LockIndexBuffer(LockFlags.Discard);
            stream.Write(indices);
            mesh.UnlockIndexBuffer();
            return mesh;
        }

    }
}
