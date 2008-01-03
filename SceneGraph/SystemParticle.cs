using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public abstract class SystemParticle<T> : ISystemParticle<T>
        where T : struct
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

        public abstract void Step(ref T destinationVertex);
    }

}
