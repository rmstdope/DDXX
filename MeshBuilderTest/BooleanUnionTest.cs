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
        private Vertex[] vertices;
        private short[] indices;
        IBody notUsed;

        [SetUp]
        public void SetUp()
        {
            union = new BooleanUnion();
        }

        private class EmptyPrimitive : IPrimitive
        {
            public void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
            {
                vertices = new Vertex[0];
                indices = new short[0];
                body = null;
            }
        }

        private class SimplePrimitive : IPrimitive
        {
            public void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
            {
                vertices = new Vertex[1];
                indices = new short[0];
                body = null;
            }
        }

        [Test]
        public void TwoEmptyPrimitives()
        {
            IPrimitive p = new EmptyPrimitive();
            union.A = p;
            union.B = p;
            union.Generate(out vertices, out indices, out notUsed);
            Assert.AreEqual(0, vertices.Length);
            Assert.AreEqual(0, indices.Length);
        }

        [Test]
        public void EmptyAndSimple()
        {
            IPrimitive e = new EmptyPrimitive();
            IPrimitive s = new SimplePrimitive();
            union.A = e;
            union.B = s;
            Assert.AreEqual(Vertices(s).Length + Vertices(e).Length, Vertices(union).Length);
            Assert.AreEqual(Indices(s).Length + Indices(e).Length, Indices(union).Length);
            Assert.AreEqual(Indices(s), Indices(union));

        }


        private Vertex[] Vertices(IPrimitive p)
        {
            Vertex[] vertices;
            short[] indices;
            p.Generate(out vertices, out indices, out notUsed);
            return vertices;
        }

        private short[] Indices(IPrimitive p)
        {
            Vertex[] vertices;
            short[] indices;
            p.Generate(out vertices, out indices, out notUsed);
            return indices;
        }
    }
}
