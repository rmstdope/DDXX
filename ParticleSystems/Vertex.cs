using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.ParticleSystems
{
    public struct VertexColorPoint
    {
        public VertexColorPoint(Vector3 position, float size, int color)
        {
            Position = position;
            Size = size;
            Color = color;
        }
        public Vector3 Position;
        public float Size;
        public int Color;

    }
}
