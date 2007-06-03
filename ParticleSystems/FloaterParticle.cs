using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.ParticleSystems
{
    public class FloaterParticleSpawner : ISystemParticleSpawner
    {
        private VertexDeclaration vertexDeclaration;
        private float boundingRadius;
        private int maxNumParticles;

        public FloaterParticleSpawner(IGraphicsFactory graphicsFactory, IDevice device, int maxNumParticles, float boundingRadius)
        {
            this.boundingRadius = boundingRadius;
            this.maxNumParticles = maxNumParticles;
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
                new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd 
            };
            // Use the vertex element array to create a vertex declaration.
            vertexDeclaration = graphicsFactory.CreateVertexDeclaration(device, elements);
        }

        public BlendOperation BlendOperation
        {
            get { return BlendOperation.Add; }
        }

        public Blend SourceBlend
        {
            get { return Blend.One; }
        }

        public Blend DestinationBlend
        {
            get { return Blend.One; }
        }

        public int MaxNumParticles
        {
            get { return maxNumParticles; }
        }

        public ISystemParticle Spawn()
        {
            return new FloaterParticle(RandomPositionInSphere(boundingRadius), Color.White, 10.0f);
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

        public Type VertexType
        {
            get { return typeof(VertexColorPoint); }
        }

        public VertexDeclaration VertexDeclaration
        {
            get { return vertexDeclaration; }
        }

        public int NumInitialSpawns
        {
            get { return maxNumParticles;  }
        }

        public bool ShouldSpawn()
        {
            return false;
        }
    }

    public class FloaterParticle : SystemParticle
    {
        private Vector3 phase;
        private Vector3 period;
        private Vector3 amplitude;

        public FloaterParticle(Vector3 position, Color color, float size)
            : base(position, color, size)
        {
            phase = new Vector3(Rand.Float(0, 2 * Math.PI), Rand.Float(0, 2 * Math.PI), Rand.Float(0, 2 * Math.PI));
            period = new Vector3(2, 2, 2) + new Vector3(Rand.Float(0, 4 * Math.PI), Rand.Float(0, 4 * Math.PI), Rand.Float(0, 4 * Math.PI));
            amplitude = new Vector3(20, 20, 20) + new Vector3(Rand.Float(0, 60), Rand.Float(0, 60), Rand.Float(0, 60));
        }

        public override void StepAndWrite(IGraphicsStream stream)
        {
            VertexColorPoint vertex = new VertexColorPoint();
            vertex.Position = Position + new Vector3(amplitude.X * (float)Math.Sin(Time.StepTime / period.X + phase.X),
                                                     amplitude.Y * (float)Math.Sin(Time.StepTime / period.Y + phase.Y),
                                                     amplitude.Z * (float)Math.Sin(Time.StepTime / period.Z + phase.Z));
            vertex.Size = Size;
            vertex.Color = Color.ToArgb();
            stream.Write(vertex);
        }

    }
}
