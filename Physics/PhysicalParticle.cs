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
        private Vector3 lastVelocity;
        private float lastDeltaTime;
        private Vector3 externalForces;
        private float invMass;
        private float dragCoefficient;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mass">mass of the particle</param>
        /// <param name="dragCoefficient">Drag coefficient of the particle.</param>
        public PhysicalParticle(float mass, float dragCoefficient)
            : this(new Vector3(0, 0, 0), mass, dragCoefficient)
        {
            // Only contains call to other constructor
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startPosition">The starting position of the particle.</param>
        /// <param name="mass">mass of the particle</param>
        /// <param name="dragCoefficient">Drag coefficient of the particle.</param>
        public PhysicalParticle(Vector3 startPosition, float mass, float dragCoefficient)
        {
            this.position = startPosition;
            this.oldPosition = startPosition;
            this.invMass = 1 / mass;
            this.lastVelocity = new Vector3(0, 0, 0);
            this.dragCoefficient = dragCoefficient;
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
            UpdatePosition(GetVelocity(), GetVelocityChange(gravity));
            UpdateLastDelta();
            externalForces = new Vector3(0, 0, 0);
        }

        private void UpdateLastDelta()
        {
            lastDeltaTime = Time.DeltaTime;
        }

        private void UpdatePosition(Vector3 velocity, Vector3 velocityMod)
        {
            oldPosition = position;
            position += (velocity + velocityMod * 0.5f) * Time.DeltaTime;
            this.lastVelocity = velocity;
        }

        private Vector3 GetVelocityChange(Vector3 gravity)
        {
            return GetAcceleration(gravity) * Time.DeltaTime;
        }

        private Vector3 GetAcceleration(Vector3 gravity)
        {
            return gravity + externalForces * invMass;
        }

        private Vector3 GetVelocity()
        {
            // Calculate the average velocity last update
            Vector3 velocity = (position - oldPosition) * (1 / lastDeltaTime);
            // Add drag coefficient
            return velocity * (1 - dragCoefficient);
        }

        public void ApplyForce(Vector3 force)
        {
            externalForces += force;
        }
    }
}
