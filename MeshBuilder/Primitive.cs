using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    public class Primitive
    {
        public Vertex[] Vertices;
        public short[] Indices;
        public IBody Body;
        public Primitive(Vertex[] vertices, short[] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }
    }
}
