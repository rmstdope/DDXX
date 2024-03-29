using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class TorusPrimitiveTest
    {
        private TorusPrimitive torus;
        private IPrimitive primitive;
        private const float epsilon = 0.000001f;

        [Test]
        public void Getters()
        {
            CreateTorus(2, 4, 3, 4);
            Assert.AreEqual(2, torus.LargeRadius);
            Assert.AreEqual(4, torus.SmallRadius);
            Assert.AreEqual(3, torus.Segments);
            Assert.AreEqual(4, torus.Sides);
        }

        [Test]
        public void TestNumVertices1()
        {
            CreateTorus(2, 4, 3, 3);
            Assert.AreEqual(3 * 3, primitive.Vertices.Length);
        }

        [Test]
        public void TestNumVertices2()
        {
            CreateTorus(2, 4, 4, 5);
            Assert.AreEqual(4 * 5, primitive.Vertices.Length);
        }

        [Test]
        public void TestNumIndices1()
        {
            CreateTorus(2, 4, 3, 3);
            Assert.AreEqual(6 * 3 * 3, primitive.Indices.Length);
        }

        [Test]
        public void TestNumIndices2()
        {
            CreateTorus(2, 4, 4, 5);
            Assert.AreEqual(6 * 4 * 5, primitive.Indices.Length);
        }

        [Test]
        public void TestSmallRadius()
        {
            CreateTorus(0, 4, 3, 3);
            for (int i = 0; i < primitive.Vertices.Length; i++)
                Assert.AreEqual(4.0f, primitive.Vertices[i].Position.Length(), epsilon);
        }

        [Test]
        public void TestLargeRadius()
        {
            CreateTorus(2, 0, 3, 3);
            for (int i = 0; i < primitive.Vertices.Length; i++)
                Assert.AreEqual(2, primitive.Vertices[i].Position.Length());
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
            for (int i = 0; i < primitive.Indices.Length; i++)
                Assert.AreEqual(correctIndices[i], primitive.Indices[i]);
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
            torus = new TorusPrimitive();
            torus.LargeRadius = largeRadius;
            torus.SmallRadius = smallRadius;
            torus.Segments = segments;
            torus.Sides = sides;
            primitive = torus.Generate();
            Assert.IsNull(primitive.Body);
        }
    }
}
