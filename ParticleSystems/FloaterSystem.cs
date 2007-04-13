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
        public class FloaterParticle : SystemParticle
        {
            public Vector3 Phase;
            public Vector3 Period;
            public Vector3 Amplitude;
            public FloaterParticle(Vector3 position, Color color, float size)
                : base(position, color, size)
            {
                Phase = 2.0f * (float)Math.PI * new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
                Period = new Vector3(2, 2, 2) + 4.0f * (float)Math.PI * new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
                Amplitude = new Vector3(20, 20, 20) + 60.0f * new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            }
        }

        private float boundaryRadius;
        private VertexDeclaration vertexDeclaration;
        static private Random rand = new Random();

        protected override Type VertexType
        {
            get { return typeof(VertexColorPoint); }
        }

        protected override VertexDeclaration VertexDeclaration
        {
            get { return vertexDeclaration; }
        }

        public float BoundaryRadius
        {
            get { return boundaryRadius; }
        }

        public FloaterSystem(string name)
            : base(name)
        {
        }

        public void Initialize(int numParticles, float boundaryRadius, string texture)
        {
            InitializeBase(numParticles, D3DDriver.GetInstance().Device, 
                D3DDriver.GraphicsFactory, D3DDriver.EffectFactory);
            this.boundaryRadius = boundaryRadius;

            CreateVertexDeclaration();

            while (ActiveParticles < NumParticles)
            {
                SpawnParticle();
            }

            if (texture == null)
                effectHandler.Techniques = new EffectHandle[] { EffectHandle.FromString("PointSpriteNoTexture") };
            else
            {
                material.DiffuseTexture = D3DDriver.TextureFactory.CreateFromFile(texture);
                effectHandler.Techniques = new EffectHandle[] { EffectHandle.FromString("PointSprite") };
            }
        }

        private void SpawnParticle()
        {
            FloaterParticle particle = new FloaterParticle(DistributeEvenlyInSphere(boundaryRadius), Color.White, 10.0f);
            particles.Add(particle);
        }

        private void CreateVertexDeclaration()
        {
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
                new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd 
            };

            // Use the vertex element array to create a vertex declaration.
            vertexDeclaration = D3DDriver.GraphicsFactory.CreateVertexDeclaration(Device, elements);
        }

        private Vector3 DistributeEvenlyInSphere(float radius)
        {
            Vector3 pos;
            do
            {
                pos = new Vector3((float)(rand.NextDouble() * radius),
                                  (float)(rand.NextDouble() * radius),
                                  (float)(rand.NextDouble() * radius));
            } while (pos.Length() > radius);
            return pos;
        }

        protected override void StepNode()
        {
            VertexColorPoint vertex = new VertexColorPoint();
            using (IGraphicsStream stream = VertexBuffer.Lock(0, 0, LockFlags.Discard))
            {
                foreach (FloaterParticle particle in particles)
                {
                    vertex.Position = particle.Position + new Vector3(particle.Amplitude.X * (float)Math.Sin(Time.StepTime / particle.Period.X + particle.Phase.X),
                                                                      particle.Amplitude.Y * (float)Math.Sin(Time.StepTime / particle.Period.Y + particle.Phase.Y),
                                                                      particle.Amplitude.Z * (float)Math.Sin(Time.StepTime / particle.Period.Z + particle.Phase.Z));
                    vertex.Size = particle.Size;
                    vertex.Color = particle.Color.ToArgb();
                    stream.Write(vertex);
                }
                VertexBuffer.Unlock();
            }
        }

    }
}
