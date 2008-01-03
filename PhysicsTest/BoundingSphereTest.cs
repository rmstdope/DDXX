using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Physics
{
    [TestFixture]
    public class BoundingSphereTest
    {
        private const float epsilon = 0.00001f;

        [Test]
        public void TestOrigoInsideOrigoBased()
        {
            BoundingSphere sphere = new BoundingSphere(new Vector3(2, 0, 0), 5.0f);
            Assert.IsTrue(sphere.IsInside(new Vector3(0, 0, 0)), "0, 0, 0 should be inside sphere.");
        }

        [Test]
        public void TestOrigoOutsideNotOrigoBased()
        {
            BoundingSphere sphere = new BoundingSphere(new Vector3(2, 0, 0), 1.0f);
            Assert.IsFalse(sphere.IsInside(new Vector3(0, 0, 0)), "0, 0, 0 should not be inside sphere.");
        }

        [Test]
        public void TestPointOutside()
        {
            BoundingSphere sphere = new BoundingSphere(9.0f);
            Assert.IsFalse(sphere.IsInside(new Vector3(10, 0, 0)), "10, 0, 0 should be outside sphere.");
        }

        [Test]
        public void TestPointOutsideNotOrigoBased()
        {
            BoundingSphere sphere = new BoundingSphere(new Vector3(-6, 0, 0), 10.0f);
            Assert.IsFalse(sphere.IsInside(new Vector3(5, 0, 0)), "5, 0, 0 should be outside sphere.");
        }

        [Test]
        public void TestPointInside()
        {
            BoundingSphere sphere = new BoundingSphere(9.0f);
            Assert.IsTrue(sphere.IsInside(new Vector3(5, 5, 5)), "5, 5, 5 should be inside sphere.");
        }

        [Test]
        public void TestPointInsideNotOrigoBased()
        {
            BoundingSphere sphere = new BoundingSphere(new Vector3(10, 20, 30), 2.0f);
            Assert.IsTrue(sphere.IsInside(new Vector3(11, 21, 31)), "11, 21, 31 should be inside sphere.");
        }

        [Test]
        public void TestPointOnEdge()
        {
            BoundingSphere sphere = new BoundingSphere(2.0f);
            Assert.IsFalse(sphere.IsInside(new Vector3(0, 2, 0)), "0, 2, 0 should be inside sphere.");
        }

        [Test]
        public void TestPointOnEdgeNotOrigoBased()
        {
            BoundingSphere sphere = new BoundingSphere(new Vector3(1, 1, 1), 4.0f);
            Assert.IsFalse(sphere.IsInside(new Vector3(1, 1, 5)), "1, 1, 5 should be inside sphere.");
        }

        [Test]
        public void TestClosesPointMoveX()
        {
            BoundingSphere sphere = new BoundingSphere(4.0f);
            CompareVectors(new Vector3(4, 0, 0), sphere.ConstrainOutside(new Vector3(2, 0, 0)));
        }

        [Test]
        public void TestClosesPointMoveZ()
        {
            BoundingSphere sphere = new BoundingSphere(4.0f);
            CompareVectors(new Vector3(0, 0, 4), sphere.ConstrainOutside(new Vector3(0, 0, 2)));
        }

        [Test]
        public void TestClosesPointMoveXYZ()
        {
            const float radius = 10.0f;
            BoundingSphere sphere = new BoundingSphere(radius);
            Vector3 newVector = sphere.ConstrainOutside(new Vector3(1, 2, 3));
            Assert.AreEqual(newVector.X * 2, newVector.Y, epsilon);
            Assert.AreEqual(newVector.X * 3, newVector.Z, epsilon);
            Assert.AreEqual(radius, newVector.Length(), epsilon);
        }

        [Test]
        public void TestClosesPointMoveXYZNotOrigoBased()
        {
            const float radius = 10.0f;
            Vector3 center = new Vector3(0.5f, 1, 1.5f); 
            BoundingSphere sphere = new BoundingSphere(center, radius);
            Vector3 newVector = sphere.ConstrainOutside(new Vector3(1, 2, 3));
            Assert.AreEqual(newVector.X * 2, newVector.Y, epsilon);
            Assert.AreEqual(newVector.X * 3, newVector.Z, epsilon);
            Assert.AreEqual(radius, (newVector - center).Length(), epsilon);
        }

        [Test]
        public void TestClosesPointMoveFromCenterNotOrigoBased()
        {
            const float radius = 10.0f;
            Vector3 center = new Vector3(1, 2, 3);
            BoundingSphere sphere = new BoundingSphere(center, radius);
            Vector3 newVector = sphere.ConstrainOutside(new Vector3(1, 2, 3));
            Assert.AreEqual(radius, (newVector - center).Length(), epsilon);
        }

        [Test]
        public void TestClosesPointNoMove()
        {
            BoundingSphere sphere = new BoundingSphere(4.0f);
            Assert.AreEqual(new Vector3(0, 0, 10), sphere.ConstrainOutside(new Vector3(0, 0, 10)),
                "0, 0, 10 should not be moved.");
        }

        [Test]
        public void TestClosesPointNoMoveNotOrigoBased()
        {
            BoundingSphere sphere = new BoundingSphere(new Vector3(0, 0, -4), 4.0f);
            Assert.AreEqual(new Vector3(0, 0, 1), sphere.ConstrainOutside(new Vector3(0, 0, 1)),
                "0, 0, 1 should not be moved.");
        }

        private void CompareVectors(Vector3 expected, Vector3 actual)
        {
            Assert.AreEqual(expected.X, actual.X, epsilon);
            Assert.AreEqual(expected.Y, actual.Y, epsilon);
            Assert.AreEqual(expected.Z, actual.Z, epsilon);
        }

    }
}
