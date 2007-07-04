using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class Scale : IModifier
    {
        private IModifier input;
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

        public IModifier Input
        {
            get { return input; }
            set { input = value; }
        }

        public Primitive Generate()
        {
            Primitive primitive = input.Generate();
            // TODO: This should affect normals as well
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                primitive.Vertices[i].Position = new Vector3(primitive.Vertices[i].Position.X * x,
                    primitive.Vertices[i].Position.Y * y, primitive.Vertices[i].Position.Z * z);
            }
            return primitive;
        }
    }
}
