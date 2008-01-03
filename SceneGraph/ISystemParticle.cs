using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public interface ISystemParticle<T>
        where T : struct
    {
        bool IsDead();
        void Step(ref T destinationVertex);
    }
}
