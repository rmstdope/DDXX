using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class MeshBuilder
    {
        private IGraphicsFactory factory;
        private ITextureFactory textureFactory;
        private IDevice device;
        private Dictionary<string, IPrimitive> primitives = new Dictionary<string,IPrimitive>();

        public MeshBuilder(IGraphicsFactory factory, ITextureFactory textureFactory, IDevice device)
        {
            this.factory = factory;
            this.textureFactory = textureFactory;
            this.device = device;
        }

        public IPrimitive GetPrimitive(string name)
        {
            return primitives[name];
        }

        public void AddPrimitive(IPrimitive primitive, string name)
        {
            if (primitives.ContainsKey(name))
                throw new DDXXException("Can not add the two primitives with the same name.");
            primitives[name] = primitive;
        }

        public IModel CreateModel(string name)
        {
            if (!primitives.ContainsKey(name))
                throw new DDXXException("Can not create mesh from a primitive that does not exist.");
            return primitives[name].CreateModel(factory, textureFactory, device);
        }

        /// <summary>
        /// Create a primitive that consist of a box.
        /// </summary>
        /// <param name="length">The length of the plane (z)</param>
        /// <param name="width">The width of the plane (x)</param>
        /// <param name="height">The height of the plane (y)</param>
        /// <param name="lengthSegments">The number of segments the plane has in z.</param>
        /// <param name="widthSegments">The number of segments the plane has in x.</param>
        /// <param name="heightSegments">The number of segments the plane has in y.</param>
        /// <returns></returns>
        public void CreateBoxPrimitive(string name, float length, float width, float height,
            int lengthSegments, int widthSegments, int heightSegments)
        {
            short v = 0;
            short i = 0;
            Vertex[] vertices = new Vertex[24];
            short[] indices = new short[36];
            // Front
            i = BoxAddIndicesForSide(v, i, indices, widthSegments, heightSegments);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            // Back
            i = BoxAddIndicesForSide(v, i, indices, widthSegments, heightSegments);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            // Top
            i = BoxAddIndicesForSide(v, i, indices, widthSegments, lengthSegments);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            // Bottom
            i = BoxAddIndicesForSide(v, i, indices, widthSegments, lengthSegments);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            // Left
            i = BoxAddIndicesForSide(v, i, indices, lengthSegments, heightSegments);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            // Right
            i = BoxAddIndicesForSide(v, i, indices, lengthSegments, heightSegments);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            AddPrimitive(new Primitive(vertices, indices), name);
        }

        private static short BoxAddIndicesForSide(short v, short i, 
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
