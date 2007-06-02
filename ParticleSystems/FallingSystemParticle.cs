using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.ParticleSystems
{
    public class FallingStarParticleSpawner : ISystemParticleSpawner
    {
        private VertexDeclaration vertexDeclaration;
        private int maxNumParticles;

        public FallingStarParticleSpawner(IGraphicsFactory graphicsFactory, IDevice device, int maxNumParticles)
        {
            this.maxNumParticles = maxNumParticles;
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
                new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd 
            };
            vertexDeclaration = graphicsFactory.CreateVertexDeclaration(device, elements);
        }

        public SystemParticle Spawn()
        {
            return new FallingStarParticle(RandomPositionInSphere(100.0f), Color.Red, Rand.Float(5) + 5);
        }

        protected Vector3 RandomPositionInSphere(float radius)
        {
            Vector3 pos;
            do
            {
                pos = new Vector3(Rand.Float(-radius, radius),
                    Rand.Float(-radius, radius),
                    Rand.Float(-radius, radius));
            } while (pos.Length() > radius);
            return pos;
        }

        public Type VertexType
        {
            get { return typeof(VertexColorPoint); }
        }

        public VertexDeclaration VertexDeclaration
        {
            get { return vertexDeclaration; }
        }

        public int NumInitialSpawns
        {
            get { return maxNumParticles; }
        }

        public int MaxNumParticles
        {
            get { return maxNumParticles; }
        }
    }

    public class FallingStarParticle : SystemParticle
    {
        private bool alive;
        public Vector3 Velocity;
        public FallingStarParticle(Vector3 position, Color color, float size)
            : base(position, color, size)
        {
            alive = true;
            Velocity = new Vector3(Rand.Float(0, 0.3), -1, Rand.Float(0, 0.3));
            Velocity.Normalize();
            Velocity *= 50;
        }

        public void Kill()
        {
            alive = false;
        }

        public override bool Alive
        {
            get { return alive; }
        }

        public override void StepAndWrite(IGraphicsStream stream)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
