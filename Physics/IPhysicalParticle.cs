using System;
using Microsoft.DirectX;
namespace Dope.DDXX.Physics
{
    public interface IPhysicalParticle
    {
        float InvMass { get; set; }
        float DragCoefficient { get; set; }
        Vector3 Position { get; set; }
        Vector3 OldPosition { get; set; }
        void Step(Vector3 gravity);
        void ApplyForce(Vector3 force);
        void Reset();
    }
}
