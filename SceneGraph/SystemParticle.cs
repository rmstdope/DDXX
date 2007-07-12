using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public abstract class SystemParticle : ISystemParticle
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

        public virtual bool IsDead()
        {
            return false;
        }

        public abstract void StepAndWrite(IGraphicsStream stream, IRenderableCamera camera);
    }
}
