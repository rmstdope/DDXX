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
    public class FallingCurtainSystemNode : ParticleSystemNode<VertexPositionColorPoint>
    {
        private Color color = new Color(200, 200, 100, 50);
        private int colorDistortion;
        private float scale;
        private float nextTime = -5;
        private float timeBetweenSpawns = 0.02f;

        public FallingCurtainSystemNode(string name, float scale)
            : base(name)
        {
            colorDistortion = 30;
            this.scale = scale;
        }

        public Color Color
        {
            set { color = value; }
        }

        public int ColorDistortion
        {
            set { colorDistortion = value; }
        }

        protected override ISystemParticle<VertexPositionColorPoint> Spawn()
        {
            float radius = 5.0f*scale;
            const float freq = 2.0f;
            Vector3 position = new Vector3((float)Math.Cos(nextTime*freq) * radius, 0.0f, (float)Math.Sin(nextTime*freq)*radius);
            Color createColor = new Color(
                (byte)(color.R + Rand.Int(-colorDistortion, colorDistortion)),
                (byte)(color.G + Rand.Int(-colorDistortion, colorDistortion)),
                (byte)(color.B + Rand.Int(-colorDistortion, colorDistortion)));
            CurtainParticle particle = new CurtainParticle(position, createColor, nextTime, scale);
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

        protected override IMaterialHandler CreateDefaultMaterial(IGraphicsFactory graphicsFactory)
        {
            IMaterialHandler material = new MaterialHandler(graphicsFactory.EffectFromFile("Content\\effects\\PointSpriteParticles"), new EffectConverter());
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

    public class CurtainParticle : SystemParticle<VertexPositionColorPoint>
    {
        private float stopTime;
        private float startTime;
        private float scale;

        public CurtainParticle(Vector3 position, Color color, float startTime, float scale)
            : base(position, color, 1.0f)
        {
            this.startTime = startTime;
            this.stopTime = (float)(2*Math.PI) + startTime;
            this.scale = scale;
        }

        public override void Step(ref VertexPositionColorPoint destinationVertex)
        {
            Size = (1.5f + (float)Math.Sin(startTime+ Time.CurrentTime) * 0.5f) * scale;
            Position.Y = ((float)Math.Cos(startTime - Time.CurrentTime) * 4.0f) * scale;
            destinationVertex.Position = Position;
            destinationVertex.PointSize = Size;
            destinationVertex.Color = Color;
        }

        public void StepSize(float time)
        {
        }

        public override bool IsDead()
        {
            if (Time.CurrentTime < startTime || Time.CurrentTime > stopTime)
                return true;
            return false;
        }
    }
}
