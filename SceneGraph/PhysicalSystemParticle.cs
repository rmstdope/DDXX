using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public class PhysicalSystemParticle<T> : SystemParticle<T>
        where T : struct
    {
        private PhysicalParticle physics;
        private Vector3 gravity;

        protected Vector3 PhysicalPosition { get { return physics.Position; } }

        public PhysicalSystemParticle(Vector3 position, float mass, float dragCoefficient, Vector3 gravity, Color color, float size)
            : base(position, color, size)
        {
            this.gravity = gravity;
            physics = new PhysicalParticle(position, mass, dragCoefficient);
        }

        public override void Step(ref T destinationVertex)
        {
            if (Time.DeltaTime > 0.0f)
                physics.Step(gravity);
        }

        protected void ApplyForce(Vector3 force)
        {
            physics.ApplyForce(force);
        }

    }
}
