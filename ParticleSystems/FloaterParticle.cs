using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ParticleSystems
{
    public class FloaterSystemNode : ParticleSystemNode<VertexPositionColorPoint>
    {
        private float boundingRadius;
        private Color defaultColor = Color.White;
        private float colorDeviation = 0.4f;
        private float particleBaseSize;
        private float particleSpeed;

        public FloaterSystemNode(string name, float boundingRadius, float particleBaseSize, float particleSpeed)
            : base(name)
        {
            this.boundingRadius = boundingRadius;
            this.particleBaseSize = particleBaseSize;
            this.particleSpeed = particleSpeed;
        }

        protected override int NumInitialSpawns
        {
            get { return maxNumParticles; }
        }

        protected override ISystemParticle<VertexPositionColorPoint> Spawn()
        {
            return new FloaterParticle(RandomPositionInSphere(boundingRadius), CreateColor(), particleBaseSize, particleSpeed);
        }

        private Color CreateColor()
        {
            return new Color(defaultColor.ToVector3() + 
                Rand.Vector3(-colorDeviation, colorDeviation));
        }

        protected override bool ShouldSpawn()
        {
            return false;
        }

        protected override IMaterialHandler CreateDefaultMaterial(IGraphicsFactory graphicsFactory)
        {
            IMaterialHandler material = new MaterialHandler(graphicsFactory.EffectFromFile("Content\\effects\\PointSpriteParticles"), new EffectConverter());
            material.BlendFunction = BlendFunction.Add;
            material.SourceBlend = Blend.One;
            material.DestinationBlend = Blend.One;
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

    public class FloaterParticle : SystemParticle<VertexPositionColorPoint>
    {
        private Vector3 phase;
        private Vector3 period;
        private Vector3 amplitude;

        public FloaterParticle(Vector3 position, Color color, float size, float speed)
            : base(position, color, size)
        {
            phase = new Vector3(Rand.Float(0, 2 * Math.PI), Rand.Float(0, 2 * Math.PI), Rand.Float(0, 2 * Math.PI));
            period = new Vector3(0.5f, 0.5f, 0.5f) + new Vector3(Rand.Float(0, 2 * Math.PI), Rand.Float(0, 2 * Math.PI), Rand.Float(0, 2 * Math.PI));
            period /= speed;
            amplitude = new Vector3(size * 1.0f, size * 1.0f, size * 1.0f) +
                new Vector3(Rand.Float(0, size * 3.0f), Rand.Float(0, size * 3.0f), Rand.Float(0, size * 3.0f));
        }

        public override void Step(ref VertexPositionColorPoint destinationVertex)
        {
            destinationVertex.Position = Position + new Vector3(amplitude.X * (float)Math.Sin(Time.CurrentTime / period.X + phase.X),
                                                     amplitude.Y * (float)Math.Sin(Time.CurrentTime / period.Y + phase.Y),
                                                     amplitude.Z * (float)Math.Sin(Time.CurrentTime / period.Z + phase.Z));
            destinationVertex.PointSize = Size;
            destinationVertex.Color = Color;
        }

    }
}
