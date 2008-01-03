using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class CustomModelMeshTest : D3DMockTest
    {
        private CustomModelMesh modelMesh;
        private IModelMeshPart modelMeshPart1;
        private IModelMeshPart modelMeshPart2;
        private IIndexBuffer indexBuffer;
        private IEffect effect2;
        private IEffectTechnique technique1;
        private IEffectTechnique technique2;
        private IEffectPass pass1;
        private IEffectPass pass2;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            modelMeshPart1 = mockery.NewMock<IModelMeshPart>();
            modelMeshPart2 = mockery.NewMock<IModelMeshPart>();
            indexBuffer = mockery.NewMock<IIndexBuffer>();
            effect2 = mockery.NewMock<IEffect>();
            technique1 = mockery.NewMock<IEffectTechnique>();
            technique2 = mockery.NewMock<IEffectTechnique>();
            pass1 = mockery.NewMock<IEffectPass>();
            pass2 = mockery.NewMock<IEffectPass>();

            Stub.On(modelMeshPart1).GetProperty("Effect").Will(Return.Value(effect));
            Stub.On(modelMeshPart2).GetProperty("Effect").Will(Return.Value(effect2));
            Stub.On(effect).GetProperty("CurrentTechnique").Will(Return.Value(technique1));
            Stub.On(effect2).GetProperty("CurrentTechnique").Will(Return.Value(technique2));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void ConstructorOnePart()
        {
            // Exercise SUT
            modelMesh = new CustomModelMesh(device, null, null, 0, null,
                PrimitiveType.PointList, new IModelMeshPart[] { modelMeshPart1 });
            // Verify
            Assert.AreEqual(1, modelMesh.MeshParts.Count);
            Assert.AreSame(modelMeshPart1, modelMesh.MeshParts[0]);
        }

        [Test]
        public void ConstructorTwoPart()
        {
            // Exercise SUT
            modelMesh = new CustomModelMesh(device, null, null, 0, null,
                PrimitiveType.LineStrip, new IModelMeshPart[] { modelMeshPart1, modelMeshPart2 });
            // Verify
            Assert.AreEqual(2, modelMesh.MeshParts.Count);
            Assert.AreSame(modelMeshPart1, modelMesh.MeshParts[0]);
            Assert.AreSame(modelMeshPart2, modelMesh.MeshParts[1]);
        }

        [Test]
        public void VertexBuffer()
        {
            // Exercise SUT
            modelMesh = new CustomModelMesh(device, vertexBuffer, null, 0, null,
                PrimitiveType.LineList, new IModelMeshPart[] { });
            // Verify
            Assert.AreSame(vertexBuffer, modelMesh.VertexBuffer);
            Assert.AreSame(null, modelMesh.IndexBuffer);
        }

        [Test]
        public void IndexBuffer()
        {
            // Exercise SUT
            modelMesh = new CustomModelMesh(device, null, indexBuffer, 0, null,
                PrimitiveType.TriangleFan, new IModelMeshPart[] { });
            // Verify
            Assert.AreSame(null, modelMesh.VertexBuffer);
            Assert.AreSame(indexBuffer, modelMesh.IndexBuffer);
        }

        [Test]
        public void DrawWithIndexBuffer()
        {
            // Setup
            modelMesh = new CustomModelMesh(device, vertexBuffer, indexBuffer, 42, vertexDeclaration,
                PrimitiveType.PointList, new IModelMeshPart[] { modelMeshPart1, modelMeshPart2 });
            Expect.Once.On(device).SetProperty("Indices").To(indexBuffer);
            Expect.Once.On(vertexStream).Method("SetSource").With(vertexBuffer, 0, 42);
            Expect.Once.On(device).SetProperty("VertexDeclaration").To(vertexDeclaration);
            StubProperties(modelMeshPart1, 1, 2, 3, 4);
            StubProperties(modelMeshPart2, 5, 6, 7, 8);
            ExpectForeachPass(technique1, new IEffectPass[] { pass1 });
            ExpectForeachPass(technique2, new IEffectPass[] { pass2 });
            using (mockery.Ordered)
            {
                Expect.Once.On(effect).Method("Begin");
                Expect.Once.On(pass1).Method("Begin");
                Expect.Once.On(device).Method("DrawIndexedPrimitives").With(PrimitiveType.PointList, 1, 0, 2, 3, 4);
                Expect.Once.On(pass1).Method("End");
                Expect.Once.On(effect).Method("End");
                Expect.Once.On(effect2).Method("Begin");
                Expect.Once.On(pass2).Method("Begin");
                Expect.Once.On(device).Method("DrawIndexedPrimitives").With(PrimitiveType.PointList, 5, 0, 6, 7, 8);
                Expect.Once.On(pass2).Method("End");
                Expect.Once.On(effect2).Method("End");
            }
            // Exercise SUT
            modelMesh.Draw();
        }

        [Test]
        public void DrawWithoutIndexBuffer()
        {
            // Setup
            modelMesh = new CustomModelMesh(device, vertexBuffer, null, 17, vertexDeclaration,
                PrimitiveType.LineStrip, new IModelMeshPart[] { modelMeshPart1, modelMeshPart2 });
            Expect.Once.On(device).SetProperty("Indices").To(Is.Null);
            Expect.Once.On(vertexStream).Method("SetSource").With(vertexBuffer, 0, 17);
            Expect.Once.On(device).SetProperty("VertexDeclaration").To(vertexDeclaration);
            StubProperties(modelMeshPart1, 1, 0, 0, 4);
            StubProperties(modelMeshPart2, 5, 0, 0, 8);
            ExpectForeachPass(technique1, new IEffectPass[] { pass1 });
            ExpectForeachPass(technique2, new IEffectPass[] { pass2 });
            using (mockery.Ordered)
            {
                Expect.Once.On(effect).Method("Begin");
                Expect.Once.On(pass1).Method("Begin");
                Expect.Once.On(device).Method("DrawPrimitives").With(PrimitiveType.LineStrip, 1, 4);
                Expect.Once.On(pass1).Method("End");
                Expect.Once.On(effect).Method("End");
                Expect.Once.On(effect2).Method("Begin");
                Expect.Once.On(pass2).Method("Begin");
                Expect.Once.On(device).Method("DrawPrimitives").With(PrimitiveType.LineStrip, 5, 8);
                Expect.Once.On(pass2).Method("End");
                Expect.Once.On(effect2).Method("End");
            }
            // Exercise SUT
            modelMesh.Draw();
        }

        private void StubProperties(IModelMeshPart modelMeshPart, int baseVertex, int numVertices,
            int startIndex, int primitiveCount)
        {
            Stub.On(modelMeshPart).GetProperty("BaseVertex").Will(Return.Value(baseVertex));
            Stub.On(modelMeshPart).GetProperty("NumVertices").Will(Return.Value(numVertices));
            Stub.On(modelMeshPart).GetProperty("StartIndex").Will(Return.Value(startIndex));
            Stub.On(modelMeshPart).GetProperty("PrimitiveCount").Will(Return.Value(primitiveCount));
        }
    }
}
