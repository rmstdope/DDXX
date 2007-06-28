using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class UvMapSphere : IPrimitive
    {
        private IPrimitive input;

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
                Vector3 normal = vertices[i].Position;
                normal.Normalize();
                vertices[i].U = (float)(Math.Asin(normal.X) / Math.PI) + 0.5f;
                vertices[i].V = (float)(Math.Asin(normal.Y) / Math.PI) + 0.5f;
            }
        }
    }
}
