using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class BoxPrimitiveTest
    {
        private const float epsilon = 0.0001f;
        private BoxPrimitive box;
        private IPrimitive primitive;
        protected enum Side
        {
            FRONT = 0,
            BACK,
            TOP,
            BOTTOM,
            LEFT,
            RIGHT
        }

        [Test]
        public void Getters()
        {
            CreateBox(1, 2, 3);
            Assert.AreEqual(1, box.Length);
            Assert.AreEqual(2, box.Width);
            Assert.AreEqual(3, box.Height);
        }

        /// <summary>
        /// Check number of vertices for a box.
        /// </summary>
        [Test]
        public void TestNumVerticesSingleSegment()
        {
            CreateBox(10, 20, 30);
            Assert.AreEqual(24, primitive.Vertices.Length, "The box should have 24 vertices.");
            Assert.AreEqual(36, primitive.Indices.Length, "The box should have 36 indices.");
        }

        /// <summary>
        /// Test the front side of the box when it only has one segment.
        /// </summary>
        [Test]
        public void TestFrontSingleSegment()
        {
            TestBoxSingleSegment(Side.FRONT, new Vector3(0, 0, 1));
        }

        /// <summary>
        /// Test the back side of the box when it only has one segment.
        /// </summary>
        [Test]
        public void TestBackSingleSegment()
        {
            TestBoxSingleSegment(Side.BACK, new Vector3(0, 0, -1));
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
            CreateBox(10, 20, 30);
            int v = GetBoxStartVertex(side, 1, 1, 1);
            int i = GetBoxStartIndex(side, 1, 1, 1);
            // Check vertices against sphere (0, 0, -1, -sideLength)
            CheckRectangleInPlane(v, v + 4, new Plane(normal.X, normal.Y, normal.Z, -sideLength));
            // Check that indices points to the correct vertices
            CheckRectangleIndices(v, v + 4, i, i + 6);
            // Check that the indices create clockwise triangles
            CheckRectangleClockwise(i, 2, normal);
            // Check normals
            CheckRectangleNormals(v, v + 4, normal);
        }

        private int GetBoxStartVertex(Side side, int lengthSegments, int widthSegments, int heightSegments)
        {
            return 4 * (int)side;
        }

        private int GetBoxStartIndex(Side side, int lengthSegments, int widthSegments, int heightSegments)
        {
            return 6 * (int)side;
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
                Vector3 testNormal = Vector3.Cross(v3 - v1, v2 - v1);
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

        private void CreateBox(float length, float width, float height)
        {
            box = new BoxPrimitive();
            box.Length = length;
            box.Width = width;
            box.Height = height;
            primitive = box.Generate();
            Assert.IsNull(primitive.Body);
        }

    }
}
