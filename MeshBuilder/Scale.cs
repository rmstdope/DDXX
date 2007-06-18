using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class Scale : IPrimitive
    {
        private IPrimitive input;
        private float x;
        private float y;
        private float z;

        public float X
        {
            get { return x; }
            set { x = value; }
        }

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float Z
        {
            get { return z; }
            set { z = value; }
        }

        public IPrimitive Input
        {
            get { return input; }
            set { input = value; }
        }

        public void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
        {
            input.Generate(out vertices, out indices, out body);
            // TODO: This should affect normals as well
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position = new Vector3(vertices[i].Position.X * x, vertices[i].Position.Y * y, vertices[i].Position.Z * z);
            }
        }
    }
}
