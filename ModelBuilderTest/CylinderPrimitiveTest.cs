using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class CylinderPrimitiveTest
    {
        private CylinderPrimitive cylinder;
        private IPrimitive primitive;
        private const float epsilon = 0.000001f;

        [Test]
        public void Getters()
        {
            CreateCylinder(1, 2, 3, 4, false);
            Assert.AreEqual(1, cylinder.Radius);
            Assert.AreEqual(2, cylinder.Height);
            Assert.AreEqual(3, cylinder.Segments);
            Assert.AreEqual(4, cylinder.HeightSegments);
            Assert.IsFalse(cylinder.Lid);
        }

        [Test]
        public void TestNumVertices1()
        {
            CreateCylinder(2, 4, 3, 1, true);
            Assert.AreEqual(4 * 2 + 2, primitive.Vertices.Length);
        }

        [Test]
        public void TestNumVertices2()
        {
            CreateCylinder(2, 4, 4, 5, true);
            Assert.AreEqual(5 + 5 + 5 + 5 + 5 + 5 + 2, primitive.Vertices.Length);
        }

        [Test]
        public void TestNumVerticesNoLid1()
        {
            CreateCylinder(2, 4, 3, 1, false);
            Assert.AreEqual(4 * 2, primitive.Vertices.Length);
        }

        [Test]
        public void TestNumVerticesNoLid2()
        {
            CreateCylinder(2, 4, 4, 5, false);
            Assert.AreEqual(5 + 5 + 5 + 5 + 5 + 5, primitive.Vertices.Length);
        }

        [Test]
        public void TestNumIndices1()
        {
            CreateCylinder(2, 4, 3, 1, true);
            Assert.AreEqual(3 * 3 * 2 + 6 * 3 * 1, primitive.Indices.Length);
        }

        [Test]
        public void TestNumIndices2()
        {
            CreateCylinder(2, 4, 4, 5, true);
            Assert.AreEqual(3 * 4 * 2 + 6 * 4 * 5, primitive.Indices.Length);
        }

        [Test]
        public void TestNumIndicesNoLid1()
        {
            CreateCylinder(2, 4, 3, 1, false);
            Assert.AreEqual(6 * 3 * 1, primitive.Indices.Length);
        }

        [Test]
        public void TestNumIndicesNoLid2()
        {
            CreateCylinder(2, 4, 4, 5, false);
            Assert.AreEqual(6 * 4 * 5, primitive.Indices.Length);
        }

        [Test]
        public void TestRadius()
        {
            CreateCylinder(4, 0, 10, 10, true);
            for (int i = 1; i < primitive.Vertices.Length - 1; i++)
                Assert.AreEqual(4, primitive.Vertices[i].Position.Length(), epsilon);
            Assert.AreEqual(new Vector3(), primitive.Vertices[0].Position);
            Assert.AreEqual(new Vector3(), primitive.Vertices[primitive.Vertices.Length - 1].Position);
        }

        [Test]
        public void TestHeight()
        {
            CreateCylinder(0, 3, 3, 2, true);
            for (int i = 0; i < 5; i++)
                Assert.AreEqual(new Vector3(0, 1.5f, 0), primitive.Vertices[i].Position);
            for (int i = 5; i < 9; i++)
                Assert.AreEqual(new Vector3(0, 0, 0), primitive.Vertices[i].Position);
            for (int i = 9; i < 14; i++)
                Assert.AreEqual(new Vector3(0, -1.5f, 0), primitive.Vertices[i].Position);
        }

        [Test]
        public void TestIndices()
        {
            short[] correctIndices = 
            {
                // Top
                0, 1, 2,
                0, 2, 3,
                0, 3, 4,

                // Sides 1
                1, 6, 2,
                1, 5, 6,
                2, 7, 3,
                2, 6, 7,
                3, 8, 4,
                3, 7, 8,

                // Sides 1
                5, 10,  6,
                5,  9, 10,
                6, 11,  7,
                6, 10, 11,
                7, 12,  8,
                7, 11, 12,

                // Bottom
                13, 10,  9,
                13, 11, 10,
                13, 12, 11,
            };
            CreateCylinder(2, 2, 3, 2, true);
            for (int i = 0; i < primitive.Indices.Length; i++)
                Assert.AreEqual(correctIndices[i], primitive.Indices[i], "Index " + i);
        }

        [Test]
        public void TestIndicesNoLid()
        {
            short[] correctIndices = 
            {
                // Sides 1
                0, 5, 1,
                0, 4, 5,
                1, 6, 2,
                1, 5, 6,
                2, 7, 3,
                2, 6, 7,

                // Sides 1
                4,  9,  5,
                4,  8,  9,
                5, 10,  6,
                5,  9, 10,
                6, 11,  7,
                6, 10, 11,
            };
            CreateCylinder(2, 2, 3, 2, false);
            for (int i = 0; i < primitive.Indices.Length; i++)
                Assert.AreEqual(correctIndices[i], primitive.Indices[i], "Index " + i);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestTooFewSegments()
        {
            CreateCylinder(2, 4, 2, 3, true);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestTooFewSides()
        {
            CreateCylinder(2, 4, 3, 0, true);
        }

        private void CreateCylinder(float radius, float height, int segments, int heightSegments, bool lid)
        {
            cylinder = new CylinderPrimitive();
            cylinder.Radius = radius;
            cylinder.Height = height;
            cylinder.Segments = segments;
            cylinder.HeightSegments = heightSegments;
            cylinder.Lid = lid;
            primitive = cylinder.Generate();
            Assert.IsNull(primitive.Body);
        }
    }
}
