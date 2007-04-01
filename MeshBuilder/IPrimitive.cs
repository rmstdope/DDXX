using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.MeshBuilder
{
    public interface IPrimitive
    {
        Vertex[] Vertices { get; }
        short[] Indices { get; }
        IBody Body { get; set; }
        ModelMaterial ModelMaterial { get; set; }
        IModel CreateModel(IGraphicsFactory factory, IDevice device);
    }
}
