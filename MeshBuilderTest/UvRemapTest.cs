using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class UvRemapTest : IPrimitive
    {
        private const float epsilon = 0.00001f;
        private Vertex[] vertices;

        [Test]
        public void TestScale()
        {
            CreateVertices(new Vector2[] { new Vector2(), new Vector2(0.1f, 0.2f) });
            UvRemap(0, 2, 0, 2);
            VerifyVertices(new Vector2[] { new Vector2(), new Vector2(0.2f, 0.4f) });
        }

        [Test]
        public void TestTranslate()
        {
            CreateVertices(new Vector2[] { new Vector2(), new Vector2(0.3f, 0.4f) });
            UvRemap(0.2f, 1, 0.3f, 1);
            VerifyVertices(new Vector2[] { new Vector2(0.2f, 0.3f), new Vector2(0.5f, 0.7f) });
        }

        [Test]
        public void TestScaleAndTranslate()
        {
            CreateVertices(new Vector2[] { new Vector2(), new Vector2(0.3f, 0.4f) });
            UvRemap(0.2f, 2, 0.3f, 3);
            VerifyVertices(new Vector2[] { new Vector2(0.2f, 0.3f), new Vector2(0.8f, 1.5f) });
        }

        private void VerifyVertices(Vector2[] uv)
        {
            Assert.AreEqual(uv.Length, vertices.Length);
            for (int i = 0; i < uv.Length; i++)
            {
                Assert.AreEqual(uv[i].X, vertices[i].U, epsilon);
                Assert.AreEqual(uv[i].Y, vertices[i].V, epsilon);
            }
        }

        private void UvRemap(float translateU, float scaleU, float translateV, float scaleV)
        {
            short[] indices;
            IBody body;
            UvRemap UvRemap = new UvRemap();
            UvRemap.TranslateU = translateU;
            UvRemap.TranslateV = translateV;
            UvRemap.ScaleU = scaleU;
            UvRemap.ScaleV = scaleV;
            UvRemap.Input = this;
            UvRemap.Generate(out vertices, out indices, out body);
            Assert.IsNull(indices);
            Assert.IsNull(body);
        }

        private void CreateVertices(Vector2[] uv)
        {
            vertices = new Vertex[uv.Length];
            for (int i = 0; i < uv.Length; i++)
            {
                vertices[i].U = uv[i].X;
                vertices[i].V = uv[i].Y;
            }
        }

        public void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
        {
            vertices = this.vertices;
            indices = null;
            body = null;
        }
    }
}
