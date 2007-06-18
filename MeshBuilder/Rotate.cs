using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class Rotate : IPrimitive
    {
        private IPrimitive input;
        private float y;
        private float x;
        private float z;

        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        public float X
        {
            get { return x; }
            set { x = value; }
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
            Matrix rotation = Matrix.RotationYawPitchRoll(y, x, z);
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 position = vertices[i].Position;
                Vector3 normal = vertices[i].Normal;
                position.TransformCoordinate(rotation);
                normal.TransformNormal(rotation);
                vertices[i].Position = position;
                vertices[i].Normal = normal;
            }
        }
    }
}
