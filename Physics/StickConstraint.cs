using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Physics
{
    public class StickConstraint : IConstraint
    {
        private PhysicalParticle particle1;
        private PhysicalParticle particle2;
        private float distance;

        public StickConstraint(PhysicalParticle particle1, PhysicalParticle particle2, float distance)
        {
            this.particle1 = particle1;
            this.particle2 = particle2;
            this.distance = distance;
        }

        public void Satisfy()
        {
            Vector3 deltaVector = particle1.Position - particle2.Position;
            float originalDistance = deltaVector.Length();
            float delta = (distance - originalDistance) / (originalDistance * (particle1.InvMass + particle2.InvMass));
            particle1.Position += delta * particle1.InvMass * deltaVector;
            particle2.Position -= delta * particle2.InvMass * deltaVector;
        }

        public ConstraintPriority Priority
        {
            get { return ConstraintPriority.StickPriority; }
        }

    }
}
