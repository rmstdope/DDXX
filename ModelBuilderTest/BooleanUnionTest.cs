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
    public class BooleanUnionTest
    {
        private BooleanUnion union;

        [SetUp]
        public void SetUp()
        {
            union = new BooleanUnion();
        }

        private class SimplePrimitive : IModifier
        {
            private Vertex[] vertices;
            private short[] indices;
            public SimplePrimitive(Vertex[] vertices, short[] indices)
            {
                this.vertices = vertices;
                this.indices = indices;
            }

            public IPrimitive Generate()
            {
                return new Primitive(vertices, indices);
            }

            public void ConnectToInput(int inputPin, IModifier outputGenerator)
            {
                throw new Exception("The method or operation is not implemented.");
            }

        }

        [Test]
        public void TwoEmptyPrimitives()
        {
            IModifier p = EmptyPrimitive();
            union.ConnectToInput(0, p);
            union.ConnectToInput(1, p);
            IPrimitive primitive = union.Generate();
            Assert.AreEqual(0, primitive.Vertices.Length);
            Assert.AreEqual(0, primitive.Indices.Length);
        }

        private IModifier EmptyPrimitive()
        {
            return new SimplePrimitive(new Vertex[] { }, new short[] { });
        }

        [Test]
        public void EmptyAndSimple()
        {
            IModifier e = EmptyPrimitive();
            IModifier s = new SimplePrimitive(new Vertex[] { RandomVertex() }, new short[] { RandomIndex() });
            union.ConnectToInput(0, e);
            union.ConnectToInput(1, s);
            Assert.AreEqual(s.Generate().Vertices.Length + e.Generate().Vertices.Length,
                union.Generate().Vertices.Length);
            Assert.AreEqual(s.Generate().Indices.Length + e.Generate().Indices.Length,
                union.Generate().Indices.Length);
            Assert.AreEqual(s.Generate().Indices, union.Generate().Indices);
            Assert.AreEqual(s.Generate().Vertices, union.Generate().Vertices);
        }

        private Vertex RandomVertex()
        {
            Vertex vertex = new Vertex();
            vertex.Position = new Vector3(Rand.Float(1), Rand.Float(1), Rand.Float(1));
            vertex.Normal= new Vector3(Rand.Float(1), Rand.Float(1), Rand.Float(1));
            vertex.U = Rand.Float(1);
            vertex.V = Rand.Float(1);
            return vertex;
        }

        private short RandomIndex()
        {
            return (short)Rand.Int(0, short.MaxValue);
        }
    }
}
