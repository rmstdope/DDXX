using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class Translate : IPrimitive
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
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Position = vertices[i].Position + new Vector3(x, y, z);
            }
        }
    }
}
