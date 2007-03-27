using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class BoxPrimitiveTest : PrimitiveTest
    {
        private const float epsilon = 0.000001f;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

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
            primitive = Primitive.BoxPrimitive(10, 20, 30, 1, 1, 1);
            int v = GetStartVertex(side, 1, 1, 1);
            int i = GetStartIndex(side, 1, 1, 1);
            // Check vertices against plane (0, 0, -1, -sideLength)
            CheckInPlane(v, v + 4, new Plane(normal.X, normal.Y, normal.Z, -sideLength));
            // Check that indices points to the correct vertices
            CheckIndices(v, v + 4, i, i + 6);
            // Check that the indices create clockwise triangles
            CheckClockwise(i, 2, normal);
            // Check normals
            CheckNormals(v, v + 4, normal);
        }

    }
}
