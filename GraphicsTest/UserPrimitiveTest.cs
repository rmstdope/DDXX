using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class UserPrimitiveTest : D3DMockTest
    {
        private IUserPrimitive<float> primitive;
        private MaterialHandler material;
        private IEffectTechnique technique;
        private IEffectPass pass;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            material = new MaterialHandler(effect, null);
            technique = mockery.NewMock<IEffectTechnique>();
            pass = mockery.NewMock<IEffectPass>();
            Stub.On(effect).GetProperty("CurrentTechnique").Will(Return.Value(technique));
            Stub.On(vertexDeclaration).GetProperty("GraphicsDevice").Will(Return.Value(device));
        }

        [Test]
        public void EmptyStartEnd()
        {
            // Setup
            primitive = new UserPrimitive<float>(vertexDeclaration, material, PrimitiveType.PointList, 7);
            // Exercise SUT
            primitive.Begin();
            primitive.End();
        }

        [Test]
        public void DrawOnePoint()
        {
            // Setup
            primitive = new UserPrimitive<float>(vertexDeclaration, material, PrimitiveType.PointList, 1);
            SetupOnePass(new float[] { 1.2f }, PrimitiveType.PointList);
            // Exercise SUT
            primitive.Begin();
            primitive.AddVertex(1.2f);
            primitive.End();
        }

        [Test]
        public void DrawMorePoints()
        {
            // Setup
            primitive = new UserPrimitive<float>(vertexDeclaration, material, PrimitiveType.PointList, 4);
            SetupOnePass(new float[] { 1, 2, 3, 4 }, PrimitiveType.PointList);
            // Exercise SUT
            primitive.Begin();
            primitive.AddVertex(1);
            primitive.AddVertex(2);
            primitive.AddVertex(3);
            primitive.AddVertex(4);
            primitive.End();
        }

        [Test]
        public void DrawPointsWithFlush()
        {
            // Setup
            primitive = new UserPrimitive<float>(vertexDeclaration, material, PrimitiveType.PointList, 2);
            SetupOnePass(new float[] { 1, 2 }, PrimitiveType.PointList);
            SetupOnePass(new float[] { 3, 4 }, PrimitiveType.PointList);
            SetupOnePass(new float[] { 5, 4 }, 1, PrimitiveType.PointList);
            // Exercise SUT
            primitive.Begin();
            primitive.AddVertex(1);
            primitive.AddVertex(2);
            primitive.AddVertex(3);
            primitive.AddVertex(4);
            primitive.AddVertex(5);
            primitive.End();
        }

        [Test]
        public void DrawTwoLineList()
        {
            // Setup
            primitive = new UserPrimitive<float>(vertexDeclaration, material, PrimitiveType.LineList, 2);
            SetupOnePass(new float[] { 1.2f, 2.4f }, 1, PrimitiveType.LineList);
            // Exercise SUT
            primitive.Begin();
            primitive.AddVertex(1.2f);
            primitive.AddVertex(2.4f);
            primitive.End();
        }

        [Test]
        public void DrawFourLineListWithFlush()
        {
            // Setup
            primitive = new UserPrimitive<float>(vertexDeclaration, material, PrimitiveType.LineList, 3);
            SetupOnePass(new float[] { 1, 2, 0 }, 1, PrimitiveType.LineList);
            SetupOnePass(new float[] { 3, 4, 0 }, 1, PrimitiveType.LineList);
            // Exercise SUT
            primitive.Begin();
            primitive.AddVertex(1);
            primitive.AddVertex(2);
            primitive.AddVertex(3);
            primitive.AddVertex(4);
            primitive.End();
        }

        [Test]
        public void DrawFourLineStripWithFlush()
        {
            // Setup
            primitive = new UserPrimitive<float>(vertexDeclaration, material, PrimitiveType.LineStrip, 3);
            SetupOnePass(new float[] { 1, 2, 3 }, 2, PrimitiveType.LineStrip);
            SetupOnePass(new float[] { 3, 4, 3 }, 1, PrimitiveType.LineStrip);
            // Exercise SUT
            primitive.Begin();
            primitive.AddVertex(1);
            primitive.AddVertex(2);
            primitive.AddVertex(3);
            primitive.AddVertex(4);
            primitive.End();
        }

        [Test]
        public void DrawSixTriangleListWithFlush()
        {
            // Setup
            primitive = new UserPrimitive<float>(vertexDeclaration, material, PrimitiveType.TriangleList, 5);
            SetupOnePass(new float[] { 1, 2, 3, 0, 0 }, 1, PrimitiveType.TriangleList);
            SetupOnePass(new float[] { 4, 5, 6, 0, 0 }, 1, PrimitiveType.TriangleList);
            // Exercise SUT
            primitive.Begin();
            primitive.AddVertex(1);
            primitive.AddVertex(2);
            primitive.AddVertex(3);
            primitive.AddVertex(4);
            primitive.AddVertex(5);
            primitive.AddVertex(6);
            primitive.End();
        }

        [Test]
        public void DrawFourTriangleStripWithFlush()
        {
            // Setup
            primitive = new UserPrimitive<float>(vertexDeclaration, material, PrimitiveType.TriangleStrip, 3);
            SetupOnePass(new float[] { 1, 2, 3 }, 1, PrimitiveType.TriangleStrip);
            SetupOnePass(new float[] { 2, 3, 4 }, 1, PrimitiveType.TriangleStrip);
            // Exercise SUT
            primitive.Begin();
            primitive.AddVertex(1);
            primitive.AddVertex(2);
            primitive.AddVertex(3);
            primitive.AddVertex(4);
            primitive.End();
        }

        private void SetupOnePass(float[] vertices, PrimitiveType primitiveType)
        {
            SetupOnePass(vertices, vertices.Length, primitiveType);
        }

        private void SetupOnePass(float[] vertices, int num, PrimitiveType primitiveType)
        {
            Expect.Once.On(device).SetProperty("VertexDeclaration").To(vertexDeclaration);
            Expect.Once.On(effect).Method("Begin");
            ExpectForeachPass(technique, new IEffectPass[] { pass });
            Expect.Once.On(pass).Method("Begin");
            Expect.Once.On(device).Method("DrawUserPrimitives").
                With(primitiveType, vertices, 0, num);
            Expect.Once.On(pass).Method("End");
            Expect.Once.On(effect).Method("End");
        }

    }
}
