using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class CylinderPrimitiveTest
    {
        private Primitive primitive;
 
        [Test]
        public void TestNumVertices1()
        {
            CreateCylinder(2, 4, 3, 1);
            Assert.AreEqual(3 * 2 + 2, primitive.Vertices.Length);
        }

        [Test]
        public void TestNumVertices2()
        {
            CreateCylinder(2, 4, 4, 5);
            Assert.AreEqual(4 + 4 + 4 + 4 + 4 + 4 + 2, primitive.Vertices.Length);
        }

        [Test]
        public void TestNumIndices1()
        {
            CreateCylinder(2, 4, 3, 1);
            Assert.AreEqual(3 * 3 * 2 + 6 * 3 * 1, primitive.Indices.Length);
        }

        [Test]
        public void TestNumIndices2()
        {
            CreateCylinder(2, 4, 4, 5);
            Assert.AreEqual(3 * 4 * 2 + 6 * 4 * 5, primitive.Indices.Length);
        }

        [Test]
        public void TestRadius()
        {
            CreateCylinder(4, 0, 10, 10);
            for (int i = 1; i < primitive.Vertices.Length - 1; i++)
                Assert.AreEqual(4, primitive.Vertices[i].Position.Length());
            Assert.AreEqual(new Vector3(), primitive.Vertices[0].Position);
            Assert.AreEqual(new Vector3(), primitive.Vertices[primitive.Vertices.Length - 1].Position);
        }

        [Test]
        public void TestHeight()
        {
            CreateCylinder(0, 3, 3, 2);
            for (int i = 0; i < 4; i++)
                Assert.AreEqual(new Vector3(0, 1.5f, 0), primitive.Vertices[i].Position);
            for (int i = 4; i < 7; i++)
                Assert.AreEqual(new Vector3(0, 0, 0), primitive.Vertices[i].Position);
            for (int i = 7; i < 11; i++)
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
                0, 3, 1,

                // Sides 1
                1, 5, 2,
                1, 4, 5,
                2, 6, 3,
                2, 5, 6,
                3, 4, 1,
                3, 6, 4,

                // Sides 1
                4, 8, 5,
                4, 7, 8,
                5, 9, 6,
                5, 8, 9,
                6, 7, 4,
                6, 9, 7,

                // Bottom
                10, 8, 7,
                10, 9, 8,
                10, 7, 9,
            };
            CreateCylinder(2, 2, 3, 2);
            for (int i = 0; i < primitive.Indices.Length; i++)
                Assert.AreEqual(correctIndices[i], primitive.Indices[i], "Index " + i);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestTooFewSegments()
        {
            CreateCylinder(2, 4, 2, 3);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestTooFewSides()
        {
            CreateCylinder(2, 4, 3, 0);
        }

        private void CreateCylinder(float radius, float height, int segments, int heightSegments)
        {
            CylinderPrimitive cylinder = new CylinderPrimitive();
            cylinder.Radius = radius;
            cylinder.Height = height;
            cylinder.Segments = segments;
            cylinder.HeightSegments = heightSegments;
            primitive = cylinder.Generate();
            Assert.IsNull(primitive.Body);
        }
    }
}
