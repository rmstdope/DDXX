using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class UvMapBoxTest : ModifierBase
    {
        private UvMapBox map;
        private IPrimitive primitive;
        private IPrimitive generatedPrimitive;

        public UvMapBoxTest()
            : base(0)
        {
        }

        [SetUp]
        public void SetUp()
        {
            map = new UvMapBox();
            map.ConnectToInput(0, this);
        }

        [Test]
        public void PositiveX()
        {
            CreateSingleVertexPrimitive(new Vertex[] {
                new Vertex(new Vector3(0, -1, -1), new Vector3(1, 0, 0)),
                new Vertex(new Vector3(0, 0, -1), new Vector3(1, 0, 0)),
                new Vertex(new Vector3(0, 1, 1), new Vector3(1, 0, 0))
            });
            Assert.AreEqual(new Vector2(0.0f, 0.0f), generatedPrimitive.Vertices[0].UV);
            Assert.AreEqual(new Vector2(0.0f, 0.5f), generatedPrimitive.Vertices[1].UV);
            Assert.AreEqual(new Vector2(1.0f, 1.0f), generatedPrimitive.Vertices[2].UV);
        }

        [Test]
        public void NegativeX()
        {
            CreateSingleVertexPrimitive(new Vertex[] {
                new Vertex(new Vector3(0, -1, -1), new Vector3(-1, 0, 0)),
                new Vertex(new Vector3(0, 0, -1), new Vector3(-1, 0, 0)),
                new Vertex(new Vector3(0, 1, 1), new Vector3(-1, 0, 0))
            });
            Assert.AreEqual(new Vector2(0.0f, 0.0f), generatedPrimitive.Vertices[0].UV);
            Assert.AreEqual(new Vector2(0.0f, 0.5f), generatedPrimitive.Vertices[1].UV);
            Assert.AreEqual(new Vector2(1.0f, 1.0f), generatedPrimitive.Vertices[2].UV);
        }

        [Test]
        public void PositiveZ()
        {
            CreateSingleVertexPrimitive(new Vertex[] {
                new Vertex(new Vector3(-1, -1, 0), new Vector3(0, 0, 1)),
                new Vertex(new Vector3(-1, 0, 0), new Vector3(0, 0, 1)),
                new Vertex(new Vector3(1, 1, 0), new Vector3(0, 0, 1))
            });
            Assert.AreEqual(new Vector2(0.0f, 0.0f), generatedPrimitive.Vertices[0].UV);
            Assert.AreEqual(new Vector2(0.0f, 0.5f), generatedPrimitive.Vertices[1].UV);
            Assert.AreEqual(new Vector2(1.0f, 1.0f), generatedPrimitive.Vertices[2].UV);
        }

        [Test]
        public void NegativeZ()
        {
            CreateSingleVertexPrimitive(new Vertex[] {
                new Vertex(new Vector3(-1, -1, 0), new Vector3(0, 0, -1)),
                new Vertex(new Vector3(-1, 0, 0), new Vector3(0, 0, -1)),
                new Vertex(new Vector3(1, 1, 0), new Vector3(0, 0, -1))
            });
            Assert.AreEqual(new Vector2(0.0f, 0.0f), generatedPrimitive.Vertices[0].UV);
            Assert.AreEqual(new Vector2(0.0f, 0.5f), generatedPrimitive.Vertices[1].UV);
            Assert.AreEqual(new Vector2(1.0f, 1.0f), generatedPrimitive.Vertices[2].UV);
        }

        [Test]
        public void PositiveY()
        {
            CreateSingleVertexPrimitive(new Vertex[] {
                new Vertex(new Vector3(-1, 0, -1), new Vector3(0, 1, 0)),
                new Vertex(new Vector3(-1, 0, 0), new Vector3(0, 1, 0)),
                new Vertex(new Vector3(1, 0, 1), new Vector3(0, 1, 0))
            });
            Assert.AreEqual(new Vector2(0.0f, 0.0f), generatedPrimitive.Vertices[0].UV);
            Assert.AreEqual(new Vector2(0.0f, 0.5f), generatedPrimitive.Vertices[1].UV);
            Assert.AreEqual(new Vector2(1.0f, 1.0f), generatedPrimitive.Vertices[2].UV);
        }

        [Test]
        public void NegativeY()
        {
            CreateSingleVertexPrimitive(new Vertex[] {
                new Vertex(new Vector3(-1, 0, -1), new Vector3(0, -1, 0)),
                new Vertex(new Vector3(-1, 0, 0), new Vector3(0, -1, 0)),
                new Vertex(new Vector3(1, 0, 1), new Vector3(0, -1, 0))
            });
            Assert.AreEqual(new Vector2(0.0f, 0.0f), generatedPrimitive.Vertices[0].UV);
            Assert.AreEqual(new Vector2(0.0f, 0.5f), generatedPrimitive.Vertices[1].UV);
            Assert.AreEqual(new Vector2(1.0f, 1.0f), generatedPrimitive.Vertices[2].UV);
        }

        private void CreateSingleVertexPrimitive(Vertex[] vertices)
        {
            primitive = new Primitive(vertices, null);
            generatedPrimitive = map.Generate();
        }

        public override IPrimitive Generate()
        {
            return primitive;
        }
    }
}
