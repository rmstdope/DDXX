using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Physics
{
    public class PhysicalParticle : Dope.DDXX.Physics.IPhysicalParticle
    {
        private Vector3 position;
        private Vector3 oldPosition;
        private float lastDeltaTime;
        private Vector3 externalForces;
        private float invMass;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="invMass">1/m (mass of the particle)</param>
        /// <param name="dragCoefficient">Drag coefficient of the particle.</param>
        public PhysicalParticle(float invMass, float dragCoefficient)
        {
            this.invMass = invMass;
            lastDeltaTime = 1.0f;
            externalForces = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startPosition">The starting position of the particle.</param>
        /// <param name="invMass">1/m (mass of the particle)</param>
        /// <param name="dragCoefficient">Drag coefficient of the particle.</param>
        public PhysicalParticle(Vector3 startPosition, float invMass, float dragCoefficient)
        {
            this.position = startPosition;
            this.oldPosition = startPosition;
            this.invMass = invMass;
            lastDeltaTime = 1.0f;
            externalForces = new Vector3(0, 0, 0);
        }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float InvMass
        {
            get { return invMass; }
            set { invMass = value; }
        }

        public void Step(Vector3 gravity)
        {
            float deltaTime = Time.DeltaTime;

            // Calculate velocity from last update
            Vector3 lastVelocity = (position - oldPosition) * (1 / lastDeltaTime);

            // Get acceleration
            Vector3 acceleration = gravity + externalForces * invMass;

            // Get velocity change
            Vector3 velocityMod = acceleration * deltaTime;

            // Move object
            oldPosition = position;
            position += (lastVelocity +  velocityMod * 0.5f) * deltaTime;
            lastDeltaTime = deltaTime;
            externalForces = new Vector3(0, 0, 0);
        }

        internal void ApplyForce(Vector3 force)
        {
            externalForces += force;
        }
    }
}
