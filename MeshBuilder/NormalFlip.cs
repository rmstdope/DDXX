using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.MeshBuilder
{
    public class NormalFlip : IModifier
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
                Vertex vertex = primitive.Vertices[i];
                vertex.Normal = -vertex.Normal;
                primitive.Vertices[i] = vertex;
            }
            for (int i = 0; i < primitive.Indices.Length; i += 3)
            {
                short i2 = primitive.Indices[i+ 1];
                short i3 = primitive.Indices[i + 2];
                primitive.Indices[i + 1] = i3;
                primitive.Indices[i + 2] = i2;
            }
            return primitive;
        }
    }
}
