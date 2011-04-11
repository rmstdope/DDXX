using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.ParticleSystems
{
    public class SpiralSystemNode : ParticleSystemNode<VertexPositionColorPoint>
    {
        private float nextTime = -5;
        private Color color = new Color(100, 80, 200, 200);
        private int colorDistortion;
        private float velocityXZ;
        private float velocityY;
        private float positionDistortion;
        private float timeBetweenSpawns;
        private float scale;

        public SpiralSystemNode(string name, float scale)
            : base(name)
        {
            velocityY = 0.5f;
            velocityXZ = 1.0f;
            positionDistortion = 0.5f;
            timeBetweenSpawns = 0.01f;
            colorDistortion = 30;
            this.scale = scale;
        }

        public float PositionDistortion
        {
            set { positionDistortion = value; }
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

        protected override ISystemParticle<VertexPositionColorPoint> Spawn()
        {
            Vector3 position = new Vector3(Rand.Float(-1, 1), Rand.Float(-1, 1), Rand.Float(-1, 1));
            Vector3 velocity = new Vector3((float)Math.Sin(nextTime * 1.8f), -1.0f, (float)Math.Cos(nextTime * 1.8f));
            position *= positionDistortion * scale;
            velocity.X *= velocityXZ * scale;
            velocity.Z *= velocityXZ * scale;
            velocity.Y *= velocityY * scale;
            velocity += new Vector3(Rand.Float(-1, 1), Rand.Float(-1, 1), Rand.Float(-1, 1)) * 0.05f * scale;
            Color createColor = new Color(
                (byte)(color.R + Rand.Int(-colorDistortion, colorDistortion)),
                (byte)(color.G + Rand.Int(-colorDistortion, colorDistortion)),
                (byte)(color.B + Rand.Int(-colorDistortion, colorDistortion)));
            SpiralParticle particle = new SpiralParticle(position, velocity, Rand.Float(scale * 0.3f, scale * 0.8f), createColor, nextTime);
            particle.StepPosition(Time.CurrentTime - nextTime - Time.DeltaTime);
            nextTime += timeBetweenSpawns;
            return particle;
        }

        protected override int NumInitialSpawns
        {
            get { return 0; }
        }

        protected override bool ShouldSpawn()
        {
            return (Time.CurrentTime > nextTime);
        }

        protected override MaterialHandler CreateDefaultMaterial(IGraphicsFactory graphicsFactory)
        {
            MaterialHandler material = new MaterialHandler(graphicsFactory.EffectFromFile("Content\\effects\\PointSpriteParticles"), new EffectConverter());
            material.BlendFunction = BlendFunction.Add;
            material.SourceBlend = Blend.One;
            material.DestinationBlend = Blend.InverseSourceColor;
            return material;
        }

        protected override int VertexSizeInBytes
        {
            get { return VertexPositionColorPoint.SizeInBytes; }
        }

        protected override VertexElement[] VertexElements
        {
            get { return VertexPositionColorPoint.VertexElements; }
        }

    }

    public class SpiralParticle : SystemParticle<VertexPositionColorPoint>
    {
        private Vector3 velocity;
        private float stopTime;
        private float startTime;

        public SpiralParticle(Vector3 position, Vector3 velocity, float size, Color color, float startTime)
            : base(position, color, size)
        {
            this.velocity = velocity;
            this.startTime = startTime;
            this.stopTime = 100 + startTime;
        }

        public override void Step(ref VertexPositionColorPoint destinationVertex)
        {
            StepPosition(Time.DeltaTime);
            destinationVertex.Position = Position;
            destinationVertex.PointSize = Size;
            destinationVertex.Color = Color;
        }

        public void StepPosition(float time)
        {
            Position += velocity * time;
        }

        public override bool  IsDead()
        {
            if (Time.CurrentTime < startTime || Time.CurrentTime > stopTime)
                return true;
            return false;
        }
    }
}
