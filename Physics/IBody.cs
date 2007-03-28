using System;
using Microsoft.DirectX;
using System.Collections.Generic;

namespace Dope.DDXX.Physics
{
    public interface IBody
    {
        void AddConstraint(IConstraint constraint);
        void AddParticle(IPhysicalParticle particle);
        void SetGravity(Vector3 gravity);
        void Step();
        List<IPhysicalParticle> Particles { get; }
    }
}
