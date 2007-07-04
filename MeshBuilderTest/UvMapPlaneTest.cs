using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class UvMapPlaneTest : IModifier
    {
        private const float epsilon = 0.00001f;
        private Primitive primitive;
        private Primitive mappedPrimitive;
        private UvMapPlane map;

        [SetUp]
        public void SetUp()
        {
            map = new UvMapPlane();
            Assert.AreEqual(1, map.AlignToAxis);
            Assert.AreEqual(1, map.TileV);
            Assert.AreEqual(1, map.TileU);
        }

        [Test]
        public void TestMapXOneVertex()
        {
            CreateVertices(new Vector3[] { new Vector3(0, 0, 0) });
            map.AlignToAxis = 0;
            map.Input = this;
            mappedPrimitive = map.Generate();
            Assert.AreEqual(1, mappedPrimitive.Vertices.Length);
            Assert.AreEqual(0, mappedPrimitive.Vertices[0].U);
            Assert.AreEqual(0, mappedPrimitive.Vertices[0].V);
        }

        [Test]
        public void TestMapXTwoVertices()
        {
            CreateVertices(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 10, 5) });
            map.AlignToAxis = 0;
            map.Input = this;
            mappedPrimitive = map.Generate();
            Assert.AreEqual(2, mappedPrimitive.Vertices.Length);
            Assert.AreEqual(0, mappedPrimitive.Vertices[0].U);
            Assert.AreEqual(0, mappedPrimitive.Vertices[0].V);
            Assert.AreEqual(1, mappedPrimitive.Vertices[1].U);
            Assert.AreEqual(1, mappedPrimitive.Vertices[1].V);
        }

        [Test]
        public void TestMapXMoreVertices()
        {
            CreateVertices(new Vector3[] { new Vector3(0, 9, 1), new Vector3(0, 0, 0), new Vector3(0, 10, 5) });
            map.AlignToAxis = 0;
            map.Input = this;
            mappedPrimitive = map.Generate();
            Assert.AreEqual(3, mappedPrimitive.Vertices.Length);
            Assert.AreEqual(0.9f, mappedPrimitive.Vertices[0].U);
            Assert.AreEqual(0.2f, mappedPrimitive.Vertices[0].V);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].U);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].V);
            Assert.AreEqual(1, mappedPrimitive.Vertices[2].U);
            Assert.AreEqual(1, mappedPrimitive.Vertices[2].V);
        }

        [Test]
        public void TestMapYMoreVertices()
        {
            CreateVertices(new Vector3[] { new Vector3(9, 0, 1), new Vector3(0, 0, 0), new Vector3(10, 0, 5) });
            map.AlignToAxis = 1;
            map.Input = this;
            mappedPrimitive = map.Generate();
            Assert.AreEqual(3, mappedPrimitive.Vertices.Length);
            Assert.AreEqual(0.9f, mappedPrimitive.Vertices[0].U);
            Assert.AreEqual(0.2f, mappedPrimitive.Vertices[0].V);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].U);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].V);
            Assert.AreEqual(1, mappedPrimitive.Vertices[2].U);
            Assert.AreEqual(1, mappedPrimitive.Vertices[2].V);
        }

        [Test]
        public void TestMapZMoreVertices()
        {
            CreateVertices(new Vector3[] { new Vector3(9, 1, 0), new Vector3(0, 0, 0), new Vector3(10, 5, 0) });
            map.AlignToAxis = 2;
            map.Input = this;
            mappedPrimitive = map.Generate();
            Assert.AreEqual(3, mappedPrimitive.Vertices.Length);
            Assert.AreEqual(0.9f, mappedPrimitive.Vertices[0].U);
            Assert.AreEqual(0.2f, mappedPrimitive.Vertices[0].V);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].U);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].V);
            Assert.AreEqual(1, mappedPrimitive.Vertices[2].U);
            Assert.AreEqual(1, mappedPrimitive.Vertices[2].V);
        }

        [Test]
        public void TestMapXAndTile()
        {
            CreateVertices(new Vector3[] { new Vector3(0, 9, 1), new Vector3(0, 0, 0), new Vector3(0, 10, 5) });
            map.AlignToAxis = 0;
            map.Input = this;
            map.TileU = 2;
            map.TileV = 3;
            mappedPrimitive = map.Generate();
            Assert.AreEqual(3, mappedPrimitive.Vertices.Length);
            Assert.AreEqual(1.8f, mappedPrimitive.Vertices[0].U);
            Assert.AreEqual(0.6f, mappedPrimitive.Vertices[0].V);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].U);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].V);
            Assert.AreEqual(2, mappedPrimitive.Vertices[2].U);
            Assert.AreEqual(3, mappedPrimitive.Vertices[2].V);
        }

        [Test]
        public void TestMapYAndTile()
        {
            CreateVertices(new Vector3[] { new Vector3(9, 0, 1), new Vector3(0, 0, 0), new Vector3(10, 0, 5) });
            map.AlignToAxis = 1;
            map.Input = this;
            map.TileU = 3;
            map.TileV = 2;
            mappedPrimitive = map.Generate();
            Assert.AreEqual(3, mappedPrimitive.Vertices.Length);
            Assert.AreEqual(2.7f, mappedPrimitive.Vertices[0].U, epsilon);
            Assert.AreEqual(0.4f, mappedPrimitive.Vertices[0].V);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].U);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].V);
            Assert.AreEqual(3, mappedPrimitive.Vertices[2].U);
            Assert.AreEqual(2, mappedPrimitive.Vertices[2].V);
        }

        [Test]
        public void TestMapZAndTile()
        {
            CreateVertices(new Vector3[] { new Vector3(9, 1, 0), new Vector3(0, 0, 0), new Vector3(10, 5, 0) });
            map.AlignToAxis = 2;
            map.Input = this;
            map.TileU = 0.5f;
            map.TileV = 0.5f;
            mappedPrimitive = map.Generate();
            Assert.AreEqual(3, mappedPrimitive.Vertices.Length);
            Assert.AreEqual(0.45f, mappedPrimitive.Vertices[0].U);
            Assert.AreEqual(0.1f, mappedPrimitive.Vertices[0].V);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].U);
            Assert.AreEqual(0, mappedPrimitive.Vertices[1].V);
            Assert.AreEqual(0.5f, mappedPrimitive.Vertices[2].U);
            Assert.AreEqual(0.5f, mappedPrimitive.Vertices[2].V);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAxisTooLarge()
        {
            map.AlignToAxis = 3;
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAxisTooSmall()
        {
            map.AlignToAxis = -1;
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInputNotSet()
        {
            mappedPrimitive = map.Generate();
        }

        private void CreateVertices(Vector3[] position)
        {
            Vertex[] vertices = new Vertex[position.Length];
            for (int i = 0; i < position.Length; i++)
                vertices[i].Position = position[i];
            primitive = new Primitive(vertices, null);
        }

        #region IModifier Members

        public Primitive Generate()
        {
            return primitive;
        }

        #endregion
    }
}
