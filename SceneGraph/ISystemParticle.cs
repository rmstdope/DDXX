using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public interface ISystemParticle
    {
        void StepAndWrite(IGraphicsStream stream);
    }
}
