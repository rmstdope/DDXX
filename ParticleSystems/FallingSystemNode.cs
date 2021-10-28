using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
namespace Dope.DDXX.ParticleSystems
{
    public class FallingSystemNode : ParticleSystemNode<VertexPositionColorPoint>
    {
        //private float nextTime = -5;
        private Color color = new Color(100, 80, 200, 200);
        private int colorDistortion;
        private Vector3 AabbMin = new Vector3(-30, -30, -30);
        private Vector3 AabbMax = new Vector3(30, 30, 30);
        //private float velocityXZ;
        //private float velocityY;
        //private float positionDistortion;
        //private float timeBetweenSpawns;
        //private float scale;

        public FallingSystemNode(string name, float scale)
            : base(name)
        {
            //velocityY = 0.5f;
            //velocityXZ = 1.0f;
            //positionDistortion = 0.5f;
            //timeBetweenSpawns = 0.01f;
            colorDistortion = 30;
            //this.scale = scale;
        }

        //public float PositionDistortion
        //{
        //    set { positionDistortion = value; }
        //}

        public Color Color
        {
            set { color = value; }
        }

        public int ColorDistortion
        {
            set { colorDistortion = value; }
        }

        //public float NextTime
        //{
        //    set { nextTime = value; }
        //}

        //public float VelocityXZ
        //{
        //    set { velocityXZ = value; }
        //}
        //public float VelocityY
        //{
        //    set { velocityY = value; }
        //}

        //public float TimeBetweenSpawns
        //{
        //    set { timeBetweenSpawns = value; }
        //}

        protected override ISystemParticle<VertexPositionColorPoint> Spawn()
        {
            Color createColor = new Color(
                (byte)(color.R + Rand.Int(-colorDistortion, colorDistortion)),
                (byte)(color.G + Rand.Int(-colorDistortion, colorDistortion)),
                (byte)(color.B + Rand.Int(-colorDistortion, colorDistortion)));
            FallingParticle particle = new FallingParticle(Rand.Vector3(AabbMin, AabbMax), Rand.Float(0.3f, 0.4f), Rand.Float(0.5f, 1.0f), createColor, -10, 10);
            return particle;
        }

        protected override int NumInitialSpawns
        {
            get { return maxNumParticles; }
        }

        protected override bool ShouldSpawn()
        {
            return false;
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

    public class FallingParticle : SystemParticle<VertexPositionColorPoint>
    {
        private float period;
        private Vector2 positionXZ;
        private float minZ;
        private float maxZ;

        public FallingParticle(Vector3 position, float period, float size, Color color, float minZ, float maxZ)
            : base(position, color, size)
        {
            this.period = period;
            this.positionXZ = new Vector2(position.X, position.Z);
            this.minZ = minZ;
            this.maxZ = maxZ;
        }

        public override void Step(ref VertexPositionColorPoint destinationVertex)
        {
            StepPosition();
            destinationVertex.Position = Position;
            destinationVertex.PointSize = Size;
            destinationVertex.Color = Color;
        }

        public void StepPosition()
        {
            Position = new Vector3(positionXZ.X + 0.5f * (float)Math.Sin(Time.CurrentTime / period),
                Position.Y - Time.DeltaTime * 4.0f,
                positionXZ.Y + 0.5f * (float)Math.Cos(Time.CurrentTime / period));
            if (Position.Y < minZ)
                Position.Y += (maxZ - minZ);
        }

        public override bool IsDead()
        {
            return false;
        }
    }
}
*/