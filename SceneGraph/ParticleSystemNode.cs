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
        private IVertexBuffer vertexBuffer;
        protected IEffectHandler effectHandler;
        protected List<SystemParticle> particles;
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

        protected IVertexBuffer VertexBuffer 
        {
            get { return vertexBuffer; }
        }

        protected abstract VertexDeclaration VertexDeclaration { get; }
        protected abstract Type VertexType { get; }

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

        protected void InitializeBase(int numParticles, IDevice device,
            IGraphicsFactory graphicsFactory, IEffectFactory effectFactory)
        {
            this.numParticles = numParticles;
            this.device = device;
            particles = new List<SystemParticle>();

            IEffect effect = effectFactory.CreateFromFile("ParticleSystem.fxo");
            this.effectHandler = new EffectHandler(effect);

            vertexBuffer = graphicsFactory.CreateVertexBuffer(VertexType, numParticles, device, Usage.WriteOnly | Usage.Dynamic, VertexFormats.None, Pool.Default);
        }

        protected override void RenderNode(IScene scene)
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
                device.SetStreamSource(0, vertexBuffer, 0);
                device.VertexDeclaration = VertexDeclaration;
                device.DrawPrimitives(PrimitiveType.PointList, 0, ActiveParticles);

                effectHandler.Effect.EndPass();
            }

            effectHandler.Effect.End();
        }
    }
}
