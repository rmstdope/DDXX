using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class DiscPrimitiveTest
    {
        private DiscPrimitive disc;
        private IPrimitive primitive;

        [Test]
        public void Getters()
        {
            CreateDisc(2, 3, 4);
            Assert.AreEqual(2, disc.InnerRadius);
            Assert.AreEqual(3, disc.Radius);
            Assert.AreEqual(4, disc.Segments);
        }

        [Test]
        public void NumVerticesNoInnerRadius()
        {
            CreateDisc(0, 2, 4);
            Assert.AreEqual(5, primitive.Vertices.Length);
        }

        [Test]
        public void NumVerticesInnerRadius()
        {
            CreateDisc(1, 2, 4);
            Assert.AreEqual(9, primitive.Vertices.Length);
        }

        [Test]
        public void NumIndicesNoInnerRadius()
        {
            CreateDisc(0, 2, 4);
            Assert.AreEqual(12, primitive.Indices.Length);
        }

        [Test]
        public void NumIndicesInnerRadius()
        {
            CreateDisc(1, 2, 4);
            Assert.AreEqual(24, primitive.Indices.Length);
        }

        private void CreateDisc(float innerRadius, float radius, int segments)
        {
            disc = new DiscPrimitive();
            disc.InnerRadius= innerRadius;
            disc.Radius = radius;
            disc.Segments = segments;
            primitive = disc.Generate();
            Assert.IsNull(primitive.Body);
        }
    }
}
