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

        public ISystemParticle Spawn()
        {
            return new FallingStarParticle(100.0f);
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
        private float radius;
        public Vector3 Velocity;
        public FallingStarParticle(float radius)
            : base(FallingStarParticle.RandomPositionInSphere(radius), Color.Red, Rand.Float(5) + 5)
        {
            this.radius = radius;
            Velocity = new Vector3(Rand.Float(0, 0.3), -1, Rand.Float(0, 0.3));
            Velocity.Normalize();
            Velocity *= 50;
        }

        private static Vector3 RandomPositionInSphere(float radius)
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

        private void Respawn()
        {
            Position = FallingStarParticle.RandomPositionInSphere(radius);
            Color = Color.Red;
            Size = Rand.Float(5) + 5;
            Velocity = new Vector3(Rand.Float(0, 0.3), -1, Rand.Float(0, 0.3));
            Velocity.Normalize();
            Velocity *= 50;
        }

        public override void StepAndWrite(IGraphicsStream stream)
        {
            VertexColorPoint vertex;

            Position += Velocity * Time.DeltaTime;
            if (Position.Length() > radius)
                Respawn();
            vertex.Position = Position;
            vertex.Size = Size;
            vertex.Color = Color.ToArgb();
            stream.Write(vertex);
        }
    }
}
