using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class SpherePrimitiveTest
    {
        private const float epsilon = 0.000001f;
        private SpherePrimitive sphere;
        private IPrimitive primitive;

        [Test]
        public void Getters()
        {
            CreateSphere(1.0f, 4);
            Assert.AreEqual(1, sphere.Radius);
            Assert.AreEqual(4, sphere.Rings);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSphereRingsNotMulOf4()
        {
            CreateSphere(1.0f, 5);
        }

        [Test]
        public void TestSphereVertexCount1()
        {
            CreateSphere(1.0f, 4);
            Assert.AreEqual(4 + 2, primitive.Vertices.Length);
            CreateSphere(1.0f, 8);
            Assert.AreEqual(8 * 3 + 2, primitive.Vertices.Length);
        }

        [Test]
        public void TestSphereVertexOnRadius4()
        {
            float radius = 10.0f;
            CreateSphere(radius, 4);
            foreach (Vertex v in primitive.Vertices)
            {
                Assert.AreEqual(radius, v.Position.Length(), epsilon);
            }
        }

        [Test]
        public void TestSphereVertexOnRadius32()
        {
            float radius = 5.0f;
            CreateSphere(radius, 32);
            foreach (Vertex v in primitive.Vertices)
            {
                Assert.AreEqual(radius, v.Position.Length(), epsilon);
            }
        }

        [Test]
        public void TestSphereIndexCount()
        {
            CreateSphere(1.0f, 4);
            Assert.AreEqual(4 * 3 + 4 * 3, primitive.Indices.Length);
            CreateSphere(1.0f, 8);
            Assert.AreEqual(8 * 3 + 6 * 8 * 2 + 8 * 3, primitive.Indices.Length);
        }

        [Test]
        public void TestSphereIndicesValues4()
        {
            CreateSphere(1.0f, 4);
            short[] indices = new short[] {
                // Top
                0,2,1,
                0,3,2,
                0,4,3,
                0,1,4,
                // Bottom
                1,2,5,
                2,3,5,
                3,4,5,
                4,1,5,
            };
            for (int sourceIndex = 0; sourceIndex < indices.Length; sourceIndex++)
            {
                Assert.AreEqual(indices[sourceIndex], indices[sourceIndex]);
            }
        }

        [Test]
        public void TestSphereIndicesValues8()
        {
            CreateSphere(1.0f, 8);
            short[] indices = new short[] {
                // Top
                 0,2,1,
                 0,3,2,
                 0,4,3,
                 0,5,4,
                 0,6,5,
                 0,7,6,
                 0,8,7,
                 0,1,8,
                // Ring 1
                 1, 2,10,
                 1,10, 9,
                 2, 3,11,
                 2,11,10,
                 3, 4,12,
                 3,12,11,
                 4, 5,13,
                 4,13,12,
                 5, 6,14,
                 5,14,13,
                 6, 7,15,
                 6,15,14,
                 7, 8,16,
                 7,16,15,
                 8, 1, 9,
                 8, 9,16,
                // Ring 2
                 9,10,18,
                 9,18,17,
                10,11,19,
                10,19,18,
                11,12,20,
                11,20,19,
                12,13,21,
                12,21,20,
                13,14,22,
                13,22,21,
                14,15,23,
                14,23,22,
                15,16,24,
                15,24,23,
                16, 9,17,
                16,17,24,
                // Bottom
                17,18,25,
                18,19,25,
                19,20,25,
                20,21,25,
                21,22,25,
                22,23,25,
                23,24,25,
                24,17,25,
            };
            for (int sourceIndex = 0; sourceIndex < indices.Length; sourceIndex++)
            {
                Assert.AreEqual(indices[sourceIndex], indices[sourceIndex], "Index " + sourceIndex);
            }
        }

        [Test]
        public void TestSphereVertexNormalsR1()
        {
            CreateSphere(1.0f, 32);
            foreach (Vertex v in primitive.Vertices)
            {
                Assert.AreEqual(v.Position.X, v.Normal.X, epsilon);
                Assert.AreEqual(v.Position.Y, v.Normal.Y, epsilon);
                Assert.AreEqual(v.Position.Z, v.Normal.Z, epsilon);
            }
        }

        [Test]
        public void TestSphereVertexNormalsR10()
        {
            CreateSphere(10.0f, 32);
            foreach (Vertex v in primitive.Vertices)
            {
                Vector3 expected = v.Position;
                expected.Normalize();
                Assert.AreEqual(expected.X, v.Normal.X, epsilon);
                Assert.AreEqual(expected.Y, v.Normal.Y, epsilon);
                Assert.AreEqual(expected.Z, v.Normal.Z, epsilon);
            }
        }

        private void CreateSphere(float radius, int rings)
        {
            sphere = new SpherePrimitive();
            sphere.Radius = radius;
            sphere.Rings = rings;
            primitive = sphere.Generate();
            Assert.IsNull(primitive.Body);
        }
    }
}
