using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class BooleanUnionTest
    {
        private BooleanUnion union;

        [SetUp]
        public void SetUp()
        {
            union = new BooleanUnion();
        }

        private class EmptyPrimitive : IModifier
        {
            public Primitive Generate()
            {
                return new Primitive(new Vertex[0], new short[0]);
            }
        }

        private class SimplePrimitive : IModifier
        {
            public Primitive Generate()
            {
                return new Primitive(new Vertex[1], new short[0]);
            }
        }

        [Test]
        public void TwoEmptyPrimitives()
        {
            IModifier p = new EmptyPrimitive();
            union.A = p;
            union.B = p;
            Primitive primitive = union.Generate();
            Assert.AreEqual(0, primitive.Vertices.Length);
            Assert.AreEqual(0, primitive.Indices.Length);
        }

        [Test]
        public void EmptyAndSimple()
        {
            IModifier e = new EmptyPrimitive();
            IModifier s = new SimplePrimitive();
            union.A = e;
            union.B = s;
            Assert.AreEqual(Vertices(s).Length + Vertices(e).Length, Vertices(union).Length);
            Assert.AreEqual(Indices(s).Length + Indices(e).Length, Indices(union).Length);
            Assert.AreEqual(Indices(s), Indices(union));

        }


        private Vertex[] Vertices(IModifier p)
        {
            return p.Generate().Vertices;
        }

        private short[] Indices(IModifier p)
        {
            return p.Generate().Indices;
        }
    }
}
