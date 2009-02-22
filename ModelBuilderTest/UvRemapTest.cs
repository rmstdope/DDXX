using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class UvRemapTest : IModifier
    {
        private const float epsilon = 0.00001f;
        private UvRemap uvRemap;
        private IPrimitive primitive;

        [Test]
        public void Getters()
        {
            CreateVertices(new Vector2[] { new Vector2() });
            UvRemap(1, 2, 3, 4);
            Assert.AreEqual(1, uvRemap.TranslateU);
            Assert.AreEqual(2, uvRemap.ScaleU);
            Assert.AreEqual(3, uvRemap.TranslateV);
            Assert.AreEqual(4, uvRemap.ScaleV);
        }

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
            uvRemap = new UvRemap();
            uvRemap.TranslateU = translateU;
            uvRemap.TranslateV = translateV;
            uvRemap.ScaleU = scaleU;
            uvRemap.ScaleV = scaleV;
            uvRemap.ConnectToInput(0, this);
            primitive = uvRemap.Generate();
            Assert.IsNull(primitive.Indices);
            Assert.IsNull(primitive.Body);
        }

        private void CreateVertices(Vector2[] uv)
        {
            Vertex[] vertices = new Vertex[uv.Length];
            for (int i = 0; i < uv.Length; i++)
            {
                vertices[i] = new Vertex();
                vertices[i].U = uv[i].X;
                vertices[i].V = uv[i].Y;
            }
            primitive = new Primitive(vertices, null);
        }

        public IPrimitive Generate()
        {
            return primitive;
        }

        #region IPrimitive Members

        public void ConnectToInput(int inputPin, IModifier outputGenerator)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IModifier Members


        public IModifier GetInputModifier(int index)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
