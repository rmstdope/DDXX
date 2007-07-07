using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public interface ISystemParticle
    {
        bool IsDead();
        void StepAndWrite(IGraphicsStream stream);
    }
}
