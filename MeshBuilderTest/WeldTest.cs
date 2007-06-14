using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class WeldTest : IPrimitive
    {
        private Weld weld;
        private Vertex[] vertices;
        private short[] indices;
        private Vertex[] weldedVertices;
        private short[] weldedIndices;

        [SetUp]
        public void SetUp()
        {
            weld = new Weld();
            weld.Input = this;
        }

        /// <summary>
        /// Test welding two verices with same position.
        /// Check only positions.
        /// </summary>
        [Test]
        public void TestWeldingSamePosition()
        {
            Vector3[] position = new Vector3[] { new Vector3(), new Vector3(1e-10f, 0, 0) };
            Vector3[] newPositions = new Vector3[] { new Vector3() };
            CreatePrimitiveFromLists(position, null, null, null);
            Weld(0.0f);
            CompareVertices(newPositions, null, null);
        }

        /// <summary>
        /// Test welding of same position with more vertices
        /// </summary>
        [Test]
        public void TestWeldingSamePositionMoreVertices()
        {
            Vector3[] positions = new Vector3[] { new Vector3(0.1f, 0, 0), new Vector3(), new Vector3(0.3f, 0, 0), new Vector3() };
            Vector3[] newPositions = new Vector3[] { new Vector3(0.1f, 0, 0), new Vector3(), new Vector3(0.3f, 0, 0) };
            CreatePrimitiveFromLists(positions, null, null, null);
            Weld(0.0f);
            CompareVertices(newPositions, null, null);
        }

        /// <summary>
        /// Test welding vertices with different normals (should be averaged).
        /// </summary>
        [Test]
        public void TestWeldingDifferentNormals()
        {
            Vector3[] positions = new Vector3[] { new Vector3(), new Vector3() };
            Vector3[] newPositions = new Vector3[] { new Vector3() };
            Vector3[] normals = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 1, 0) };
            Vector3[] newNormals = new Vector3[] { new Vector3(1, 1, 0) };
            CreatePrimitiveFromLists(positions, normals, null, null);
            Weld(0.0f);
            CompareVertices(newPositions, newNormals, null);
        }

        /// <summary>
        /// Test welding vertices with different normals (should be averaged).
        /// </summary>
        [Test]
        public void TestWeldingDifferentUV()
        {
            Vector3[] positions = new Vector3[] { new Vector3(), new Vector3() };
            Vector3[] newPositions = new Vector3[] { new Vector3() };
            Vector2[] uv = new Vector2[] { new Vector2(10, 20), new Vector2(30, 20) };
            Vector2[] newUV = new Vector2[] { new Vector2(20, 20) };
            CreatePrimitiveFromLists(positions, null, uv, null);
            Weld(0.0f);
            CompareVertices(newPositions, null, newUV);
        }

        /// <summary>
        /// Test welding two vertices that are close enough but not on the same position.
        /// </summary>
        [Test]
        public void TestWeldingDifferentPositions()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(0.0f, 0, 0), new Vector3(0.5f, 0, 0), 
                new Vector3(0.3f, 0, 0), new Vector3(0.1f, 0, 0) };
            Vector3[] newPositions = new Vector3[] { 
                new Vector3(0.0f, 0, 0), new Vector3(0.5f, 0, 0), 
                new Vector3(0.3f, 0, 0) };
            CreatePrimitiveFromLists(positions, null, null, null);
            Weld(0.1f);
            CompareVertices(newPositions, null, null);
        }

        /// <summary>
        /// Test that indices are changed when welding.
        /// </summary>
        [Test]
        public void TestWeldingIndices1()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(), new Vector3() , new Vector3(1, 1, 1), new Vector3(2, 2, 2) };
            short[] indices = new short[] { 1, 2, 3 };
            short[] newIndices = new short[] { 0, 1, 2 };
            CreatePrimitiveFromLists(positions, null, null, indices);
            Weld(0.0f);
            CompareIndices(newIndices);
        }

        /// <summary>
        /// Test that more indices are changed when welding.
        /// </summary>
        [Test]
        public void TestWeldingIndices2()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(), new Vector3() , new Vector3(1, 1, 1), new Vector3(2, 2, 2) };
            short[] indices = new short[] { 0, 2, 3, 3, 2, 1 };
            short[] newIndices = new short[] { 0, 1, 2, 2, 1, 0 };
            CreatePrimitiveFromLists(positions, null, null, indices);
            Weld(0.0f);
            CompareIndices(newIndices);
        }

        /// <summary>
        /// Test that welding removes triangles with two vertices equals.
        /// </summary>
        [Test]
        public void TestWeldingRemoveTrianglesSameVertices()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(), new Vector3() , new Vector3(1, 1, 1), new Vector3(2, 2, 2) };
            short[] indices = new short[] { 0, 1, 2 };
            short[] newIndices = new short[] { };
            CreatePrimitiveFromLists(positions, null, null, indices);
            Weld(0.0f);
            CompareIndices(newIndices);
        }

        /// <summary>
        /// Test that welding removes triangles with two vertices equals.
        /// </summary>
        [Test]
        public void TestWeldingRemoveTrianglesSameVertices2()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(), new Vector3() , new Vector3(1, 1, 1), new Vector3(2, 2, 2) };
            short[] indices = new short[] { 0, 0, 0, 0, 1, 2, 1, 2, 3, 2, 2, 2, 3, 3, 3, 1, 1, 1 };
            short[] newIndices = new short[] { 0, 1, 2 };
            CreatePrimitiveFromLists(positions, null, null, indices);
            Weld(0.0f);
            CompareIndices(newIndices);
        }

        /// <summary>
        /// Test that welding remove duplicated triangles.
        /// </summary>
        [Test]
        public void TestWeldingRemoveDuplicateTriangles()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(1,1,1), new Vector3(2,2,2) , new Vector3(3,3,3) };
            short[] indices = new short[] { 0, 1, 2, 0, 1, 2 };
            short[] newIndices = new short[] { 0, 1, 2 };
            CreatePrimitiveFromLists(positions, null, null, indices);
            Weld(0.0f);
            CompareIndices(newIndices);
        }

        /// <summary>
        /// Test that welding remove duplicated triangles even if permutated.
        /// We must make sure that culling is honored though!
        /// </summary>
        [Test]
        public void TestWeldingRemoveDuplicateTriangles2()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(1,1,1), new Vector3(2,2,2) , new Vector3(3,3,3) };
            short[] indices = new short[] { 0, 1, 2, 0, 1, 2, 1, 2, 0, 2, 0, 1, 2, 1, 0 };
            short[] newIndices = new short[] { 0, 1, 2, 2, 1, 0 };
            CreatePrimitiveFromLists(positions, null, null, indices);
            Weld(0.0f);
            CompareIndices(newIndices);
        }

        private void Weld(float distance)
        {
            IBody body;
            weld.Distance = distance;
            weld.Generate(out weldedVertices, out weldedIndices, out body);
            Assert.IsNull(body);
        }

        private void CreatePrimitiveFromLists(Vector3[] positions,
            Vector3[] normals, Vector2[] uv, short[] indices)
        {
            vertices = new Vertex[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                vertices[i].Position = positions[i];
                if (normals != null)
                {
                    normals[i].Normalize();
                    vertices[i].Normal = normals[i];
                }
                if (uv != null)
                {
                    vertices[i].U = uv[i].X;
                    vertices[i].V = uv[i].Y;
                }
            }
            if (indices == null)
                indices = new short[] { };
            this.indices = indices;
        }

        private void CompareVertices(Vector3[] newPositions, Vector3[] newNormals, Vector2[] newUV)
        {
            Assert.AreEqual(newPositions.Length, weldedVertices.Length);
            for (int i = 0; i < newPositions.Length; i++)
            {
                Assert.AreEqual(newPositions[i], weldedVertices[i].Position);
                if (newNormals != null)
                {
                    newNormals[i].Normalize();
                    Assert.AreEqual(newNormals[i], weldedVertices[i].Normal);
                }
                if (newUV != null)
                {
                    Assert.AreEqual(newUV[i].X, weldedVertices[i].U);
                    Assert.AreEqual(newUV[i].Y, weldedVertices[i].V);
                }
            }
        }

        private void CompareIndices(short[] newIndices)
        {
            Assert.AreEqual(newIndices.Length, weldedIndices.Length);
            for (int i = 0; i < newIndices.Length; i++)
                Assert.AreEqual(newIndices[i], weldedIndices[i]);
        }

        #region IPrimitive Members

        public void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
        {
            vertices = this.vertices;
            indices = this.indices;
            body = null;
        }

        #endregion
    }
}
