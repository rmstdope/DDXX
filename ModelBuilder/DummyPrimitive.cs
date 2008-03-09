using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.ModelBuilder
{
    public class DummyPrimitive : ModifierBase
    {
        private Vertex[] vertices;
        private short[] indices;

        public DummyPrimitive(Vertex[] vertices, short[] indices)
            : base(0)
        {
            this.vertices = vertices;
            this.indices = indices;
        }

        public override IPrimitive Generate()
        {
            return new Primitive(vertices, indices);
        }
    }
}
