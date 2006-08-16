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
    public class NodeBaseTest
    {
        class DerivedNode : NodeBase
        {
            public bool stepCalled;
            public bool renderCalled;
            public CameraNode renderCamera;
            public DerivedNode(string name) : base(name) { }
            protected override void StepNode() { stepCalled = true; }
            protected override void RenderNode(CameraNode camera) { renderCalled = true; renderCamera = camera; }
        }

        [Test]
        public void TestConnecions()
        {
            NodeBase node1 = new DerivedNode("NodeName");
            Assert.AreEqual(null, node1.Parent);
            Assert.AreEqual("NodeName", node1.Name);

            node1 = new DerivedNode("NewNodeName");
            Assert.AreEqual("NewNodeName", node1.Name);

            NodeBase node2 = new DerivedNode("NewNewNodeName");
            node1.AddChild(node2);
            Assert.AreEqual(node1, node2.Parent);
            Assert.AreEqual("NewNewNodeName", node2.Name);

            Assert.IsTrue(node1.HasChild(node2));
            Assert.IsFalse(node1.HasChild(node1));
            Assert.IsFalse(node2.HasChild(node1));
            Assert.IsFalse(node2.HasChild(node2));
        }

        [Test]
        public void TestStep()
        {
            DerivedNode node1 = new DerivedNode("NodeName");
            DerivedNode node2 = new DerivedNode("NewNewNodeName");
            node1.AddChild(node2);

            node1.Step();

            Assert.IsTrue(node1.stepCalled);
            Assert.IsTrue(node2.stepCalled);
        }

        [Test]
        public void TestRender()
        {
            DerivedNode node1 = new DerivedNode("NodeName");
            DerivedNode node2 = new DerivedNode("NewNewNodeName");
            CameraNode camera = new CameraNode("Camera");
            node1.AddChild(node2);

            node1.Render(camera);

            Assert.IsTrue(node1.renderCalled);
            Assert.AreSame(node1.renderCamera, camera);
            Assert.IsTrue(node2.renderCalled);
            Assert.AreSame(node2.renderCamera, camera);
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
            NodeBase node1 = new DerivedNode("NodeName");
            NodeBase node2 = new DerivedNode("NodeName");
            node1.AddChild(node2);
            Assert.AreEqual(Matrix.Identity, node2.WorldMatrix);

            node1.WorldState.Position = new Vector3(1, 2, 3);
            node2.WorldState.Position = new Vector3(4, 5, 6);
            AssertVectors(new Vector3(6, 9, 12), Vector3.TransformCoordinate(vec, node2.WorldMatrix));

            node1.WorldState.Reset();
            node2.WorldState.Reset();
            Assert.AreEqual(Matrix.Identity, node2.WorldMatrix);

            // 1, 0, 0, 0 means rotate 180 deg around y axis (turn)
            node1.WorldState.Rotation = new Quaternion(0, 1, 0, 0);
            node2.WorldState.Rotation = new Quaternion(0, 1, 0, 0);
            AssertVectors(vec, Vector3.TransformCoordinate(vec, node2.WorldMatrix));

            node1.WorldState.Reset();
            node2.WorldState.Reset();
            node1.WorldState.Scaling = new Vector3(2, 3, 4);
            node2.WorldState.Scaling = new Vector3(3, 4, 5);
            AssertVectors(new Vector3(6, 24, 60), Vector3.TransformCoordinate(vec, node2.WorldMatrix));

            node1.WorldState.Position = new Vector3(100, 200, 300);
            node1.WorldState.Rotation = new Quaternion(0, 1, 0, 0);
            node1.WorldState.Scaling = new Vector3(2, 3, 4);
            node2.WorldState.Position = new Vector3(100, 200, 300);
            node2.WorldState.Rotation = new Quaternion(0, 1, 0, 0);
            node2.WorldState.Scaling = new Vector3(2, 3, 4);
            AssertVectors(new Vector3(100 - 98 * 2, 200 + 206 * 3, 300 - 288 * 4), Vector3.TransformCoordinate(vec, node2.WorldMatrix));
        }
    }
}
