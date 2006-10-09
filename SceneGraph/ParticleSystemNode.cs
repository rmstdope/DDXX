using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Physics;

namespace Dope.DDXX.SceneGraph
{
    public abstract class ParticleSystemNode : NodeBase
    {
        private int numParticles;
        private IDevice device;
        private IEffectHandler effectHandler;
        protected List<Particle> particles;

        public IDevice Device
        {
            get { return device; }
        }

        public int NumParticles
        {
            get { return numParticles; }
        }

        public int ActiveParticles
        {
            get { return particles.Count; }
        }

        public IEffectHandler EffectHandler
        {
            set { effectHandler = value; }
        }

        protected abstract IVertexBuffer VertexBuffer { get; }
        protected abstract VertexDeclaration VertexDeclaration { get; }

        public ParticleSystemNode(string name)
            : base(name)
        {
        }

        protected void InitializeBase(int numParticles)
        {
            this.numParticles = numParticles;
            this.device = D3DDriver.GetInstance().GetDevice();
            particles = new List<Particle>();

            IEffect effect = D3DDriver.EffectFactory.CreateFromFile("ParticleSystem.fxo");
            this.effectHandler = new EffectHandler(effect);
        }

        protected override void RenderNode(IRenderableScene scene)
        {
            effectHandler.SetNodeConstants(scene, this);

            int passes = effectHandler.Effect.Begin(FX.None);

            for (int i = 0; i < passes; i++)
            {
                effectHandler.Effect.BeginPass(i);

                device.SetStreamSource(0, VertexBuffer, 0);
                device.VertexDeclaration = VertexDeclaration;
                device.DrawPrimitives(PrimitiveType.PointList, 0, ActiveParticles);

                effectHandler.Effect.EndPass();
            }

            effectHandler.Effect.End();
        }
    }
}
