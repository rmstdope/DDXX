using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class TranslateTest : IPrimitive
    {
        private Vertex[] vertices;

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
            Assert.AreEqual(positions.Length, vertices.Length);
            for (int i = 0; i < positions.Length; i++)
                Assert.AreEqual(positions[i], vertices[i].Position);
        }

        private void Translate(float x, float y, float z)
        {
            short[] indices;
            IBody body;
            Translate Translate = new Translate();
            Translate.X = x;
            Translate.Y = y;
            Translate.Z = z;
            Translate.Input = this;
            Translate.Generate(out vertices, out indices, out body);
            Assert.IsNull(indices);
            Assert.IsNull(body);
        }

        private void CreateVertices(Vector3[] positions)
        {
            vertices = new Vertex[positions.Length];
            for (int i = 0; i < positions.Length; i++)
                vertices[i].Position = positions[i];
        }

        public void Generate(out Vertex[] vertices, out short[] indices, out Dope.DDXX.Physics.IBody body)
        {
            vertices = this.vertices;
            indices = null;
            body = null;
        }
    }
}
