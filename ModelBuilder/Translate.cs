using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class Translate : ModifierBase
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

        public Translate()
            : base(1)
        {
        }

        public override IPrimitive Generate()
        {
            IPrimitive primitive = GetInput(0);
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                primitive.Vertices[i].Position = primitive.Vertices[i].Position + new Vector3(x, y, z);
            }
            return primitive;
        }
    }
}
