using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.ModelBuilder
{
    public class BooleanUnion : ModifierBase
    {
        public BooleanUnion()
            : base(2)
        {
        }

        public override IPrimitive Generate()
        {
            IPrimitive aPrimitive = GetInput(0);
            IPrimitive bPrimitive = GetInput(1);
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
