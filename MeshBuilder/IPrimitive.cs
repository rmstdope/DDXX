using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace MeshBuilder
{
    public interface IPrimitive
    {
        Vertex[] Vertices { get; }
        int[] Indices { get; }
        IMesh CreateMesh(IGraphicsFactory factory);
    }
}
