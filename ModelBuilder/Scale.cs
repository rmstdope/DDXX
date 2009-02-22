using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class Scale : ModifierBase
    {
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

        public Scale()
            : base(1)
        {
            x = 1;
            y = 1;
            z = 1;
        }

        public override IPrimitive Generate()
        {
            IPrimitive primitive = GetInput(0);
            // TODO: This should affect normals as well
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                primitive.Vertices[i].Position = new Vector3(primitive.Vertices[i].Position.X * x, primitive.Vertices[i].Position.Y * y, primitive.Vertices[i].Position.Z * z);
            }
            ComputeNormals(primitive);
            return primitive;
        }
    }
}
