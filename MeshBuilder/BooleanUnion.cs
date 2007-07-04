using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.MeshBuilder
{
    public class BooleanUnion : IModifier
    {
        private IModifier a;
        private IModifier b;

        public IModifier A
        {
            get { return a; }
            set { a = value; }
        }

        public IModifier B
        {
            get { return b; }
            set { b = value; }
        }

        public Primitive Generate()
        {
            Primitive aPrimitive = a.Generate();
            Primitive bPrimitive = b.Generate();
            Vertex[] vertices = new Vertex[aPrimitive.Vertices.Length + bPrimitive.Vertices.Length];
            short[] indices = new short[aPrimitive.Indices.Length + bPrimitive.Indices.Length];
            aPrimitive.Vertices.CopyTo(vertices, 0);
            aPrimitive.Indices.CopyTo(indices, 0);
            bPrimitive.Vertices.CopyTo(vertices, aPrimitive.Vertices.Length);
            bPrimitive.Indices.CopyTo(indices, aPrimitive.Indices.Length);
            return new Primitive(vertices, indices);
        }
    }
}
