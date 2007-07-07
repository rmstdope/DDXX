using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Physics
{
    public class StickConstraint : IConstraint
    {
        private const float epsilon = 0.000f;
        private IPhysicalParticle particle1;
        private IPhysicalParticle particle2;
        private float distance;
        private float stiffness;

        public IPhysicalParticle Particle1
        {
            get { return particle1; }
            set { particle1 = value; }
        }

        public IPhysicalParticle Particle2
        {
            get { return particle2; }
            set { particle2 = value; }
        }

        public StickConstraint(IPhysicalParticle particle1, IPhysicalParticle particle2, float distance)
            : this(particle1, particle2, distance, 1.0f)
        {
        }

        public StickConstraint(IPhysicalParticle particle1, IPhysicalParticle particle2, float distance, float stiffness)
        {
            this.particle1 = particle1;
            this.particle2 = particle2;
            this.distance = distance;
            this.stiffness = stiffness;
        }

        public void Satisfy()
        {
            Vector3 deltaVector = GetVectorBetweenParticles();
            float originalDistance = deltaVector.Length();
            float deltaDistance = (distance - originalDistance);
            if (deltaDistance > epsilon || deltaDistance < -epsilon)
            {
                float delta = deltaDistance / (originalDistance * (particle1.InvMass + particle2.InvMass));
                particle1.Position += delta * particle1.InvMass * deltaVector * stiffness;
                particle2.Position -= delta * particle2.InvMass * deltaVector * stiffness;
            }
        }

        private Vector3 GetVectorBetweenParticles()
        {
            return particle1.Position - particle2.Position;
        }

        public ConstraintPriority Priority
        {
            get { return ConstraintPriority.StickPriority; }
        }

    }
}
