using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using System.Drawing;
using Dope.DDXX.Utility;

/*
namespace Dope.DDXX.ParticleSystems
{
    public class SwirlParticleSpawner : ISystemParticleSpawner
    {
        private VertexDeclaration vertexDeclaration;
        private int maxNumParticles;
        private BlendOperation blendOperation = BlendOperation.Add;
        private Color color = Color.FromArgb(200, Color.White);
        float nextTime = 0;
        private int colorDistortion;
        private float timeBetweenSpawns;

        public SwirlParticleSpawner(IGraphicsFactory graphicsFactory, IDevice device, int maxNumParticles)
        {
            this.maxNumParticles = maxNumParticles;
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
                new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd 
            };
            vertexDeclaration = graphicsFactory.CreateVertexDeclaration(device, elements);
            timeBetweenSpawns = 0.003f;
        }

        public BlendOperation BlendOperation
        {
            get { return blendOperation; }
            set { blendOperation = value; }
        }

        public Blend SourceBlend
        {
            get { return Blend.One; }//.SourceAlpha; }
        }

        public Blend DestinationBlend
        {
            get { return Blend.InvSourceColor; }//.InvSourceAlpha; }
        }

        public Color Color
        {
            set { color = value; }
        }

        public int ColorDistortion
        {
            set { colorDistortion = value; }
        }

        public float NextTime
        {
            set { nextTime = value; }
        }

        public float TimeBetweenSpawns
        {
            set { timeBetweenSpawns = value; }
        }

        public ISystemParticle Spawn(IRenderableCamera camera)
        {
            float phi = Rand.Float(Math.PI * 2);
            float angleVelocity = Rand.Float(2.0f, 4.5f);
            float yVelocity = Rand.Float(1.5f, 2.8f);
            float radius = Rand.Float(1, 2.5f);
            float size = Rand.Float(0.1f, 0.2f);
            Color createColor = Color.FromArgb(
                color.R + Rand.Int(-colorDistortion, colorDistortion),
                color.G + Rand.Int(-colorDistortion, colorDistortion),
                color.B + Rand.Int(-colorDistortion, colorDistortion));
            SwirlParticle particle = new SwirlParticle(radius, phi, angleVelocity, yVelocity, size, createColor);
            particle.Step(Time.StepTime - nextTime);
            nextTime += timeBetweenSpawns;
            return particle;
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
            get { return 0; }
        }

        public int MaxNumParticles
        {
            get { return maxNumParticles; }
        }

        public bool ShouldSpawn()
        {
            return (Time.StepTime > nextTime);
        }

        public string GetTechniqueName(bool textured)
        {
            if (textured)
                return "PointSprite";
            return "PointSpriteNoTexture";
        }
    }

    public class SwirlParticle : SystemParticle
    {
        private float radius;
        private float phi;
        private float angleVelocity;
        private float yVelocity;
        private float y;
        private const float LifeTime = 2;
        private float cumulativeTime;

        public SwirlParticle(float radius, float phi, float angleVelocity, float yVelocity, float size, Color color)
            : base(new Vector3(), color, size)
        {
            this.radius = radius;
            this.phi = phi;
            this.angleVelocity = angleVelocity;
            this.yVelocity = yVelocity;
            y = 0;
            cumulativeTime = 0;
        }

        public override void StepAndWrite(IGraphicsStream stream, IRenderableCamera camera)
        {
            VertexColorPoint vertex;

            Step(Time.DeltaTime);
            vertex.Position = Position;
            vertex.Size = Size;
            if (cumulativeTime < 0.5f)
            {
                float d = cumulativeTime / 0.5f;
                vertex.Color = Color.FromArgb((int)(Color.R * d), (int)(Color.G * d), (int)(Color.B * d)).ToArgb();
            }
            else
                vertex.Color = Color.ToArgb();
            stream.Write(vertex);
        }

        public void Step(float time)
        {
            phi += time * angleVelocity;
            y += time * yVelocity;
            cumulativeTime += time;
            Position = new Vector3((float)Math.Sin(phi) * radius, y, (float)Math.Cos(phi) * radius);
        }

        public override bool IsDead()
        {
            if (cumulativeTime > LifeTime)
                return true;
            return false;
        }
    }
}
*/