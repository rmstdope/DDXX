using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using System.Drawing;
using Dope.DDXX.Utility;

/*
namespace Dope.DDXX.ParticleSystems
{
    public class GlitterParticleSpawner : ISystemParticleSpawner
    {
        private VertexDeclaration vertexDeclaration;
        private int maxNumParticles;

        public GlitterParticleSpawner(IGraphicsFactory graphicsFactory, IDevice device, int maxNumParticles)
        {
            this.maxNumParticles = maxNumParticles;
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                new VertexElement(0, 24, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
                new VertexElement(0, 28, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd 
            };
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

        public ISystemParticle Spawn(IRenderableCamera camera)
        {
            GlitterParticle particle = new GlitterParticle();
            return particle;
        }

        public Type VertexType
        {
            get { return typeof(VertexNormalColorPoint); }
        }

        public VertexDeclaration VertexDeclaration
        {
            get { return vertexDeclaration; }
        }

        public int NumInitialSpawns
        {
            get { return maxNumParticles; }
        }

        public int MaxNumParticles
        {
            get { return maxNumParticles; }
        }

        public bool ShouldSpawn()
        {
            return false;
        }

        public string GetTechniqueName(bool textured)
        {
            return "Glitter";
        }
    }

    public class GlitterParticle : SystemParticle
    {
        private Vector3 velocity;
        private float rotationSpeed;
        private const float Radius = 5;

        public GlitterParticle()
            : base(new Vector3(), Color.FromArgb(255, Color.White), 1)
        {
            Spawn();
        }

        private static Vector3 RandomPositionInSphere(float radius)
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

        public override void StepAndWrite(IGraphicsStream stream, IRenderableCamera camera)
        {
            VertexNormalColorPoint vertex;

            Step(Time.DeltaTime);
            vertex.Position = Position;
            vertex.Normal = new Vector3();
            vertex.Size = Size;
            vertex.Color = Color.ToArgb();
            vertex.Normal = new Vector3(0, 0, (float)Math.Cos(rotationSpeed * (Time.StepTime + 10)));
            stream.Write(vertex);
        }

        public void Step(float time)
        {
            Position += velocity * time;
            if (Position.Length() > Radius)
                Spawn();
        }

        private void Spawn()
        {
            Position = RandomPositionInSphere(Radius);
            velocity = new Vector3(0, -Rand.Float(0, 0.5) - 0.5f, 0);
            Size = Rand.Float(0.01, 0.01) * 6;
            rotationSpeed = Rand.Float(5, 10);
        }
    }
}
*/