using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using System.Drawing;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.ParticleSystems
{
    public class SphereParticleSpawner : ISystemParticleSpawner
    {
        private VertexDeclaration vertexDeclaration;
        private float boundingRadius;
        private int maxNumParticles;
        private int colorDistortion;
        private Color color = Color.FromArgb(200, Color.White);
        private float size;
        private float sizeDistortion;

        public float SizeDistortion
        {
            set { sizeDistortion = value; }
        }

        public float Size
        {
            set { size = value; }
        }

        public int ColorDistortion
        {
            set { colorDistortion = value; }
        }

        public Color Color
        {
            set { color = value; }
        }

        public SphereParticleSpawner(IGraphicsFactory graphicsFactory, IDevice device, int maxNumParticles, float boundingRadius)
        {
            this.boundingRadius = boundingRadius;
            this.maxNumParticles = maxNumParticles;
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
                new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd 
            };
            // Use the vertex element array to create a vertex declaration.
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

        public int MaxNumParticles
        {
            get { return maxNumParticles; }
        }

        public ISystemParticle Spawn(IRenderableCamera camera)
        {
            Color createColor = Color.FromArgb(
                color.R + Rand.Int(-colorDistortion, colorDistortion),
                color.G + Rand.Int(-colorDistortion, colorDistortion),
                color.B + Rand.Int(-colorDistortion, colorDistortion));
            return new SphereParticle(Rand.Float(boundingRadius * 0.8f, boundingRadius), createColor, size + Rand.Float(-sizeDistortion, sizeDistortion));
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

    public class SphereParticle : SystemParticle
    {
        private Vector3 period;

        public SphereParticle(float radius, Color color, float size)
            : base(new Vector3(), color, size)
        {
            period = new Vector3(0, 0, 0) + new Vector3(Rand.Float(-0.2f, 0.2f), Rand.Float(-0.2f, 0.2f), Rand.Float(-0.2f, 0.2f));
            Position = new Vector3(Rand.Float(-1, 1), Rand.Float(-1, 1), Rand.Float(-1, 1));
            Position.Normalize();
            Position = Position * radius;
        }

        public override void StepAndWrite(IGraphicsStream stream, IRenderableCamera camera)
        {
            Matrix matrix = Matrix.RotationYawPitchRoll(period.X * Time.DeltaTime, period.Y * Time.DeltaTime, period.Z * Time.DeltaTime);
            VertexColorPoint vertex = new VertexColorPoint();
            Position.TransformCoordinate(matrix);
            vertex.Position = Position;
            vertex.Size = Size;
            vertex.Color = Color.ToArgb();
            stream.Write(vertex);
        }

    }
}
