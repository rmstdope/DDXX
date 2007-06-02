using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.ParticleSystems
{
    public class FloaterSystem : ParticleSystemNode
    {
        private ISystemParticleSpawner particleSpawner;

        protected override Type VertexType
        {
            get { return particleSpawner.VertexType; }
        }

        protected override VertexDeclaration VertexDeclaration
        {
            get { return particleSpawner.VertexDeclaration; }
        }

        public FloaterSystem(string name)
            : base(name)
        {
        }

        public void Initialize(ISystemParticleSpawner spawner, ITexture texture)
        {
            particleSpawner = spawner;

            InitializeBase(particleSpawner.MaxNumParticles, D3DDriver.GetInstance().Device, 
                D3DDriver.GraphicsFactory, D3DDriver.EffectFactory, texture);

            for (int i = 0; i < particleSpawner.NumInitialSpawns; i++)
                particles.Add(particleSpawner.Spawn());
        }

        protected override void StepNode()
        {
            using (IGraphicsStream stream = VertexBuffer.Lock(0, 0, LockFlags.Discard))
            {
                foreach (SystemParticle particle in particles)
                {
                    particle.StepAndWrite(stream);
                }
                VertexBuffer.Unlock();
            }
        }

    }
}
