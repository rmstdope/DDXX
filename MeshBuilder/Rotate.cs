using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class Rotate : IModifier
    {
        private IModifier input;
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

        public IModifier Input
        {
            get { return input; }
            set { input = value; }
        }

        public Primitive Generate()
        {
            Primitive primitive = input.Generate();
            Matrix rotation = Matrix.RotationYawPitchRoll(y, x, z);
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                Vector3 position = primitive.Vertices[i].Position;
                Vector3 normal = primitive.Vertices[i].Normal;
                position.TransformCoordinate(rotation);
                normal.TransformNormal(rotation);
                primitive.Vertices[i].Position = position;
                primitive.Vertices[i].Normal = normal;
            }
            return primitive;
        }
    }
}
