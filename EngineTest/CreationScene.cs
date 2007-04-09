using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Dope.DDXX.SceneGraph;

namespace EngineTest
{
    public class CreationScene : BaseDemoEffect
    {
        private UnindexedMesh mesh;
        private struct Vertex
        {
            public Vertex(Vector3 position)
            {
                Position = position;
            }
            Vector3 Position;
        }

        public CreationScene(float startTime, float endTime)
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Vertex[] vertices = new Vertex[]
            {
                new Vertex(new Vector3(0, 0, 0)),
                new Vertex(new Vector3(20, 0, 0)),
                new Vertex(new Vector3(20, -10, 0)),
                new Vertex(new Vector3(0, -10, 0))
            };

            mesh = new UnindexedMesh(D3DDriver.GraphicsFactory, typeof(Vertex), 4, D3DDriver.GetInstance().Device,
                Usage.WriteOnly | Usage.Dynamic, VertexFormats.Position, Pool.Default);
            mesh.SetVertexBufferData(vertices, LockFlags.Discard);

            //IVertexBuffer buffer = D3DDriver.GraphicsFactory.
            //    CreateVertexBuffer(typeof(Vertex), 4, D3DDriver.GetInstance().Device,
            //    Usage.Dynamic, VertexFormats.Position, Pool.Default);
            //buffer.SetData(vertices, 0, LockFlags.Discard);
            IEffect effect = EffectFactory.CreateFromFile("Test.fxo");
            EffectHandler effectHandler = new EffectHandler(effect, "Test", null);
        }

        public override void Step()
        {
        }

        public override void Render()
        {
        }
    }
}
