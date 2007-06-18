using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class TorusPrimitiveTest
    {
        private Vertex[] vertices;
        private short[] indices;

        [Test]
        public void TestNumVertices1()
        {
            CreateTorus(2, 4, 3, 3);
            Assert.AreEqual(3 * 3, vertices.Length);
        }

        [Test]
        public void TestNumVertices2()
        {
            CreateTorus(2, 4, 4, 5);
            Assert.AreEqual(4 * 5, vertices.Length);
        }

        [Test]
        public void TestNumIndices1()
        {
            CreateTorus(2, 4, 3, 3);
            Assert.AreEqual(6 * 3 * 3, indices.Length);
        }

        [Test]
        public void TestNumIndices2()
        {
            CreateTorus(2, 4, 4, 5);
            Assert.AreEqual(6 * 4 * 5, indices.Length);
        }

        [Test]
        public void TestSmallRadius()
        {
            CreateTorus(0, 4, 3, 3);
            for (int i = 0; i < vertices.Length; i++)
                Assert.AreEqual(4, vertices[i].Position.Length());
        }

        [Test]
        public void TestLargeRadius()
        {
            CreateTorus(2, 0, 3, 3);
            for (int i = 0; i < vertices.Length; i++)
                Assert.AreEqual(2, vertices[i].Position.Length());
        }

        [Test]
        public void TestIndices()
        {
            short[] correctIndices = 
            {
                0, 1, 4,
                0, 4, 3,
                1, 2, 5,
                1, 5, 4,
                2, 0, 3,
                2, 3, 5,

                3, 4, 7,
                3, 7, 6,
                4, 5, 8,
                4, 8, 7,
                5, 3, 6,
                5, 6, 8,

                6, 7, 1,
                6, 1, 0,
                7, 8, 2,
                7, 2, 1,
                8, 6, 0,
                8, 0, 2,
            };
            CreateTorus(2, 2, 3, 3);
            for (int i = 0; i < indices.Length; i++)
                Assert.AreEqual(correctIndices[i], indices[i]);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestTooFewSegments()
        {
            CreateTorus(2, 4, 2, 3);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestTooFewSides()
        {
            CreateTorus(2, 4, 3, 2);
        }

        private void CreateTorus(int largeRadius, int smallRadius, int segments, int sides)
        {
            IBody body;
            TorusPrimitive torus = new TorusPrimitive();
            torus.LargeRadius = largeRadius;
            torus.SmallRadius = smallRadius;
            torus.Segments = segments;
            torus.Sides = sides;
            torus.Generate(out vertices, out indices, out body);
            Assert.IsNull(body);
        }
    }
}
