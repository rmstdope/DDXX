using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class Translate : IModifier
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

        #region IModifier Members

        public Primitive Generate()
        {
            Primitive primitive = input.Generate();
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                primitive.Vertices[i].Position = primitive.Vertices[i].Position + new Vector3(x, y, z);
            }
            return primitive;
        }

        #endregion
    }
}
