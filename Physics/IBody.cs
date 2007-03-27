using System;
using Microsoft.DirectX;

namespace Dope.DDXX.Physics
{
    public interface IBody
    {
        void AddConstraint(IConstraint constraint);
        void AddParticle(IPhysicalParticle particle);
        void SetGravity(Vector3 gravity);
        void Step();
    }
}
