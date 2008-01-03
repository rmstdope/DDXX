using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class AmplitudeTest
    {
        private Amplitude amplitude;

        [SetUp]
        public void SetUp()
        {
            amplitude = new Amplitude();
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
        public void EmptyPrimitive()
        {
            IModifier m = new SimplePrimitive(new Vertex[] { }, new short[] { });
            amplitude.ConnectToInput(0, m);
            IPrimitive primitive = amplitude.Generate();
            Assert.AreEqual(0, primitive.Vertices.Length);
            Assert.AreEqual(0, primitive.Indices.Length);
        }

        [Test]
        public void DefaultFunction()
        {
            IModifier m = new SimplePrimitive(new Vertex[] {
                new Vertex(new Vector3(1, 2, 3), Vector3.Zero),
                new Vertex(new Vector3(4, 5, 6), Vector3.Zero)}, 
                new short[] { });
            amplitude.ConnectToInput(0, m);
            IPrimitive primitive = amplitude.Generate();
            Assert.AreEqual(new Vector3(1, 2, 3), primitive.Vertices[0].Position);
            Assert.AreEqual(new Vector3(4, 5, 6), primitive.Vertices[1].Position);
        }

        [Test]
        public void NonDefaultFunction()
        {
            IModifier m = new SimplePrimitive(new Vertex[] {
                new Vertex(new Vector3(1, 2, 3), Vector3.Zero),
                new Vertex(new Vector3(4, 5, 6), Vector3.Zero)},
                new short[] { });
            amplitude.ConnectToInput(0, m);
            amplitude.Function = delegate(Vector3 pos) { return new Vector3(2, 4, 8); };
            IPrimitive primitive = amplitude.Generate();
            Assert.AreEqual(new Vector3(2, 8, 24), primitive.Vertices[0].Position);
            Assert.AreEqual(new Vector3(8, 20, 48), primitive.Vertices[1].Position);
        }

    }
}
