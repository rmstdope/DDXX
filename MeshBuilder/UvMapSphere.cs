using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class UvMapSphere : IModifier
    {
        private IModifier input;

        public IModifier Input 
        { 
            get { return input; } 
            set { input = value; }
        }

        public Primitive Generate()
        {
            Primitive primitive = input.Generate();
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                Vector3 normal = primitive.Vertices[i].Position;
                normal.Normalize();
                primitive.Vertices[i].U = (float)(Math.Asin(normal.X) / Math.PI) + 0.5f;
                primitive.Vertices[i].V = (float)(Math.Asin(normal.Y) / Math.PI) + 0.5f;
            }
            return primitive;
        }
    }
}
