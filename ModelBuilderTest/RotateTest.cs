using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;
using Dope.DDXX.Physics;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class RotateTest : IModifier
    {
        private const float epsilon = 0.00001f;
        private Rotate rotate;
        private IPrimitive primitive;

        [SetUp]
        public void SetUp()
        {
            primitive = new Primitive(null, null);
        }

        [Test]
        public void Getters()
        {
            CreatePositions(new Vector3[] { new Vector3() });
            Rotate(1, 2, 3);
            Assert.AreEqual(1, rotate.X);
            Assert.AreEqual(2, rotate.Y);
            Assert.AreEqual(3, rotate.Z);
        }

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
            rotate = new Rotate();
            rotate.X = x;
            rotate.Y = y;
            rotate.Z = z;
            rotate.ConnectToInput(0,this);
            primitive = rotate.Generate();
            Assert.IsNull(primitive.Indices);
            Assert.IsNull(primitive.Body);
        }

        private void CreatePositions(Vector3[] positions)
        {
            primitive.Vertices = new Vertex[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                primitive.Vertices[i] = new Vertex();
                primitive.Vertices[i].Position = positions[i];
            }
        }

        private void CreateNormals(Vector3[] normals)
        {
            primitive.Vertices= new Vertex[normals.Length];
            for (int i = 0; i < normals.Length; i++)
            {
                primitive.Vertices[i] = new Vertex();
                primitive.Vertices[i].Normal = normals[i];
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
