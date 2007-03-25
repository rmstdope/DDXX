using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.MeshBuilder
{
    public interface IPrimitive
    {
        Vertex[] Vertices { get; }
        short[] Indices { get; }
        IMesh CreateMesh(IGraphicsFactory factory, IDevice device);
    }
}
