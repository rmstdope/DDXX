using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Physics
{
    public class Body
    {
        private const int NUM_ITERATIONS = 4;
        private Vector3 gravity = new Vector3(0, 0, 0);
        private List<IPhysicalParticle> particles = new List<IPhysicalParticle>();
        private List<IConstraint> constraints = new List<IConstraint>();

        public void SetGravity(Vector3 gravity)
        {
            this.gravity = gravity;
        }

        public void AddParticle(IPhysicalParticle particle)
        {
            particles.Add(particle);
        }

        public void AddConstraint(IConstraint constraint)
        {
            constraints.Add(constraint);
            constraints.Sort(delegate(IConstraint c1, IConstraint c2) 
            {
                if (c1.Priority > c2.Priority)
                    return -1;
                else if (c2.Priority > c1.Priority)
                    return 1;
                return 0;
            });
        }

        public void Step()
        {
            foreach (IPhysicalParticle particle in particles)
                particle.Step(gravity);
            for (int i = 0; i < NUM_ITERATIONS; i++)
                foreach (IConstraint constraint in constraints)
                    constraint.Satisfy();
        }
    }
}