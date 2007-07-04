using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class ScaleTest : IModifier
    {
        private Primitive primitive;

        [Test]
        public void TestScaleOrigo()
        {
            CreateVertices(new Vector3[] { new Vector3() });
            Scale(2, 3, 4);
            VerifyVertices(new Vector3[] { new Vector3() });
        }

        [Test]
        public void TestScaleOneVertex()
        {
            CreateVertices(new Vector3[] { new Vector3(1, 1, 1) });
            Scale(2, 3, 4);
            VerifyVertices(new Vector3[] { new Vector3(2, 3, 4) });
        }

        [Test]
        public void TestScaleMoreVertices()
        {
            CreateVertices(new Vector3[] { new Vector3(1, 2, 3), new Vector3(4, 5, 6) });
            Scale(3, 4, 5);
            VerifyVertices(new Vector3[] { new Vector3(3, 8, 15), new Vector3(12, 20, 30) });
        }

        private void VerifyVertices(Vector3[] positions)
        {
            Assert.IsNull(primitive.Indices);
            Assert.IsNull(primitive.Body);
            Assert.AreEqual(positions.Length, primitive.Vertices.Length);
            for (int i = 0; i < positions.Length; i++)
                Assert.AreEqual(positions[i], primitive.Vertices[i].Position);
        }

        private void Scale(float x, float y, float z)
        {
            Scale scale = new Scale();
            scale.X = x;
            scale.Y = y;
            scale.Z = z;
            scale.Input = this;
            primitive = scale.Generate();
        }

        private void CreateVertices(Vector3[] positions)
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
