using System;
using Microsoft.DirectX;
using System.Collections.Generic;

namespace Dope.DDXX.Physics
{
    public interface IBody
    {
        void AddConstraint(IConstraint constraint);
        void AddParticle(IPhysicalParticle particle);
        Vector3 Gravity { get; set; }
        void Step();
        List<IPhysicalParticle> Particles { get; }
        void ApplyForce(Vector3 vector3);
    }
}
