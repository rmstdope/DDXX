using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.Utility;

namespace Dope.DDXX.ParticleSystems
{
    public class FallingStarSystem : ParticleSystemNode
    {
        private VertexDeclaration vertexDeclaration;
        private VertexColorPoint[] vertexData;

        public class FallingStarParticle : SystemParticle
        {
            private bool alive;
            public Vector3 Velocity;
            public FallingStarParticle(Vector3 position, Color color, float size)
                : base(position, color, size)
            {
                alive = true;
                Velocity = new Vector3((float)rand.NextDouble() * 0.3f, -1, 
                    (float)rand.NextDouble() * 0.3f);
                Velocity.Normalize();
            }

            public void Kill()
            {
                alive = false;
            }

            public override bool Alive
            {
                get { return alive; }
            }
        }

        public FallingStarSystem(string name)
            : base(name)
        {
        }

        public void Initialize(int numParticles, IDevice device, IGraphicsFactory graphicsFactory, 
            IEffectFactory effectFactory, ITexture texture)
        {
            base.InitializeBase(numParticles, device, graphicsFactory, effectFactory, texture);

            CreateVertexDeclaration(graphicsFactory);
            vertexData = new VertexColorPoint[NumParticles];

            while (ActiveParticles < NumParticles)
                SpawnParticle();
        }

        private void CreateVertexDeclaration(IGraphicsFactory graphicsFactory)
        {
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
                new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd 
            };
            vertexDeclaration = graphicsFactory.CreateVertexDeclaration(Device, elements);
        }

        private void SpawnParticle()
        {
            FallingStarParticle particle = new FallingStarParticle(RandomPositionInSphere(2.0f), Color.White, (float)rand.NextDouble() * 0.2f);
            particles.Add(particle);
        }

        protected override Type VertexType
        {
            get { return typeof(VertexColorPoint); }
        }

        protected override VertexDeclaration VertexDeclaration
        {
            get { return vertexDeclaration; }
        }

        protected override void StepNode()
        {
            foreach (FallingStarParticle particle in particles)
            {
                particle.Position += particle.Velocity * Time.DeltaTime;
                if (particle.Position.Length() > 2.0)
                    particle.Kill();
            }
            particles.RemoveAll(delegate(SystemParticle particle) 
                { return !particle.Alive; });
            while (ActiveParticles < NumParticles)
                SpawnParticle();
            int i = 0;
            foreach (FallingStarParticle particle in particles)
            {
                vertexData[i].Position = particle.Position;
                vertexData[i].Size = particle.Size;
                vertexData[i].Color = particle.Color.ToArgb();
                i++;
            }
            VertexBuffer.SetData(vertexData, 0, LockFlags.Discard);
        }

    }
}
