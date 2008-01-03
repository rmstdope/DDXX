using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Physics
{
    public class PositionConstraint : Dope.DDXX.Physics.IConstraint
    {
        private IPhysicalParticle particle;
        private Vector3 position;

        public PositionConstraint(IPhysicalParticle particle, Vector3 position)
        {
            this.particle = particle;
            this.position = position;
        }

        public void Satisfy()
        {
            particle.Position = position;
        }

        public ConstraintPriority Priority 
        {
            get
            {
                return ConstraintPriority.PositionPriority;
            }
        }
    }
}
