using System;
namespace Dope.DDXX.Physics
{
    public interface IPhysicalParticle
    {
        float InvMass { get; set; }
        Microsoft.DirectX.Vector3 Position { get; set; }
        void Step(Microsoft.DirectX.Vector3 gravity);
    }
}
