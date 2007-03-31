using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    public interface IPrimitive
    {
        Vertex[] Vertices { get; }
        short[] Indices { get; }
        IBody Body { get; } 
        IModel CreateModel(IGraphicsFactory factory, ITextureFactory textureFactory, IDevice device);
    }
}
