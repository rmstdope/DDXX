using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;

namespace Dope.DDXX.ModelBuilder
{
    public interface IPrimitive
    {
        Vertex[] Vertices { get; set; }
        short[] Indices { get; set; }
        IBody Body { get; set; }
        void Calculate();
    }
}
