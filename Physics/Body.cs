using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Physics
{
    public class Body : Dope.DDXX.Physics.IBody
    {
        private const int NUM_ITERATIONS = 3;
        private Vector3 gravity = new Vector3(0, 0, 0);
        private List<IPhysicalParticle> particles = new List<IPhysicalParticle>();
        private List<IConstraint> constraints = new List<IConstraint>();

        public List<IPhysicalParticle> Particles
        {
            get { return particles; }
        }

        public Vector3 Gravity
        {
            get { return gravity; }
            set { gravity = value; }
        }

        public Body()
        {
            Gravity = new Vector3(0, -9.82f, 0);
        }

        public void AddParticle(IPhysicalParticle particle)
        {
            particles.Add(particle);
        }

        public void AddConstraint(IConstraint constraint)
        {
            constraints.Add(constraint);
            SortConstraintsByPriority();
        }

        private void SortConstraintsByPriority()
        {
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
            StepParticles();
            StepConstraints();
        }

        private void StepConstraints()
        {
            for (int i = 0; i < NUM_ITERATIONS; i++)
                foreach (IConstraint constraint in constraints)
                    constraint.Satisfy();

        }

        private void StepParticles()
        {
            foreach (IPhysicalParticle particle in particles)
                particle.Step(gravity);
        }

        public void ApplyForce(Vector3 force)
        {
            foreach (IPhysicalParticle particle in particles)
                particle.ApplyForce(force);
        }
    }
}
