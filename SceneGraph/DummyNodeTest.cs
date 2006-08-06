using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using SceneGraph;
using Utility;
using Microsoft.DirectX;

namespace SceneGraph
{
    [TestFixture]
    public class DummyNodeTest
    {
        [Test]
        public void TestConnecions()
        {
            DummyNode node1 = new DummyNode("NodeName");
            Assert.AreEqual(null, node1.Parent);
            Assert.AreEqual("NodeName", node1.Name);

            node1 = new DummyNode("NewNodeName");
            Assert.AreEqual("NewNodeName", node1.Name);

            DummyNode node2 = new DummyNode("NewNewNodeName");
            node1.AddChild(node2);
            Assert.AreEqual(node1, node2.Parent);
            Assert.AreEqual("NewNewNodeName", node2.Name);

        }

        class DerivedNode : DummyNode
        {
            public bool stepCalled;
            public DerivedNode() : base("test")
            {
            }
            public override void Step()
            {
                base.Step();

                stepCalled = true;
            }
        }

        [Test]
        public void TestStep()
        {
            DerivedNode node1 = new DerivedNode();// ("NodeName");
            DerivedNode node2 = new DerivedNode();//("NewNewNodeName");
            node1.AddChild(node2);

            node1.Step();

            Assert.IsTrue(node1.stepCalled);
            Assert.IsTrue(node2.stepCalled);
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
            DummyNode node1 = new DummyNode("NodeName");
            DummyNode node2 = new DummyNode("NodeName");
            node1.AddChild(node2);
            Assert.AreEqual(Matrix.Identity, node2.WorldMatrix);

            node1.WorldState.SetPosition(new Vector3(1, 2, 3));
            node2.WorldState.SetPosition(new Vector3(4, 5, 6));
            AssertVectors(new Vector3(6, 9, 12), Vector3.TransformCoordinate(vec, node2.WorldMatrix));

            node1.WorldState.Reset();
            node2.WorldState.Reset();
            Assert.AreEqual(Matrix.Identity, node2.WorldMatrix);

            // 1, 0, 0, 0 means rotate 180 deg around y axis (turn)
            node1.WorldState.SetRotation(new Quaternion(0, 1, 0, 0));
            node2.WorldState.SetRotation(new Quaternion(0, 1, 0, 0));
            AssertVectors(vec, Vector3.TransformCoordinate(vec, node2.WorldMatrix));

            node1.WorldState.Reset();
            node2.WorldState.Reset();
            node1.WorldState.SetScaling(new Vector3(2, 3, 4));
            node2.WorldState.SetScaling(new Vector3(3, 4, 5));
            AssertVectors(new Vector3(6, 24, 60), Vector3.TransformCoordinate(vec, node2.WorldMatrix));

            node1.WorldState.SetPosition(new Vector3(100, 200, 300));
            node1.WorldState.SetRotation(new Quaternion(0, 1, 0, 0));
            node1.WorldState.SetScaling(new Vector3(2, 3, 4));
            node2.WorldState.SetPosition(new Vector3(100, 200, 300));
            node2.WorldState.SetRotation(new Quaternion(0, 1, 0, 0));
            node2.WorldState.SetScaling(new Vector3(2, 3, 4));
            AssertVectors(new Vector3(100 - 98 * 2, 200 + 206 * 3, 300 - 288 * 4), Vector3.TransformCoordinate(vec, node2.WorldMatrix));
        }
    }
}
