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
    public class MovingTrailNode : ParticleSystemNode<VertexPositionColorPoint>
    {
        private Color defaultColor = Color.White;
        private float colorDeviation = 0.2f;
        private float particleBaseSize;
        private float particleSpeed;

        public MovingTrailNode(string name, float particleBaseSize, float particleSpeed)
            : base(name)
        {
            this.particleBaseSize = particleBaseSize;
            this.particleSpeed = particleSpeed;
        }

        protected override int NumInitialSpawns
        {
            get { return maxNumParticles; }
        }

        protected override ISystemParticle<VertexPositionColorPoint> Spawn()
        {
            return new MovingTrailParticle(CreateColor(), particleBaseSize, particleSpeed);
        }

        private Color CreateColor()
        {
            return new Color(defaultColor.ToVector3() +
                Rand.Vector3(-colorDeviation, colorDeviation));
        }

        protected override bool ShouldSpawn()
        {
            return true;
        }

        protected override IMaterialHandler CreateDefaultMaterial(IGraphicsFactory graphicsFactory)
        {
            IMaterialHandler material = new MaterialHandler(graphicsFactory.EffectFromFile("Content\\effects\\PointSpriteParticles"), new EffectConverter());
            material.DiffuseTexture = graphicsFactory.TextureFactory.WhiteTexture;
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

    public class MovingTrailParticle : SystemParticle<VertexPositionColorPoint>
    {
        private Vector3 velocity;
        private float alpha;

        public MovingTrailParticle(Color color, float size, float speed)
            : base(Vector3.Zero, color, size)
        {
            alpha = 1;
            velocity = Rand.Vector3(-1, 1) * speed;
        }

        public override void Step(ref VertexPositionColorPoint destinationVertex)
        {
            alpha -= Rand.Float(0.02f);
            Position += (velocity * alpha * Time.DeltaTime);
            destinationVertex.Position = Position;
            destinationVertex.PointSize = Size;
            destinationVertex.Color = new Color(Color.ToVector3() * alpha);
        }

        public override bool IsDead()
        {
            return alpha <= 0.0f;
        }
    }
}
