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
        private ISystemParticleSpawner particleSpawner;

        private VertexColorPoint[] vertexData;

        public FallingStarSystem(string name)
            : base(name)
        {
        }

        public void Initialize(ISystemParticleSpawner spawner, IDevice device, IGraphicsFactory graphicsFactory, 
            IEffectFactory effectFactory, ITexture texture)
        {
            particleSpawner = spawner;

            base.InitializeBase(spawner.MaxNumParticles, device, graphicsFactory, effectFactory, texture);

            vertexData = new VertexColorPoint[NumParticles];

            for (int i = 0; i < particleSpawner.NumInitialSpawns; i++)
                particles.Add(particleSpawner.Spawn());
        }

        protected override Type VertexType
        {
            get { return particleSpawner.VertexType; }
        }

        protected override VertexDeclaration VertexDeclaration
        {
            get { return particleSpawner.VertexDeclaration; }
        }

        protected override void StepNode()
        {
            foreach (FallingStarParticle particle in particles)
            {
                particle.Position += particle.Velocity * Time.DeltaTime;
                if (particle.Position.Length() > 100.0)
                    particle.Kill();
            }
            particles.RemoveAll(delegate(SystemParticle particle) 
                { return !particle.Alive; });
            while (ActiveParticles < NumParticles)
                particles.Add(particleSpawner.Spawn());
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
