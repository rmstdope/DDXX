using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    public abstract class ParticleSystemNode<T> : NodeBase
        where T : struct
    {
        protected T[] vertices;
        private IGraphicsDevice device;
        protected IVertexBuffer vertexBuffer;
        protected List<ISystemParticle<T>> particles;

        protected abstract IMaterialHandler CreateDefaultMaterial(IGraphicsFactory graphicsFactory);
        protected abstract int NumInitialSpawns { get; }
        protected abstract ISystemParticle<T> Spawn();
        protected abstract bool ShouldSpawn();
        protected int maxNumParticles;
        private IMaterialHandler material;
        protected abstract int VertexSizeInBytes { get; }
        protected abstract VertexElement[] VertexElements { get; }
        private IVertexDeclaration vertexDeclaration;

        public IMaterialHandler Material
        {
            get { return material; }
            set { material = value; }
        }

        public int ActiveParticles
        {
            get { return particles.Count; }
        }

        public ParticleSystemNode(string name)
            : base(name)
        {
            DrawPass = DrawPass.Second;
        }

        public void Initialize(IGraphicsFactory graphicsFactory, int maxNumParticles)
        {
            this.device = graphicsFactory.GraphicsDevice;
            this.maxNumParticles = maxNumParticles;
            particles = new List<ISystemParticle<T>>();
            material = CreateDefaultMaterial(graphicsFactory);

#if !XBOX
            vertexBuffer = graphicsFactory.CreateVertexBuffer(typeof(T), maxNumParticles, /*BufferUsage.WriteOnly |*/ BufferUsage.WriteOnly);
#endif
            vertexDeclaration = graphicsFactory.CreateVertexDeclaration(VertexElements);

            for (int i = 0; i < NumInitialSpawns; i++)
                particles.Add(Spawn());
            
            vertices = new T[maxNumParticles];
        }

        protected override void StepNode()
        {
            particles.RemoveAll(delegate(ISystemParticle<T> particle) { if (particle.IsDead()) return true; else return false; });
            while (maxNumParticles != ActiveParticles && ShouldSpawn())
            {
                particles.Add(Spawn());
            }

            for (int i = 0; i < ActiveParticles; i++)
            {
                particles[i].Step(ref vertices[i]);
            }

#if !XBOX
            if (ActiveParticles != 0)
                vertexBuffer.SetData<T>(vertices, 0, ActiveParticles);
#endif
        }

        protected override void RenderNode(IScene scene)
        {
            if (ActiveParticles == 0)
                return;

            material.SetupRendering(new Matrix[] { WorldMatrix }, scene.ActiveCamera.ViewMatrix, scene.ActiveCamera.ProjectionMatrix, scene.AmbientColor, new LightState());

#if !XBOX
            device.Vertices[0].SetSource(vertexBuffer, 0, VertexSizeInBytes);
            device.VertexDeclaration = vertexDeclaration;
#endif

            material.Effect.Begin();
            foreach (IEffectPass pass in material.Effect.CurrentTechnique.Passes)
            {
                pass.Begin();
#if !XBOX
                device.DrawPrimitives(PrimitiveType.PointList, 0, ActiveParticles);
#else
                device.DrawUserPrimitives<T>(PrimitiveType.PointList, vertices, 0, ActiveParticles);
#endif
                pass.End();
            }
            material.Effect.End();
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

    }
}
