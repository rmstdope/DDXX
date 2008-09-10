using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class PlanePrimitiveTest
    {
        private const float epsilon = 0.000001f;
        private PlanePrimitive plane;
        private IPrimitive primitive;

        [Test]
        public void Getters()
        {
            CreatePlane(10, 30, 1, 2, false);
            Assert.AreEqual(10, plane.Width);
            Assert.AreEqual(30, plane.Height);
            Assert.AreEqual(1, plane.WidthSegments);
            Assert.AreEqual(2, plane.HeightSegments);
            Assert.AreEqual(false, plane.Textured);
        }

        /// <summary>
        /// Check number of vertices for a plane with one segment.
        /// </summary>
        [Test]
        public void TestNumVerticesPlaneSingleSegment()
        {
            CreatePlane(10, 30, 1, 1, false);
            Assert.AreEqual(4, primitive.Vertices.Length, "The plane should have 4 vertices.");
            Assert.AreEqual(6, primitive.Indices.Length, "The plane should have 6 indices.");
        }

        /// <summary>
        /// Check number of vertices for a plane with multiple segments.
        /// </summary>
        [Test]
        public void TestNumVerticesPlaneMultipleSegments()
        {
            CreatePlane(20, 40, 1, 4, false);
            Assert.AreEqual(10, primitive.Vertices.Length, "The plane should have 10 vertices.");
            Assert.AreEqual(24, primitive.Indices.Length, "The plane should have 24 indices.");
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
            CheckRectangleNormals(startVertex, numVertices, new Vector3(0, 0, 1));
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
            CheckRectangleNormals(startVertex, numVertices, new Vector3(0, 0, 1));
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
            CheckRectangleNormals(startVertex, numVertices, new Vector3(0, 0, 1));
            // Check limits
            CheckPlaneVertexLimits(20.0f, 40.0f, 2, 4);
            // Check no texture coordinates
            CheckPlaneTexCoords(true, 2, 4);
        }

        private void CreatePlane(float width, float height, int widthSegments, int heightSegments, bool textured)
        {
            plane = new PlanePrimitive();
            plane.Width = width;
            plane.Height = height;
            plane.WidthSegments = widthSegments;
            plane.HeightSegments = heightSegments;
            plane.Textured = textured;
            primitive = plane.Generate();
            Assert.IsNull(primitive.Body);
        }

        private void CheckRectangleInPlane(int startIndex, int endIndex, Plane plane)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                Assert.AreEqual(0.0f, plane.Dot(new Vector4(primitive.Vertices[i].Position, 1)), epsilon,
                    "All points should be in sphere (" + startIndex + ", " + endIndex + ")");
            }
        }

        private void CheckRectangleNormals(int startIndex, int endIndex, Vector3 normal)
        {
            for (int i = startIndex; i < endIndex; i++)
                Assert.AreEqual(normal, primitive.Vertices[i].Normal);
        }

        private void CheckRectangleClockwise(int startI, int numTriangles, Vector3 normal)
        {
            for (int i = 0; i < numTriangles; i++)
            {
                Vector3 v1 = primitive.Vertices[primitive.Indices[startI + i * 3 + 0]].Position;
                Vector3 v2 = primitive.Vertices[primitive.Indices[startI + i * 3 + 1]].Position;
                Vector3 v3 = primitive.Vertices[primitive.Indices[startI + i * 3 + 2]].Position;
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
                Assert.IsTrue(primitive.Indices[i] >= startV, "Indices must not point to vertex smaller than " + startV);
                Assert.IsTrue(primitive.Indices[i] < endV, "Indices must not point to vertex larger or equal to " + endV);
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
                        Assert.AreEqual(height / 2, primitive.Vertices[v].Position.Y);
                    else
                        Assert.IsTrue(primitive.Vertices[v].Position.Y <
                            primitive.Vertices[v - widthSegments - 1].Position.Y);
                    if (y == heightSegments)
                        Assert.AreEqual(-height / 2, primitive.Vertices[v].Position.Y);
                    if (x == 0)
                        Assert.AreEqual(-width / 2, primitive.Vertices[v].Position.X);
                    else
                        Assert.IsTrue(primitive.Vertices[v].Position.X >
                            primitive.Vertices[v - 1].Position.X);
                    if (x == widthSegments)
                        Assert.AreEqual(width / 2, primitive.Vertices[v].Position.X);
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
                        Assert.IsFalse(primitive.Vertices[v].TextureCoordinatesUsed,
                            "Texture coordinates should not be used.");
                    else
                    {
                        Assert.IsTrue(primitive.Vertices[v].TextureCoordinatesUsed,
                            "Texture coordinates should not be used.");
                        if (y == 0)
                            Assert.AreEqual(0, primitive.Vertices[v].V);
                        else
                            Assert.IsTrue(primitive.Vertices[v].V >
                                primitive.Vertices[v - widthSegments - 1].V);
                        if (y == heightSegments)
                            Assert.AreEqual(1.0f, primitive.Vertices[v].V);
                        if (x == 0)
                            Assert.AreEqual(0, primitive.Vertices[v].U);
                        else
                            Assert.IsTrue(primitive.Vertices[v].U >
                                primitive.Vertices[v - 1].U);
                        if (x == widthSegments)
                            Assert.AreEqual(1.0f, primitive.Vertices[v].U);
                        v++;
                    }
                }
            }
        }

    }
}
