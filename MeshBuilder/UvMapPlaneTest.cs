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
    public class UvMapPlaneTest : IPrimitive
    {
        private const float epsilon = 0.00001f;
        private Vertex[] vertices;
        private Vertex[] mappedVertices;
        private short[] mappedIndices;
        private IBody mappedBody;
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
            map.Generate(out mappedVertices, out mappedIndices, out mappedBody);
            Assert.AreEqual(1, mappedVertices.Length);
            Assert.AreEqual(0, mappedVertices[0].U);
            Assert.AreEqual(0, mappedVertices[0].V);
        }

        [Test]
        public void TestMapXTwoVertices()
        {
            CreateVertices(new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 10, 5) });
            map.AlignToAxis = 0;
            map.Input = this;
            map.Generate(out mappedVertices, out mappedIndices, out mappedBody);
            Assert.AreEqual(2, mappedVertices.Length);
            Assert.AreEqual(0, mappedVertices[0].U);
            Assert.AreEqual(0, mappedVertices[0].V);
            Assert.AreEqual(1, mappedVertices[1].U);
            Assert.AreEqual(1, mappedVertices[1].V);
        }

        [Test]
        public void TestMapXMoreVertices()
        {
            CreateVertices(new Vector3[] { new Vector3(0, 9, 1), new Vector3(0, 0, 0), new Vector3(0, 10, 5) });
            map.AlignToAxis = 0;
            map.Input = this;
            map.Generate(out mappedVertices, out mappedIndices, out mappedBody);
            Assert.AreEqual(3, mappedVertices.Length);
            Assert.AreEqual(0.9f, mappedVertices[0].U);
            Assert.AreEqual(0.2f, mappedVertices[0].V);
            Assert.AreEqual(0, mappedVertices[1].U);
            Assert.AreEqual(0, mappedVertices[1].V);
            Assert.AreEqual(1, mappedVertices[2].U);
            Assert.AreEqual(1, mappedVertices[2].V);
        }

        [Test]
        public void TestMapYMoreVertices()
        {
            CreateVertices(new Vector3[] { new Vector3(9, 0, 1), new Vector3(0, 0, 0), new Vector3(10, 0, 5) });
            map.AlignToAxis = 1;
            map.Input = this;
            map.Generate(out mappedVertices, out mappedIndices, out mappedBody);
            Assert.AreEqual(3, mappedVertices.Length);
            Assert.AreEqual(0.9f, mappedVertices[0].U);
            Assert.AreEqual(0.2f, mappedVertices[0].V);
            Assert.AreEqual(0, mappedVertices[1].U);
            Assert.AreEqual(0, mappedVertices[1].V);
            Assert.AreEqual(1, mappedVertices[2].U);
            Assert.AreEqual(1, mappedVertices[2].V);
        }

        [Test]
        public void TestMapZMoreVertices()
        {
            CreateVertices(new Vector3[] { new Vector3(9, 1, 0), new Vector3(0, 0, 0), new Vector3(10, 5, 0) });
            map.AlignToAxis = 2;
            map.Input = this;
            map.Generate(out mappedVertices, out mappedIndices, out mappedBody);
            Assert.AreEqual(3, mappedVertices.Length);
            Assert.AreEqual(0.9f, mappedVertices[0].U);
            Assert.AreEqual(0.2f, mappedVertices[0].V);
            Assert.AreEqual(0, mappedVertices[1].U);
            Assert.AreEqual(0, mappedVertices[1].V);
            Assert.AreEqual(1, mappedVertices[2].U);
            Assert.AreEqual(1, mappedVertices[2].V);
        }

        [Test]
        public void TestMapXAndTile()
        {
            CreateVertices(new Vector3[] { new Vector3(0, 9, 1), new Vector3(0, 0, 0), new Vector3(0, 10, 5) });
            map.AlignToAxis = 0;
            map.Input = this;
            map.TileU = 2;
            map.TileV = 3;
            map.Generate(out mappedVertices, out mappedIndices, out mappedBody);
            Assert.AreEqual(3, mappedVertices.Length);
            Assert.AreEqual(1.8f, mappedVertices[0].U);
            Assert.AreEqual(0.6f, mappedVertices[0].V);
            Assert.AreEqual(0, mappedVertices[1].U);
            Assert.AreEqual(0, mappedVertices[1].V);
            Assert.AreEqual(2, mappedVertices[2].U);
            Assert.AreEqual(3, mappedVertices[2].V);
        }

        [Test]
        public void TestMapYAndTile()
        {
            CreateVertices(new Vector3[] { new Vector3(9, 0, 1), new Vector3(0, 0, 0), new Vector3(10, 0, 5) });
            map.AlignToAxis = 1;
            map.Input = this;
            map.TileU = 3;
            map.TileV = 2;
            map.Generate(out mappedVertices, out mappedIndices, out mappedBody);
            Assert.AreEqual(3, mappedVertices.Length);
            Assert.AreEqual(2.7f, mappedVertices[0].U, epsilon);
            Assert.AreEqual(0.4f, mappedVertices[0].V);
            Assert.AreEqual(0, mappedVertices[1].U);
            Assert.AreEqual(0, mappedVertices[1].V);
            Assert.AreEqual(3, mappedVertices[2].U);
            Assert.AreEqual(2, mappedVertices[2].V);
        }

        [Test]
        public void TestMapZAndTile()
        {
            CreateVertices(new Vector3[] { new Vector3(9, 1, 0), new Vector3(0, 0, 0), new Vector3(10, 5, 0) });
            map.AlignToAxis = 2;
            map.Input = this;
            map.TileU = 0.5f;
            map.TileV = 0.5f;
            map.Generate(out mappedVertices, out mappedIndices, out mappedBody);
            Assert.AreEqual(3, mappedVertices.Length);
            Assert.AreEqual(0.45f, mappedVertices[0].U);
            Assert.AreEqual(0.1f, mappedVertices[0].V);
            Assert.AreEqual(0, mappedVertices[1].U);
            Assert.AreEqual(0, mappedVertices[1].V);
            Assert.AreEqual(0.5f, mappedVertices[2].U);
            Assert.AreEqual(0.5f, mappedVertices[2].V);
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
            map.Generate(out mappedVertices, out mappedIndices, out mappedBody);
        }

        private void CreateVertices(Vector3[] position)
        {
            vertices = new Vertex[position.Length];
            for (int i = 0; i < position.Length; i++)
                vertices[i].Position = position[i];
        }

        #region IPrimitive Members

        public void Generate(out Vertex[] vertices, out short[] indices, out Dope.DDXX.Physics.IBody body)
        {
            vertices = this.vertices;
            indices = null;
            body = null;
        }

        #endregion
    }
}
