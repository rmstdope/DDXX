using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class PlanePrimitiveTest : PrimitiveTest
    {

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        /// <summary>
        /// Check number of vertices for a plane with one segment.
        /// </summary>
        [Test]
        public void TestNumVerticesPlaneSingleSegment()
        {
            Primitive box = Primitive.PlanePrimitive(10, 30, 1, 1);
            Assert.AreEqual(4, box.Vertices.Length, "The plane should have 4 vertices.");
            Assert.AreEqual(6, box.Indices.Length, "The plane should have 6 indices.");
        }

        /// <summary>
        /// Check number of vertices for a plane with multiple segments.
        /// </summary>
        [Test]
        public void TestNumVerticesPlaneMultipleSegments()
        {
            Primitive box = Primitive.PlanePrimitive(20, 40, 1, 4);
            Assert.AreEqual(10, box.Vertices.Length, "The plane should have 10 vertices.");
            Assert.AreEqual(24, box.Indices.Length, "The plane should have 24 indices.");
        }

        /// <summary>
        /// Test a plane with only one segment.
        /// </summary>
        [Test]
        public void TestPlaneSingleSegment()
        {
            primitive = Primitive.PlanePrimitive(10, 20, 1, 1);
            int startVertex = 0;
            int startIndex = 0;
            int numVertices = 4;
            int numTriangles = 2;
            // Check vertices against plane (0, 0, -1, 0)
            CheckInPlane(startVertex, numVertices, new Plane(0, 0, -1, 0));
            // Check that indices points to the correct vertices
            CheckIndices(startVertex, numVertices, startIndex, numTriangles * 3);
            // Check that the indices create clockwise triangles
            CheckClockwise(startIndex, numTriangles, new Vector3(0, 0, -1));
            // Check normals
            CheckNormals(startVertex, numVertices, new Vector3(0, 0, -1));
            // Check limits
            CheckLimits(10.0f, 20.0f, 1, 1);
        }

        /// <summary>
        /// Test a plane with more segments.
        /// </summary>
        [Test]
        public void TestPlaneMultipleSegment()
        {
            primitive = Primitive.PlanePrimitive(20, 40, 2, 4);
            int startVertex = 0;
            int startIndex = 0;
            int numVertices = 15;
            int numTriangles = 16;
            // Check vertices against plane (0, 0, -1, 0)
            CheckInPlane(startVertex, numVertices, new Plane(0, 0, -1, 0));
            // Check that indices points to the correct vertices
            CheckIndices(startVertex, numVertices, startIndex, numTriangles * 3);
            // Check that the indices create clockwise triangles
            CheckClockwise(startIndex, numTriangles, new Vector3(0, 0, -1));
            // Check normals
            CheckNormals(startVertex, numVertices, new Vector3(0, 0, -1));
            // Check limits
            CheckLimits(20.0f, 40.0f, 2, 4);
        }

        private void CheckLimits(float width, float height, int widthSegments, int heightSegments)
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

    }
}
