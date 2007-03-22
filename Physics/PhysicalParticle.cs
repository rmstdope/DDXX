using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Physics
{
    public class PhysicalParticle
    {
        private Vector3 position;
        private Vector3 oldPosition;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startPosition">The starting position of the particle.</param>
        /// <param name="invMass">1/m (mass of the particle)</param>
        /// <param name="dragCoefficient">Drag coefficient of the particle.</param>
        public PhysicalParticle(Vector3 startPosition, float invMass, float dragCoefficient)
        {
            this.position = startPosition;
        }

        public Vector3 Position
        {
            get { return position; }
            //set { position = value; }
        }


        public void Step(Vector3 gravity)
        {
            float dt = Time.DeltaTime;

            // Calculate velocity
            Vector3 lastVelocity = (position - oldPosition) * (1 / dt);

            // Get acceleration
            Vector3 acceleration = gravity;

            // Get velocity change
            Vector3 velocityMod = acceleration * dt;

            // Move object
            oldPosition = position;
            position += (lastVelocity +  velocityMod * 0.5f) * dt;
        }
    }
}
