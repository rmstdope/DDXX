using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Physics;
using System.Drawing;

namespace Dope.DDXX.SceneGraph
{
    public abstract class ParticleSystemNode : NodeBase
    {
        private int numParticles;
        private IDevice device;
        protected IEffectHandler effectHandler;
        protected List<Particle> particles;
        protected ModelMaterial material;
        protected BlendOperation blendOperation;
        protected Blend sourceBlend;
        protected Blend destinationBlend;

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
            Material dxMaterial = new Material();
            dxMaterial.Ambient = Color.White;
            material = new ModelMaterial(dxMaterial);
            blendOperation = BlendOperation.Add;
            sourceBlend = Blend.One;
            destinationBlend = Blend.One;
        }

        protected void InitializeBase(int numParticles)
        {
            this.numParticles = numParticles;
            this.device = D3DDriver.GetInstance().Device;
            particles = new List<Particle>();

            IEffect effect = D3DDriver.EffectFactory.CreateFromFile("ParticleSystem.fxo");
            this.effectHandler = new EffectHandler(effect, "", null);
        }

        protected override void RenderNode(IRenderableScene scene)
        {
            effectHandler.SetNodeConstants(WorldMatrix, scene.ActiveCamera.ViewMatrix, scene.ActiveCamera.ProjectionMatrix);
            effectHandler.SetMaterialConstants(scene.AmbientColor, material, 0);

            int passes = effectHandler.Effect.Begin(FX.None);

            for (int i = 0; i < passes; i++)
            {
                effectHandler.Effect.BeginPass(i);

                device.RenderState.AlphaBlendEnable = true;
                device.RenderState.BlendOperation = blendOperation;
                device.RenderState.SourceBlend = sourceBlend;
                device.RenderState.DestinationBlend = destinationBlend;
                device.SetStreamSource(0, VertexBuffer, 0);
                device.VertexDeclaration = VertexDeclaration;
                device.DrawPrimitives(PrimitiveType.PointList, 0, ActiveParticles);

                effectHandler.Effect.EndPass();
            }

            effectHandler.Effect.End();
        }
    }
}
