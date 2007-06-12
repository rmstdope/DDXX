using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    public interface IPrimitive
    {
        void Generate(out Vertex[] vertices, out short[] indices, out IBody body);
    }
}
