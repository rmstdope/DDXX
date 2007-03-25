using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;

namespace MeshBuilder
{
    [TestFixture]
    public class PrimitiveTest
    {
        private Primitive box;
        private enum Side
        {
            FRONT = 0,
            BACK,
            TOP,
            BOTTOM,
            LEFT,
            RIGHT
        }
        private const float epsilon = 0.000001f;

        /// <summary>
        /// Check number of vertices for a box.
        /// </summary>
        [Test]
        public void TestNumVerticesSingleSegment()
        {
            Primitive box = Primitive.BoxPrimitive(10, 20, 30, 1, 1, 1);
            Assert.AreEqual(24, box.Vertices.Length, "The box should have 24 vertices.");
            Assert.AreEqual(36, box.Indices.Length, "The box should have 36 indices.");
        }

        /// <summary>
        /// Test the front side of the box when it only has one segment.
        /// </summary>
        [Test]
        public void TestFrontSingleSegment()
        {
            TestBoxSingleSegment(Side.FRONT, new Vector3(0, 0, -1));
        }

        /// <summary>
        /// Test the back side of the box when it only has one segment.
        /// </summary>
        [Test]
        public void TestBackSingleSegment()
        {
            TestBoxSingleSegment(Side.BACK, new Vector3(0, 0, 1));
        }

        /// <summary>
        /// Test the top side of the box when it only has one segment.
        /// </summary>
        [Test]
        public void TestTopSingleSegment()
        {
            TestBoxSingleSegment(Side.TOP, new Vector3(0, 1, 0));
        }

        /// <summary>
        /// Test the bottom side of the box when it only has one segment.
        /// </summary>
        [Test]
        public void TestBottomSingleSegment()
        {
            TestBoxSingleSegment(Side.BOTTOM, new Vector3(0, -1, 0));
        }

        /// <summary>
        /// Test the left side of the box when it only has one segment.
        /// </summary>
        [Test]
        public void TestLeftSingleSegment()
        {
            TestBoxSingleSegment(Side.LEFT, new Vector3(-1, 0, 0));
        }

        /// <summary>
        /// Test the right side of the box when it only has one segment.
        /// </summary>
        [Test]
        public void TestRightSingleSegment()
        {
            TestBoxSingleSegment(Side.RIGHT, new Vector3(1, 0, 0));
        }

        private void TestBoxSingleSegment(Side side, Vector3 normal)
        {
            float sideLength;
            if (side == Side.BACK || side == Side.FRONT)
                sideLength = 5;
            else if (side == Side.TOP || side == Side.BOTTOM)
                sideLength = 15;
            else
                sideLength = 10;
            box = Primitive.BoxPrimitive(10, 20, 30, 1, 1, 1);
            int v = GetStartVertex(side, 1, 1, 1);
            int i = GetStartIndex(side, 1, 1, 1);
            // Check vertices against plane (0, 0, -1, -sideLength)
            CheckInPlane(v, v + 4, new Plane(normal.X, normal.Y, normal.Z, -sideLength));
            // Check that indices points to the correct vertices
            CheckIndices(v, v + 4, i, i + 6);
            // Check that the indices create clockwise triangles
            CheckClockwise(i, 2, normal);
        }

        private int GetStartVertex(Side side, int lengthSegments, int widthSegments, int heightSegments)
        {
            return 4 * (int)side;
        }

        private int GetStartIndex(Side side, int lengthSegments, int widthSegments, int heightSegments)
        {
            return 6 * (int)side;
        }

        private void CheckInPlane(Vertex[] vertex, int startIndex, int endIndex)
        {
            Plane plane = Plane.FromPoints(vertex[startIndex].Position, 
                vertex[startIndex + 1].Position, vertex[startIndex + 2].Position);
            for (int i = startIndex + 3; i < endIndex; i++)
            {
                Assert.AreEqual(0.0f, plane.Dot(vertex[i].Position), epsilon,
                    "All points should be in plane (" + startIndex + ", " + endIndex + ")");
            }
        }

        private void CheckInPlane(int startIndex, int endIndex, Plane plane)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                Assert.AreEqual(0.0f, plane.Dot(box.Vertices[i].Position), epsilon,
                    "All points should be in plane (" + startIndex + ", " + endIndex + ")");
            }
        }

        private void CheckClockwise(int startI, int numTriangles, Vector3 normal)
        {
            for (int i = 0; i < numTriangles; i++)
            {
                Vector3 v1 = box.Vertices[box.Indices[startI + i * 3 + 0]].Position;
                Vector3 v2 = box.Vertices[box.Indices[startI + i * 3 + 1]].Position;
                Vector3 v3 = box.Vertices[box.Indices[startI + i * 3 + 2]].Position;
                Vector3 testNormal = Vector3.Cross(v2 - v1, v3 - v1);
                testNormal.Normalize();
                Assert.AreEqual(normal.X, testNormal.X, epsilon, "Normals should be equal.");
                Assert.AreEqual(normal.Y, testNormal.Y, epsilon, "Normals should be equal.");
                Assert.AreEqual(normal.Z, testNormal.Z, epsilon, "Normals should be equal.");
            }
        }

        private void CheckIndices(int startV, int endV, int startI, int endI)
        {
            for (int i = startI; i < endI; i++)
            {
                Assert.IsTrue(box.Indices[i] >= startV, "Indices must not point to vertex smaller than " + startV);
                Assert.IsTrue(box.Indices[i] < endV, "Indices must not point to vertex larger or equal to " + endV);
            }
        }

    }
}
