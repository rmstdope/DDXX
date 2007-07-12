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
        private float nextTime = -10;
        private BlendOperation blendOperation = BlendOperation.Add;
        private Color color = Color.FromArgb(200, Color.White);
        private int colorDistortion;
        private float velocityXZ;
        private float velocityY;
        private float positionDistortion;
        private float timeBetweenSpawns;

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
            velocityY = 10;
            velocityXZ = 30;
            positionDistortion = 20;
            timeBetweenSpawns = 0.003f;
        }

        public float PositionDistortion
        {
            set { positionDistortion = value; }
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

        public float VelocityXZ
        {
            set { velocityXZ = value; }
        }
        public float VelocityY
        {
            set { velocityY = value; }
        }

        public float TimeBetweenSpawns
        {
            set { timeBetweenSpawns = value; }
        }

        public ISystemParticle Spawn(IRenderableCamera camera)
        {
            Vector3 position = new Vector3(Rand.Float(-1, 1), Rand.Float(-1, 1), Rand.Float(-1, 1));
            Vector3 velocity = new Vector3((float)Math.Sin(nextTime * 1.8f), -1.0f, (float)Math.Cos(nextTime * 1.8f));
            position *= positionDistortion;
            velocity.X *= velocityXZ;
            velocity.Z *= velocityXZ;
            velocity.Y *= velocityY;
            velocity += new Vector3(Rand.Float(-1, 1), Rand.Float(-1, 1), Rand.Float(-1, 1)) * 1.4f;
            Color createColor = Color.FromArgb(
                color.R + Rand.Int(-colorDistortion, colorDistortion),
                color.G + Rand.Int(-colorDistortion, colorDistortion),
                color.B + Rand.Int(-colorDistortion, colorDistortion));
            SpiralParticle particle = new SpiralParticle(position, velocity, Rand.Float(5, 10), createColor);
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

    public class SpiralParticle : SystemParticle
    {
        private Vector3 velocity;
        private float stopTime;

        public SpiralParticle(Vector3 position, Vector3 velocity, float size, Color color)
            : base(position, color, size)
        {
            this.velocity = velocity;
            stopTime = 20;
        }

        public override void StepAndWrite(IGraphicsStream stream, IRenderableCamera camera)
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
            //if (stopTime <= 0)
            //    newTime = 0;
            //else if (stopTime - newTime <= 0)
            //    newTime = stopTime;
            //stopTime -= newTime;
            Position += velocity * newTime;
            //Position.Y += velocity.Y * (time - newTime);
                //Vector2 xz = new Vector2(Position.X, Position.Z);
                //float length = xz.Length();
                //if (length > 100)
                //{
                //    xz *= 100 / length;
                //    Position.X = xz.X;
                //    Position.Z = xz.Y;
                //}
        }

        public override bool  IsDead()
        {
            if (stopTime <= 0)
                return true;
            return false;
        }
    }
}
