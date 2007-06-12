using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class PlanePrimitiveTest
    {
        private const float epsilon = 0.000001f;
        private Vertex[] vertices;
        private short[] indices;

        /// <summary>
        /// Check number of vertices for a plane with one segment.
        /// </summary>
        [Test]
        public void TestNumVerticesPlaneSingleSegment()
        {
            CreatePlane(10, 30, 1, 1, false);
            Assert.AreEqual(4, vertices.Length, "The plane should have 4 vertices.");
            Assert.AreEqual(6, indices.Length, "The plane should have 6 indices.");
        }

        /// <summary>
        /// Check number of vertices for a plane with multiple segments.
        /// </summary>
        [Test]
        public void TestNumVerticesPlaneMultipleSegments()
        {
            CreatePlane(20, 40, 1, 4, false);
            Assert.AreEqual(10, vertices.Length, "The plane should have 10 vertices.");
            Assert.AreEqual(24, indices.Length, "The plane should have 24 indices.");
        }

        /// <summary>
        /// Test a plane with only one segment.
        /// </summary>
        [Test]
        public void TestPlaneSingleSegment()
        {
            CreatePlane(10, 20, 1, 1, false);
            int startVertex = 0;
            int startIndex = 0;
            int numVertices = 4;
            int numTriangles = 2;
            // Check vertices against plane (0, 0, -1, 0)
            CheckRectangleInPlane(startVertex, numVertices, new Plane(0, 0, -1, 0));
            // Check that indices points to the correct vertices
            CheckRectangleIndices(startVertex, numVertices, startIndex, numTriangles * 3);
            // Check that the indices create clockwise triangles
            CheckRectangleClockwise(startIndex, numTriangles, new Vector3(0, 0, -1));
            // Check normals
            CheckRectangleNormals(startVertex, numVertices, new Vector3(0, 0, -1));
            // Check limits
            CheckPlaneVertexLimits(10.0f, 20.0f, 1, 1);
            // Check no texture coordinates
            CheckPlaneTexCoords(false, 1, 1);
        }

        /// <summary>
        /// Test a plane with more segments.
        /// </summary>
        [Test]
        public void TestPlaneMultipleSegment()
        {
            CreatePlane(20, 40, 2, 4, false);
            int startVertex = 0;
            int startIndex = 0;
            int numVertices = 15;
            int numTriangles = 16;
            // Check vertices against plane (0, 0, -1, 0)
            CheckRectangleInPlane(startVertex, numVertices, new Plane(0, 0, -1, 0));
            // Check that indices points to the correct vertices
            CheckRectangleIndices(startVertex, numVertices, startIndex, numTriangles * 3);
            // Check that the indices create clockwise triangles
            CheckRectangleClockwise(startIndex, numTriangles, new Vector3(0, 0, -1));
            // Check normals
            CheckRectangleNormals(startVertex, numVertices, new Vector3(0, 0, -1));
            // Check limits
            CheckPlaneVertexLimits(20.0f, 40.0f, 2, 4);
            // Check no texture coordinates
            CheckPlaneTexCoords(false, 2, 4);
        }

        /// <summary>
        /// Test a plane with more segments and texture coordinates.
        /// </summary>
        [Test]
        public void TestPlaneMultipleSegmentTextured()
        {
            CreatePlane(20, 40, 2, 4, true);
            int startVertex = 0;
            int startIndex = 0;
            int numVertices = 15;
            int numTriangles = 16;
            // Check vertices against plane (0, 0, -1, 0)
            CheckRectangleInPlane(startVertex, numVertices, new Plane(0, 0, -1, 0));
            // Check that indices points to the correct vertices
            CheckRectangleIndices(startVertex, numVertices, startIndex, numTriangles * 3);
            // Check that the indices create clockwise triangles
            CheckRectangleClockwise(startIndex, numTriangles, new Vector3(0, 0, -1));
            // Check normals
            CheckRectangleNormals(startVertex, numVertices, new Vector3(0, 0, -1));
            // Check limits
            CheckPlaneVertexLimits(20.0f, 40.0f, 2, 4);
            // Check no texture coordinates
            CheckPlaneTexCoords(true, 2, 4);
        }

        private void CreatePlane(float width, float height, int widthSegments, int heightSegments, bool textured)
        {
            IBody body;
            PlanePrimitive plane = new PlanePrimitive();
            plane.Width = width;
            plane.Height = height;
            plane.WidthSegments = widthSegments;
            plane.HeightSegments = heightSegments;
            plane.Textured = textured;
            plane.Generate(out vertices, out indices, out body);
            Assert.IsNull(body);
        }

        private void CheckRectangleInPlane(int startIndex, int endIndex, Plane plane)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                Assert.AreEqual(0.0f, plane.Dot(vertices[i].Position), epsilon,
                    "All points should be in sphere (" + startIndex + ", " + endIndex + ")");
            }
        }

        private void CheckRectangleNormals(int startIndex, int endIndex, Vector3 normal)
        {
            for (int i = startIndex; i < endIndex; i++)
                Assert.AreEqual(normal, vertices[i].Normal);
        }

        private void CheckRectangleClockwise(int startI, int numTriangles, Vector3 normal)
        {
            for (int i = 0; i < numTriangles; i++)
            {
                Vector3 v1 = vertices[indices[startI + i * 3 + 0]].Position;
                Vector3 v2 = vertices[indices[startI + i * 3 + 1]].Position;
                Vector3 v3 = vertices[indices[startI + i * 3 + 2]].Position;
                Vector3 testNormal = Vector3.Cross(v2 - v1, v3 - v1);
                testNormal.Normalize();
                Assert.AreEqual(normal.X, testNormal.X, epsilon, "Normals should be equal.");
                Assert.AreEqual(normal.Y, testNormal.Y, epsilon, "Normals should be equal.");
                Assert.AreEqual(normal.Z, testNormal.Z, epsilon, "Normals should be equal.");
            }
        }

        private void CheckRectangleIndices(int startV, int endV, int startI, int endI)
        {
            for (int i = startI; i < endI; i++)
            {
                Assert.IsTrue(indices[i] >= startV, "Indices must not point to vertex smaller than " + startV);
                Assert.IsTrue(indices[i] < endV, "Indices must not point to vertex larger or equal to " + endV);
            }
        }

        private void CheckPlaneVertexLimits(float width, float height, int widthSegments, int heightSegments)
        {
            int v = 0;
            for (int y = 0; y < heightSegments + 1; y++)
            {
                for (int x = 0; x < widthSegments + 1; x++)
                {
                    if (y == 0)
                        Assert.AreEqual(height / 2, vertices[v].Position.Y);
                    else
                        Assert.IsTrue(vertices[v].Position.Y <
                            vertices[v - widthSegments - 1].Position.Y);
                    if (y == heightSegments)
                        Assert.AreEqual(-height / 2, vertices[v].Position.Y);
                    if (x == 0)
                        Assert.AreEqual(-width / 2, vertices[v].Position.X);
                    else
                        Assert.IsTrue(vertices[v].Position.X >
                            vertices[v - 1].Position.X);
                    if (x == widthSegments)
                        Assert.AreEqual(width / 2, vertices[v].Position.X);
                    v++;
                }
            }
        }

        private void CheckPlaneTexCoords(bool texCoords, int widthSegments, int heightSegments)
        {
            int v = 0;
            for (int y = 0; y < heightSegments + 1; y++)
            {
                for (int x = 0; x < widthSegments + 1; x++)
                {
                    if (!texCoords)
                        Assert.IsFalse(vertices[v].TextureCoordinatesUsed,
                            "Texture coordinates should not be used.");
                    else
                    {
                        Assert.IsTrue(vertices[v].TextureCoordinatesUsed,
                            "Texture coordinates should not be used.");
                        if (y == 0)
                            Assert.AreEqual(0, vertices[v].V);
                        else
                            Assert.IsTrue(vertices[v].V >
                                vertices[v - widthSegments - 1].V);
                        if (y == heightSegments)
                            Assert.AreEqual(1.0f, vertices[v].V);
                        if (x == 0)
                            Assert.AreEqual(0, vertices[v].U);
                        else
                            Assert.IsTrue(vertices[v].U >
                                vertices[v - 1].U);
                        if (x == widthSegments)
                            Assert.AreEqual(1.0f, vertices[v].U);
                        v++;
                    }
                }
            }
        }

    }
}
