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
        private VertexColorPoint[] vertexData;
        static private Random rand = new Random();

        public class FallingStarParticle : SystemParticle
        {
            public Vector3 Phase;
            public Vector3 Period;
            public Vector3 Amplitude;
            public FallingStarParticle(Vector3 position, Color color, float size)
                : base(position, color, size)
            {
                Phase = 2.0f * (float)Math.PI * new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
                Period = new Vector3(2, 2, 2) + 4.0f * (float)Math.PI * new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
                Amplitude = new Vector3(20, 20, 20) + 60.0f * new Vector3((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            }
        }

        public FallingStarSystem(string name)
            : base(name)
        {
        }

        public void Initialize(int numParticles, IDevice device, IGraphicsFactory graphicsFactory, 
            IEffectFactory effectFactory, ITexture texture)
        {
            base.InitializeBase(numParticles, device, graphicsFactory, effectFactory);

            vertexData = new VertexColorPoint[NumParticles];

            while (ActiveParticles < NumParticles)
                SpawnParticle();

            if (texture == null)
                effectHandler.Techniques = new EffectHandle[] { EffectHandle.FromString("PointSpriteNoTexture") };
            else
            {
                material.DiffuseTexture = texture;
                effectHandler.Techniques = new EffectHandle[] { EffectHandle.FromString("PointSprite") };
            }
        }

        private void SpawnParticle()
        {
            FallingStarParticle particle = new FallingStarParticle(DistributeEvenlyInSphere(5.0f), Color.White, 10.0f);
            particles.Add(particle);
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

        protected override Type VertexType
        {
            get { return typeof(VertexColorPoint); }
        }

        protected override VertexDeclaration VertexDeclaration
        {
            get 
            {
                VertexElement[] elements = new VertexElement[]
                {
                    new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                    new VertexElement(0, 12, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
                    new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                    VertexElement.VertexDeclarationEnd 
                };
                return D3DDriver.GraphicsFactory.CreateVertexDeclaration(Device, elements);
            }
        }

        protected override void StepNode()
        {
            int i = 0;
            foreach (FallingStarParticle particle in particles)
            {
                vertexData[i].Position = particle.Position + 
                    new Vector3(particle.Amplitude.X * (float)Math.Sin(Time.StepTime / particle.Period.X + particle.Phase.X),
                                particle.Amplitude.Y * (float)Math.Sin(Time.StepTime / particle.Period.Y + particle.Phase.Y),
                                particle.Amplitude.Z * (float)Math.Sin(Time.StepTime / particle.Period.Z + particle.Phase.Z));
                vertexData[i].Size = particle.Size;
                vertexData[i].Color = particle.Color.ToArgb();
                i++;
            }
            VertexBuffer.SetData(vertexData, 0, LockFlags.Discard);
        }

    }
}
