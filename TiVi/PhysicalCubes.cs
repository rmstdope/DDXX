using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Physics;
using Microsoft.DirectX;
using Dope.DDXX.Graphics;
using System.Drawing;
using Dope.DDXX.SceneGraph;

namespace TiVi
{
    public class PhysicalCubes : BaseDemoEffect
    {
        private class YPosConstraint : IConstraint
        {
            private const float epsilon = 0.001f;
            private float yPos;
            private IPhysicalParticle particle;
            
            public YPosConstraint(float yPos, IPhysicalParticle particle)
            {
                this.particle = particle;
                this.yPos = yPos;
            }

            public ConstraintPriority Priority
            {
                get { return ConstraintPriority.PositionPriority; }
            }

            public void Satisfy()
            {
                if (particle.Position.Y < yPos - epsilon)
                    particle.Position = new Vector3(particle.Position.X, yPos, particle.Position.Z);
            }
        }

        private Body body;
        private ILine line;
        private IScene scene;
        private CameraNode camera;

        public PhysicalCubes(float start, float end)
            : base(start, end)
        {
        }

        protected override void Initialize()
        {
            const float stiffness = 0.7f;
            CreateStandardSceneAndCamera(out scene, out camera, 15);
            line = GraphicsFactory.CreateLine(Device);

            body = new Body();
            body.AddParticle(new PhysicalParticle(new Vector3(-1, 1, 1), 1, 0));
            body.AddParticle(new PhysicalParticle(new Vector3(1, 1, 1), 1, 0));
            body.AddParticle(new PhysicalParticle(new Vector3(1, -1, 1), 1, 0));
            body.AddParticle(new PhysicalParticle(new Vector3(-1, -1, 1), 1, 0));
            body.AddParticle(new PhysicalParticle(new Vector3(-1, 1, -1), 1, 0));
            body.AddParticle(new PhysicalParticle(new Vector3(1, 1, -1), 1, 0));
            body.AddParticle(new PhysicalParticle(new Vector3(1, -1, -1), 1, 0));
            body.AddParticle(new PhysicalParticle(new Vector3(-1, -1, -1), 1, 0));

            body.AddConstraint(new StickConstraint(body.Particles[0], body.Particles[1], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[1], body.Particles[2], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[2], body.Particles[3], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[3], body.Particles[0], 2, stiffness));

            body.AddConstraint(new StickConstraint(body.Particles[4], body.Particles[5], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[5], body.Particles[6], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[6], body.Particles[7], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[7], body.Particles[4], 2, stiffness));

            body.AddConstraint(new StickConstraint(body.Particles[0], body.Particles[4], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[1], body.Particles[5], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[2], body.Particles[6], 2, stiffness));
            body.AddConstraint(new StickConstraint(body.Particles[3], body.Particles[7], 2, stiffness));

            body.AddConstraint(new StickConstraint(body.Particles[0], body.Particles[6], (float)Math.Sqrt(12)));
            body.AddConstraint(new StickConstraint(body.Particles[1], body.Particles[7], (float)Math.Sqrt(12)));
            body.AddConstraint(new StickConstraint(body.Particles[2], body.Particles[4], (float)Math.Sqrt(12)));
            body.AddConstraint(new StickConstraint(body.Particles[3], body.Particles[5], (float)Math.Sqrt(12)));

            foreach (IPhysicalParticle particle in body.Particles)
                body.AddConstraint(new YPosConstraint(-5, particle));

            body.Particles[0].Position += new Vector3(0.02f, 0.02f, 0.02f);
            //body.AddConstraint(new PositionConstraint(body.Particles[0], body.Particles[0].Position));
            body.Gravity = new Vector3(0, -4.0f, 0);
        }

        public override void Step()
        {
            body.Step();
        }

        public override void Render()
        {
            Matrix transform = camera.ViewMatrix * camera.ProjectionMatrix;
            Vector3[] vecs = new Vector3[] { new Vector3(0, 0, 0), new Vector3(10, 0, 0) };
            Vector3[] vertices = new Vector3[body.Particles.Count];
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = body.Particles[i].Position;
            //line.Width = 20;
            //line.Antialias = false;
            line.Begin();
            //line.DrawTransform(vecs, transform, Color.White);
            foreach (IConstraint constraint in body.Constraints)
            {
                if (constraint is StickConstraint)
                {
                    StickConstraint stick = constraint as StickConstraint;
                    line.DrawTransform(new Vector3[] { stick.Particle1.Position, stick.Particle2.Position }, transform, Color.White);
                }
            }
            //line.DrawTransform(new Vector3[] { body.Particles[0].Position, body.Particles[1].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[1].Position, body.Particles[2].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[2].Position, body.Particles[3].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[3].Position, body.Particles[0].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[4].Position, body.Particles[5].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[5].Position, body.Particles[6].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[6].Position, body.Particles[7].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[7].Position, body.Particles[4].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[0].Position, body.Particles[4].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[1].Position, body.Particles[5].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[2].Position, body.Particles[6].Position }, transform, Color.White);
            //line.DrawTransform(new Vector3[] { body.Particles[3].Position, body.Particles[7].Position }, transform, Color.White);
            line.End();
        }
    }
}
