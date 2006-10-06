using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.ParticleSystems
{
    public class FloaterSystem : ParticleSystemNode
    {
        public struct FloaterVertex
        {
            public Vector3 Position;
            public float Size;
            public int Color;
            public FloaterVertex(Vector3 position, float size, int color)
            {
                Position = position;
                Size = size;
                Color = color;
            }
        }

        private float boundaryRadius;
        private IVertexBuffer vertexBuffer;
        private VertexDeclaration vertexDeclaration;

        protected override IVertexBuffer VertexBuffer
        {
            get { return vertexBuffer; }
        }

        protected override VertexDeclaration VertexDeclaration
        {
            get { return vertexDeclaration; }
        }

        public float BoundaryRadius
        {
            get { return boundaryRadius; }
        }

        public FloaterSystem(string name)
            : base(name)
        {
        }

        public void Initialize(int numParticles, float boundaryRadius)
        {
            InitializeBase(numParticles);
            this.boundaryRadius = boundaryRadius;

            CreateVertexBuffer();

            CreateVertexDeclaration();
        }

        private void CreateVertexDeclaration()
        {
            VertexElement[] elements = new VertexElement[]
            {
                new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                new VertexElement(0, 12, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
                new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                VertexElement.VertexDeclarationEnd 
            };

            // Use the vertex element array to create a vertex declaration.
            vertexDeclaration = D3DDriver.Factory.CreateVertexDeclaration(Device, elements);
        }

        private void CreateVertexBuffer()
        {
            vertexBuffer = D3DDriver.Factory.CreateVertexBuffer(typeof(FloaterVertex), NumParticles, Device, Usage.WriteOnly | Usage.Dynamic, VertexFormats.None, Pool.Default);
            IGraphicsStream stream = vertexBuffer.Lock(0, 0, LockFlags.Discard);
            for (int i = 0; i < NumParticles; i++)
            {
                stream.Write(new FloaterVertex(DistributeEvenlyInSphere(this.boundaryRadius), 1.0f, Color.White.ToArgb()));
            }
            vertexBuffer.Unlock();
        }

        private static Vector3 DistributeEvenlyInSphere(float radius)
        {
            Random rand = new Random();
            Vector3 pos;
            do
            {
                pos = new Vector3((float)(rand.NextDouble() * radius),
                                  (float)(rand.NextDouble() * radius),
                                  (float)(rand.NextDouble() * radius));
            } while (pos.Length() > radius);
            return pos;
        }
    }
}
