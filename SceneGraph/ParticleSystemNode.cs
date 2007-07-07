using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Physics;
using System.Drawing;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class ParticleSystemNode : NodeBase
    {
        private IDevice device;
        private IVertexBuffer vertexBuffer;
        private IEffectHandler effectHandler;
        private List<ISystemParticle> particles;
        private ModelMaterial material;
        static private Random rand = new Random();
        private ISystemParticleSpawner particleSpawner;

        public IEffectHandler EffectHandler
        {
            set { effectHandler = value; }
        }

        public int ActiveParticles
        {
            get { return particles.Count; }
        }

        public ParticleSystemNode(string name)
            : base(name)
        {
            Material dxMaterial = new Material();
            dxMaterial.Ambient = Color.Black;
            material = new ModelMaterial(dxMaterial);
        }

        protected Vector3 RandomPositionInSphere(float radius)
        {
            Vector3 pos;
            do
            {
                pos = new Vector3((float)(((rand.NextDouble() * 2) - 1) * radius),
                                  (float)(((rand.NextDouble() * 2) - 1) * radius),
                                  (float)(((rand.NextDouble() * 2) - 1) * radius));
            } while (pos.Length() > radius);
            return pos;
        }

        public void Initialize(ISystemParticleSpawner spawner, IDevice device,
            IGraphicsFactory graphicsFactory, IEffectFactory effectFactory, ITexture texture)
        {
            particleSpawner = spawner;

            this.device = device;
            particles = new List<ISystemParticle>();

            IEffect effect = effectFactory.CreateFromFile("ParticleSystem.fxo");
            effectHandler = new EffectHandler(effect);

            vertexBuffer = graphicsFactory.CreateVertexBuffer(particleSpawner.VertexType, particleSpawner.MaxNumParticles, device, Usage.WriteOnly | Usage.Dynamic, VertexFormats.None, Pool.Default);

            material.DiffuseTexture = texture;
            effectHandler.Techniques = new EffectHandle[] { EffectHandle.FromString(spawner.GetTechniqueName(texture != null)) };

            for (int i = 0; i < particleSpawner.NumInitialSpawns; i++)
                particles.Add(particleSpawner.Spawn());
        }

        protected override void StepNode()
        {
            particles.RemoveAll(delegate(ISystemParticle particle) { if (particle.IsDead()) return true; else return false; });
            while (particleSpawner.MaxNumParticles != particles.Count && particleSpawner.ShouldSpawn())
                particles.Add(particleSpawner.Spawn());
            using (IGraphicsStream stream = vertexBuffer.Lock(0, 0, LockFlags.Discard))
            {
                foreach (ISystemParticle particle in particles)
                {
                    particle.StepAndWrite(stream);
                }
                vertexBuffer.Unlock();
            }
        }

        protected override void RenderNode(IScene scene)
        {
            if (particles.Count == 0)
                return;
            effectHandler.SetNodeConstants(WorldMatrix, scene.ActiveCamera.ViewMatrix, scene.ActiveCamera.ProjectionMatrix);
            effectHandler.SetMaterialConstants(scene.AmbientColor, material, 0);

            int passes = effectHandler.Effect.Begin(FX.None);

            for (int i = 0; i < passes; i++)
            {
                effectHandler.Effect.BeginPass(i);

                device.RenderState.AlphaBlendEnable = true;
                device.RenderState.BlendOperation = particleSpawner.BlendOperation;
                device.RenderState.SourceBlend = particleSpawner.SourceBlend;
                device.RenderState.DestinationBlend = particleSpawner.DestinationBlend;
                device.SetStreamSource(0, vertexBuffer, 0);
                device.VertexDeclaration = particleSpawner.VertexDeclaration;
                device.DrawPrimitives(PrimitiveType.PointList, 0, ActiveParticles);

                effectHandler.Effect.EndPass();
            }

            effectHandler.Effect.End();
        }
    }
}
