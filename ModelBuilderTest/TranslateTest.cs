using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class TranslateTest : IModifier
    {
        private Translate translate;
        private IPrimitive primitive;

        [SetUp]
        public void SetUp()
        {
            primitive = new Primitive(null, null);
        }

        [Test]
        public void Getters()
        {
            CreateVertices(new Vector3[] { new Vector3() });
            Translate(2, 3, 4);
            Assert.AreEqual(2, translate.X);
            Assert.AreEqual(3, translate.Y);
            Assert.AreEqual(4, translate.Z);
        }

        [Test]
        public void TestTranslateOrigo()
        {
            CreateVertices(new Vector3[] { new Vector3() });
            Translate(2, 3, 4);
            VerifyVertices(new Vector3[] { new Vector3(2, 3, 4) });
        }

        [Test]
        public void TestTranslateOneVertex()
        {
            CreateVertices(new Vector3[] { new Vector3(1, 1, 1) });
            Translate(-1, -2, -3);
            VerifyVertices(new Vector3[] { new Vector3(0, -1, -2) });
        }

        [Test]
        public void TestTranslateMoreVertices()
        {
            CreateVertices(new Vector3[] { new Vector3(1, 2, 3), new Vector3(4, 5, 6) });
            Translate(3, 4, 5);
            VerifyVertices(new Vector3[] { new Vector3(4, 6, 8), new Vector3(7, 9, 11) });
        }

        private void VerifyVertices(Vector3[] positions)
        {
            Assert.AreEqual(positions.Length, primitive.Vertices.Length);
            for (int i = 0; i < positions.Length; i++)
                Assert.AreEqual(positions[i], primitive.Vertices[i].Position);
        }

        private void Translate(float x, float y, float z)
        {
            translate = new Translate();
            translate.X = x;
            translate.Y = y;
            translate.Z = z;
            translate.ConnectToInput(0, this);
            primitive = translate.Generate();
            Assert.IsNull(primitive.Indices);
            Assert.IsNull(primitive.Body);
        }

        private void CreateVertices(Vector3[] positions)
        {
            primitive.Vertices = new Vertex[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                primitive.Vertices[i] = new Vertex();
                primitive.Vertices[i].Position = positions[i];
            }
        }

        public void ConnectToInput(int inputPin, IModifier outputGenerator)
        {
        }

        public IPrimitive Generate()
        {
            return primitive;
        }
    }
}
