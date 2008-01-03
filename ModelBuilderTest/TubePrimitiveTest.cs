using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class TubePrimitiveTest
    {
        private TubePrimitive tube;
        private IPrimitive primitive;

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void OuterRadiusTooSmall()
        {
            // Setup 
            CreateTube(1, 0.99f, 1, 10, 10);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TooFewSegments()
        {
            // Setup 
            CreateTube(1, 2, 1, 2, 10);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TooFewHeightSegments()
        {
            // Setup 
            CreateTube(1, 2, 1, 3, 0);
        }

        [Test]
        public void NumVerticesSmallPrimitive()
        {
            // Setup 
            CreateTube(1, 2, 1, 3, 1);
            // Verify
            Assert.AreEqual(4 * 2 * 2, primitive.Vertices.Length);
        }

        [Test]
        public void NumVerticesLargerPrimitive()
        {
            // Setup 
            CreateTube(1, 2, 1, 4, 2);
            // Verify
            Assert.AreEqual(5 * 3 * 2, primitive.Vertices.Length);
        }

        [Test]
        public void NumIndicesSmallPrimitive()
        {
            // Setup 
            CreateTube(1, 2, 1, 3, 1);
            // Verify
            Assert.AreEqual(6 * 3 * 2 + 6 * 3 * 2, primitive.Indices.Length);
        }

        [Test]
        public void NumIndicesLargerPrimitive()
        {
            // Setup 
            CreateTube(1, 2, 1, 4, 2);
            // Verify
            Assert.AreEqual(6 * 4 * 2 * 2 + 6 * 4 * 2, primitive.Indices.Length);
        }

        [Test]
        public void OuterRadius()
        {
            // Setup
            CreateTube(1.2f, 2.4f, 1, 4, 2);
            // Verify
            for (int i = 0; i < primitive.Vertices.Length / 2; i++)
            {
                Vector3 p = new Vector3(primitive.Vertices[i].Position.X, 0, primitive.Vertices[i].Position.Z);
                Assert.AreEqual(2.4f, p.Length());
            }
        }

        [Test]
        public void InnerRadius()
        {
            // Setup
            CreateTube(1.2f, 2.4f, 1, 4, 2);
            // Verify
            for (int i = primitive.Vertices.Length / 2; i < primitive.Vertices.Length; i++)
            {
                Vector3 p = new Vector3(primitive.Vertices[i].Position.X, 0, primitive.Vertices[i].Position.Z);
                Assert.AreEqual(1.2f, p.Length());
            }
        }

        private void CreateTube(float innerRadius, float outerRadius, float height, int segments, int heightSegments)
        {
            tube = new TubePrimitive();
            tube.InnerRadius = innerRadius;
            tube.OuterRadius = outerRadius;
            tube.Height = height;
            tube.Segments = segments;
            tube.HeightSegments = heightSegments;
            primitive = tube.Generate();
        }

    }
}
