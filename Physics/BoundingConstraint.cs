using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Physics
{
    public class BoundingConstraint : IConstraint
    {
        private IPhysicalParticle particle;
        private IBoundingObject boundingObject;

        public BoundingConstraint(IPhysicalParticle particle, IBoundingObject boundingObject)
        {
            this.particle = particle;
            this.boundingObject = boundingObject;
        }

        public ConstraintPriority Priority
        {
            get { return ConstraintPriority.PositionPriority; }
        }

        public void Satisfy()
        {
            particle.Position = boundingObject.ConstrainOutside(particle.Position);
        }
    }
}
