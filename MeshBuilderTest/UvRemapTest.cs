using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class UvRemapTest : IModifier
    {
        private const float epsilon = 0.00001f;
        private Primitive primitive;

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
            Assert.AreEqual(uv.Length, primitive.Vertices.Length);
            for (int i = 0; i < uv.Length; i++)
            {
                Assert.AreEqual(uv[i].X, primitive.Vertices[i].U, epsilon);
                Assert.AreEqual(uv[i].Y, primitive.Vertices[i].V, epsilon);
            }
        }

        private void UvRemap(float translateU, float scaleU, float translateV, float scaleV)
        {
            UvRemap UvRemap = new UvRemap();
            UvRemap.TranslateU = translateU;
            UvRemap.TranslateV = translateV;
            UvRemap.ScaleU = scaleU;
            UvRemap.ScaleV = scaleV;
            UvRemap.Input = this;
            primitive = UvRemap.Generate();
            Assert.IsNull(primitive.Indices);
            Assert.IsNull(primitive.Body);
        }

        private void CreateVertices(Vector2[] uv)
        {
            Vertex[] vertices = new Vertex[uv.Length];
            for (int i = 0; i < uv.Length; i++)
            {
                vertices[i].U = uv[i].X;
                vertices[i].V = uv[i].Y;
            }
            primitive = new Primitive(vertices, null);
        }

        public Primitive Generate()
        {
            return primitive;
        }
    }
}
