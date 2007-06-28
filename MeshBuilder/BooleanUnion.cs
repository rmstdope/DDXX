using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.MeshBuilder
{
    public class BooleanUnion : IPrimitive
    {
        private IPrimitive a;
        private IPrimitive b;

        public IPrimitive A
        {
            get { return a; }
            set { a = value; }
        }

        public IPrimitive B
        {
            get { return b; }
            set { b = value; }
        }

        public void Generate(out Vertex[] vertices, out short[] indices, out Dope.DDXX.Physics.IBody body)
        {
            Dope.DDXX.Physics.IBody unused;
            Vertex[] aVertices;
            short[] aIndices;
            a.Generate(out aVertices, out aIndices, out unused);
            Vertex[] bVertices;
            short[] bIndices;
            b.Generate(out bVertices, out bIndices, out unused);
            vertices = new Vertex[aVertices.Length + bVertices.Length];
            indices = new short[aIndices.Length + bIndices.Length];
            aVertices.CopyTo(vertices, 0);
            aIndices.CopyTo(indices, 0);
            bVertices.CopyTo(vertices, aVertices.Length);
            bIndices.CopyTo(indices, aIndices.Length);
            body = null;
        }
    }
}
