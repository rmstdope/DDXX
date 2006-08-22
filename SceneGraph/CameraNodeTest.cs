using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class CameraNodeTest
    {
        [Test]
        public void TestCamera()
        {
            CameraNode c1 = new CameraNode("C1");
            Assert.AreEqual(null, c1.Parent);
            Assert.AreEqual("C1", c1.Name);
        }

        [Test]
        public void TestFOV()
        {
            CameraNode c1 = new CameraNode("C1");

            Assert.AreEqual((float)Math.PI / 4, c1.GetFOV());

            c1.SetFOV(100.0f);
            Assert.AreEqual(100.0f, c1.GetFOV());
        }

        [Test]
        public void TestProjection()
        {
            float fov1 = (float)Math.PI / 4;
            float fov2 = (float)Math.PI / 2;
            float aspect1 = 4.0f / 3.0f;
            float aspect2 = 16.0f / 9.0f;
            float zNear1 = 1.0f;
            float zNear2 = 0.5f;
            float zFar1 = 1000.0f;
            float zFar2 = 5000.0f;
            Matrix exp1 = Matrix.PerspectiveFovLH(fov1, aspect1, zNear1, zFar1);
            Matrix exp2 = Matrix.PerspectiveFovLH(fov2, aspect2, zNear2, zFar2);

            CameraNode c1 = new CameraNode("Name");
            Assert.IsTrue(exp1.Equals(c1.GetProjectionMatrix()));
            c1.SetFOV(fov2);
            c1.SetClippingPlanes(zNear2, zFar2);
            c1.SetAspect(aspect2);
            Assert.IsTrue(exp2.Equals(c1.GetProjectionMatrix()));
        }

        public void AssertVectors(Vector3 vec1, Vector3 vec2)
        {
            float epsilon = 0.0001f;
            float len = (vec1 - vec2).Length();
            Assert.AreEqual(0, len, epsilon);
        }

        [Test]
        public void TestView()
        {
            Vector3 vec = new Vector3(1, 2, 3);
            CameraNode c1 = new CameraNode("Name");
            AssertVectors(new Vector3(1, 2, 3), Vector3.TransformCoordinate(vec, c1.GetViewMatrix()));

            c1.WorldState.Position = new Vector3(100, 200, 300);
            c1.WorldState.Rotation = new Quaternion(0, 1, 0, 0);
            AssertVectors(new Vector3(99, -198, 297), Vector3.TransformCoordinate(vec, c1.GetViewMatrix()));
        }
    }
}