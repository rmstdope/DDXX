using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class TranslateTest : IModifier
    {
        private Primitive primitive;

        [Test]
        public void TestTranslateOrigo()
        {
            CreatePrimitive(new Vector3[] { new Vector3() });
            Translate(2, 3, 4);
            VerifyPrimitive(new Vector3[] { new Vector3(2, 3, 4) });
        }

        [Test]
        public void TestTranslateOneVertex()
        {
            CreatePrimitive(new Vector3[] { new Vector3(1, 1, 1) });
            Translate(-1, -2, -3);
            VerifyPrimitive(new Vector3[] { new Vector3(0, -1, -2) });
        }

        [Test]
        public void TestTranslateMoreVertices()
        {
            CreatePrimitive(new Vector3[] { new Vector3(1, 2, 3), new Vector3(4, 5, 6) });
            Translate(3, 4, 5);
            VerifyPrimitive(new Vector3[] { new Vector3(4, 6, 8), new Vector3(7, 9, 11) });
        }

        private void VerifyPrimitive(Vector3[] positions)
        {
            Assert.IsNull(primitive.Indices);
            Assert.IsNull(primitive.Body);
            Assert.AreEqual(positions.Length, primitive.Vertices.Length);
            for (int i = 0; i < positions.Length; i++)
                Assert.AreEqual(positions[i], primitive.Vertices[i].Position);
        }

        private void Translate(float x, float y, float z)
        {
            Translate Translate = new Translate();
            Translate.X = x;
            Translate.Y = y;
            Translate.Z = z;
            Translate.Input = this;
            primitive = Translate.Generate();
        }

        private void CreatePrimitive(Vector3[] positions)
        {
            Vertex[] vertices = new Vertex[positions.Length];
            for (int i = 0; i < positions.Length; i++)
                vertices[i].Position = positions[i];
            primitive = new Primitive(vertices, null);
        }

        public Primitive Generate()
        {
            return primitive;
        }
    }
}
