using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SceneGraph;
using Microsoft.DirectX;

namespace SceneGraph
{
    [TestFixture]
    public class NodeTest
    {
        [Test]
        public void TestGraph()
        {
            Node node1 = new Node("NodeName");
            Assert.AreEqual(null, node1.GetParent());
            Assert.AreEqual("NodeName", node1.GetName());

            node1 = new Node("NewNodeName");
            Assert.AreEqual("NewNodeName", node1.GetName());

            Node node2 = new Node("NewNewNodeName", node1);
            Assert.AreEqual(node1, node2.GetParent());
            Assert.AreEqual("NewNewNodeName", node2.GetName());

        }

        public void AssertVectors(Vector3 vec1, Vector3 vec2)
        {
            float epsilon = 0.0001f;
            float len = (vec1 - vec2).Length();
            Assert.AreEqual(0, len, epsilon);
        }

        [Test]
        public void TestWorldMatrix()
        {
            Vector3 vec = new Vector3(1, 2, 3);
            Node node1 = new Node("NodeName");
            Node node2 = new Node("NodeName", node1);
            Assert.AreEqual(Matrix.Identity, node2.GetWorldMatrix());

            node1.SetPosition(new Vector3(1, 2, 3));
            node2.SetPosition(new Vector3(4, 5, 6));
            AssertVectors(new Vector3(6, 9, 12), Vector3.TransformCoordinate(vec, node2.GetWorldMatrix()));

            node1.Reset();
            node2.Reset();
            Assert.AreEqual(Matrix.Identity, node2.GetWorldMatrix());

            // 1, 0, 0, 0 means rotate 180 deg around y axis (turn)
            node1.SetRotation(new Quaternion(0, 1, 0, 0));
            node2.SetRotation(new Quaternion(0, 1, 0, 0));
            AssertVectors(vec, Vector3.TransformCoordinate(vec, node2.GetWorldMatrix()));

            node1.Reset();
            node2.Reset();
            node1.SetScaling(new Vector3(2, 3, 4));
            node2.SetScaling(new Vector3(3, 4, 5));
            AssertVectors(new Vector3(6, 24, 60), Vector3.TransformCoordinate(vec, node2.GetWorldMatrix()));

            node1.SetPosition(new Vector3(100, 200, 300));
            node1.SetRotation(new Quaternion(0, 1, 0, 0));
            node1.SetScaling(new Vector3(2, 3, 4));
            node2.SetPosition(new Vector3(100, 200, 300));
            node2.SetRotation(new Quaternion(0, 1, 0, 0));
            node2.SetScaling(new Vector3(2, 3, 4));
            AssertVectors(new Vector3(100 - 98 * 2, 200 + 206 * 3, 300 - 288 * 4), Vector3.TransformCoordinate(vec, node2.GetWorldMatrix()));
        }
    }
}
