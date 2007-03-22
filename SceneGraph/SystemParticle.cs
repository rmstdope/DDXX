using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.SceneGraph
{
    public class SystemParticle
    {
        public Vector3 Position;
        public Color Color;
        public float Size;

        public SystemParticle(Vector3 position, Color color, float size)
        {
            this.Position = position;
            this.Color = color;
            this.Size = size;
        }

    }
}
