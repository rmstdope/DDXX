using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.Utility;

namespace Dope.DDXX.ParticleSystems
{
    public class SpiralParticleSpawner : ISystemParticleSpawner
    {
        private VertexDeclaration vertexDeclaration;
        private int maxNumParticles;
        private float nextTime = -20;

        public SpiralParticleSpawner(IGraphicsFactory graphicsFactory, IDevice device, int maxNumParticles)
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
        }

        public BlendOperation BlendOperation
        {
            get { return BlendOperation.Add; }
        }

        public Blend SourceBlend
        {
            get { return Blend.One; }//.SourceAlpha; }
        }

        public Blend DestinationBlend
        {
            get { return Blend.InvSourceColor; }//.InvSourceAlpha; }
        }

        public ISystemParticle Spawn()
        {
            Vector3 position = new Vector3(Rand.Float(-1, 1), Rand.Float(-1, 1), Rand.Float(-1, 1));
            Vector3 velocity = new Vector3((float)Math.Sin(nextTime * 0.8f), -0.6f, (float)Math.Cos(nextTime * 0.8f));
            position *= 10;
            velocity *= 20;
            velocity += new Vector3(Rand.Float(-1, 1), Rand.Float(-1, 1), Rand.Float(-1, 1)) * 1.4f;
            SpiralParticle particle = new SpiralParticle(position, velocity, Rand.Float(5, 10));
            particle.Step(Time.StepTime - nextTime);
            nextTime += 0.02f;
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

    public class SpiralParticle : SystemParticle
    {
        private Vector3 velocity;
        private float stopTime;

        public SpiralParticle(Vector3 position, Vector3 velocity, float size)
            : base(position, Color.FromArgb(200, Color.White), size)
        {
            this.velocity = velocity;
            stopTime = 7;
        }

        public override void StepAndWrite(IGraphicsStream stream)
        {
            VertexColorPoint vertex;

            Step(Time.DeltaTime);
            vertex.Position = Position;
            vertex.Size = Size;
            vertex.Color = Color.ToArgb();
            stream.Write(vertex);
        }

        public void Step(float time)
        {
            float newTime = time;
            if (stopTime <= 0)
                newTime = 0;
            else if (stopTime - newTime <= 0)
                newTime = stopTime;
            stopTime -= newTime;
            Position += velocity * newTime;
            Position.Y += velocity.Y * (time - newTime);
                //Vector2 xz = new Vector2(Position.X, Position.Z);
                //float length = xz.Length();
                //if (length > 100)
                //{
                //    xz *= 100 / length;
                //    Position.X = xz.X;
                //    Position.Z = xz.Y;
                //}
        }
    }
}
