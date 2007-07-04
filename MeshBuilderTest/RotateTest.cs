using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class RotateTest : IModifier
    {
        private const float epsilon = 0.00001f;
        private Primitive primitive;

        [Test]
        public void TestRotateOrigo()
        {
            CreatePositions(new Vector3[] { new Vector3() });
            Rotate(2, 3, 4);
            VerifyPositions(new Vector3[] { new Vector3() });
        }

        [Test]
        public void TestNoRotation()
        {
            CreatePositions(new Vector3[] { new Vector3(3, 4, 5) });
            Rotate(0, 0, 0);
            VerifyPositions(new Vector3[] { new Vector3(3, 4, 5) });
        }

        [Test]
        public void TestRotateOneVertexX()
        {
            CreatePositions(new Vector3[] { new Vector3(1, 1, 1) });
            Rotate((float)Math.PI, 0, 0);
            VerifyPositions(new Vector3[] { new Vector3(1, -1, -1) });
        }

        [Test]
        public void TestRotateOneVertexY()
        {
            CreatePositions(new Vector3[] { new Vector3(1, 1, 1) });
            Rotate(0, (float)Math.PI, 0);
            VerifyPositions(new Vector3[] { new Vector3(-1, 1, -1) });
        }

        [Test]
        public void TestRotateOneVertexZ()
        {
            CreatePositions(new Vector3[] { new Vector3(1, 1, 1) });
            Rotate(0, 0, (float)Math.PI);
            VerifyPositions(new Vector3[] { new Vector3(-1, -1, 1) });
        }

        [Test]
        public void TestRotateMoreVertices()
        {
            CreatePositions(new Vector3[] { new Vector3(1, 2, 3), new Vector3(4, 5, 6) });
            Rotate((float)Math.PI, 0, 0);
            VerifyPositions(new Vector3[] { new Vector3(1, -2, -3), new Vector3(4, -5, -6) });
        }

        [Test]
        public void TestRotateNormals()
        {
            CreateNormals(new Vector3[] { new Vector3(0, 0, 1), new Vector3(0, 1, 0) });
            Rotate((float)Math.PI, 0, 0);
            VerifyNormals(new Vector3[] { new Vector3(0, 0, -1), new Vector3(0, -1, 0) });
        }

        private void VerifyPositions(Vector3[] positions)
        {
            Assert.IsNull(primitive.Indices);
            Assert.IsNull(primitive.Body);
            Assert.AreEqual(positions.Length, primitive.Vertices.Length);
            for (int i = 0; i < positions.Length; i++)
            {
                Assert.AreEqual(positions[i].X, primitive.Vertices[i].Position.X, epsilon);
                Assert.AreEqual(positions[i].Y, primitive.Vertices[i].Position.Y, epsilon);
                Assert.AreEqual(positions[i].Z, primitive.Vertices[i].Position.Z, epsilon);
            }
        }

        private void VerifyNormals(Vector3[] normals)
        {
            Assert.AreEqual(normals.Length, primitive.Vertices.Length);
            for (int i = 0; i < normals.Length; i++)
            {
                Assert.AreEqual(normals[i].X, primitive.Vertices[i].Normal.X, epsilon);
                Assert.AreEqual(normals[i].Y, primitive.Vertices[i].Normal.Y, epsilon);
                Assert.AreEqual(normals[i].Z, primitive.Vertices[i].Normal.Z, epsilon);
            }
        }

        private void Rotate(float x, float y, float z)
        {
            Rotate Rotate = new Rotate();
            Rotate.X = x;
            Rotate.Y = y;
            Rotate.Z = z;
            Rotate.Input = this;
            primitive = Rotate.Generate();
        }

        private void CreatePositions(Vector3[] positions)
        {
            Vertex[] vertices = new Vertex[positions.Length];
            for (int i = 0; i < positions.Length; i++)
                vertices[i].Position = positions[i];
            primitive = new Primitive(vertices, null);
        }

        private void CreateNormals(Vector3[] normals)
        {
            Vertex[] vertices = new Vertex[normals.Length];
            for (int i = 0; i < normals.Length; i++)
                vertices[i].Normal = normals[i];
            primitive = new Primitive(vertices, null);
        }

        public Primitive Generate()
        {
            return primitive;
        }
    }
}
