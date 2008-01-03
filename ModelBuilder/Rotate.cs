using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class Rotate : ModifierBase
    {
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

        public Rotate()
            : base(1)
        {
        }

        public override IPrimitive Generate()
        {
            IPrimitive primitive = GetInput(0);
            Matrix rotation = Matrix.CreateFromYawPitchRoll(y, x, z);
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                Vector3 position = primitive.Vertices[i].Position;
                Vector3 normal = primitive.Vertices[i].Normal;
                position = Vector3.Transform(position, rotation);
                normal = Vector3.TransformNormal(normal, rotation);
                primitive.Vertices[i].Position = position;
                primitive.Vertices[i].Normal = normal;
            }
            return primitive;
        }
    }
}
