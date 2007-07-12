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
    public class CloudParticleSpawner : ISystemParticleSpawner
    {
        private VertexDeclaration vertexDeclaration;
        private int maxNumParticles;
        private BlendOperation blendOperation = BlendOperation.Add;
        private Color color = Color.FromArgb(200, Color.White);
        private int colorDistortion;

        public CloudParticleSpawner(IGraphicsFactory graphicsFactory, IDevice device, int maxNumParticles)
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
            get { return blendOperation; }
            set { blendOperation = value; }
        }

        public Blend SourceBlend
        {
            get { return Blend.One; }
        }

        public Blend DestinationBlend
        {
            get { return Blend.InvSourceColor; }
        }

        public Color Color
        {
            set { color = value; }
        }

        public int ColorDistortion
        {
            set { colorDistortion = value; }
        }

        public ISystemParticle Spawn(IRenderableCamera camera)
        {
            float phi = Rand.Float(Math.PI);
            float rho = Rand.Float(-Math.PI / 2, Math.PI / 2);
            float size = Rand.Float(1f, 2f);
            Color createColor = Color.FromArgb(
                color.R + Rand.Int(-colorDistortion, colorDistortion),
                color.G + Rand.Int(-colorDistortion, colorDistortion),
                color.B + Rand.Int(-colorDistortion, colorDistortion));
            float x = Rand.Float(-5, 5);//(float)(10 * Math.Cos(phi) * Math.Sin(rho));
            float y = Rand.Float(-5, 5);//(float)(10 * Math.Sin(phi) * Math.Sin(rho));
            float z = Rand.Float(0, 10);//(float)(10 * Math.Cos(rho));
            Vector3 pos = new Vector3(x, y, z) + camera.Position;
            CloudParticle particle = new CloudParticle(pos, size, createColor);
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
            get { return maxNumParticles; }
        }

        public int MaxNumParticles
        {
            get { return maxNumParticles; }
        }

        public bool ShouldSpawn()
        {
            return true;
        }

        public string GetTechniqueName(bool textured)
        {
            if (textured)
                return "PointSprite";
            return "PointSpriteNoTexture";
        }
    }

    public class CloudParticle : SystemParticle
    {
        private bool dead;

        public CloudParticle(Vector3 pos, float size, Color color)
            : base(pos, color, size)
        {
            dead = false;
        }

        public override void StepAndWrite(IGraphicsStream stream, IRenderableCamera camera)
        {
            VertexColorPoint vertex;

            vertex.Position = Position;
            vertex.Size = Size;
            vertex.Color = Color.ToArgb();
            stream.Write(vertex);

            Vector3 toParticle = Position - camera.Position;
            if (Vector3.Dot(camera.WorldState.Forward, toParticle) < 0)
                dead = true;
        }

        public override bool IsDead()
        {
            return dead;
        }
    }
}
