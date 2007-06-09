using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using Dope.DDXX.Graphics.Skinning;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class NodeBaseTest
    {
        class NodeBaseImpl : NodeBase
        {
            public NodeBaseImpl(string name) : base(name) { }
            protected override void StepNode() { }
            protected override void RenderNode(IScene scene) { }
        }

        class DerivedNode : NodeBase
        {
            public bool stepCalled;
            public bool renderCalled;
            public bool setLightStateCalled;
            public IScene renderScene;
            public DerivedNode(string name) : base(name) { }
            protected override void StepNode() { stepCalled = true; }
            protected override void RenderNode(IScene scene) { renderCalled = true; renderScene = scene; }
            protected override void SetLightStateNode(LightState state) { setLightStateCalled = true; }
        }

        [Test]
        public void TestConnections()
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
            Assert.AreEqual(node1.Children.Count, 1);
            Assert.AreEqual(node2.Children.Count, 0);
            Assert.AreEqual(node1.Children[0], node2);
            Assert.AreEqual(2, node1.CountNodes());
            Assert.AreEqual(1, node2.CountNodes());
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

            node1.Render(null);

            Assert.IsTrue(node1.renderCalled);
            Assert.IsNull(node1.renderScene);
            Assert.IsTrue(node2.renderCalled);
            Assert.IsNull(node2.renderScene);
        }

        public void AssertVectors(Vector3 vec1, Vector3 vec2)
        {
            float epsilon = 0.001f;
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
            node1.WorldState.Rotation = Matrix.RotationY((float)Math.PI);
            node2.WorldState.Rotation = Matrix.RotationY((float)Math.PI);
            AssertVectors(vec, Vector3.TransformCoordinate(vec, node2.WorldMatrix));

            node1.WorldState.Reset();
            node2.WorldState.Reset();
            node1.WorldState.Scaling = new Vector3(2, 3, 4);
            node2.WorldState.Scaling = new Vector3(3, 4, 5);
            AssertVectors(new Vector3(6, 24, 60), Vector3.TransformCoordinate(vec, node2.WorldMatrix));

            node1.WorldState.Position = new Vector3(100, 200, 300);
            node1.WorldState.Rotation = Matrix.RotationY((float)Math.PI);
            node1.WorldState.Scaling = new Vector3(2, 3, 4);
            node2.WorldState.Position = new Vector3(100, 200, 300);
            node2.WorldState.Rotation = Matrix.RotationY((float)Math.PI);
            node2.WorldState.Scaling = new Vector3(2, 3, 4);
            AssertVectors(new Vector3(100 - 98 * 2, 200 + 206 * 3, 300 - 288 * 4), Vector3.TransformCoordinate(vec, node2.WorldMatrix));
        }

        [Test]
        public void TestSetLightState()
        {
            DerivedNode node1 = new DerivedNode("NewNewNodeName1");
            DerivedNode node2 = new DerivedNode("NewNewNodeName2");
            node1.AddChild(node2);

            node1.SetLightState(null);

            Assert.IsTrue(node1.setLightStateCalled);
            Assert.IsTrue(node2.setLightStateCalled);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestFrameWorldStateConflict()
        {
            IFrame frame = new FrameAdapter(new SkinnedFrame("Test"));
            DerivedNode node1 = new DerivedNode("NewNewNodeName1");
            node1.EnableFrameHandling(frame);
            Vector3 vec = node1.WorldState.Position;
        }

        [Test]
        public void TestFramePosition()
        {
            IFrame frame = new FrameAdapter(new SkinnedFrame("Test"));
            frame.TransformationMatrix = Matrix.Translation(3, 5, 7);
            DerivedNode node1 = new DerivedNode("NewNewNodeName1");
            node1.WorldState.Position = new Vector3(1, 2, 3);
            node1.EnableFrameHandling(frame);
            Matrix m = node1.WorldMatrix;
            Assert.AreEqual(new Vector3(3, 5, 7), new Vector3(m.M41, m.M42, m.M43), "Position should be (3, 5, 7)");
        }

        [Test]
        public void TestFrameTransformation()
        {
            IFrame frame = new FrameAdapter(new SkinnedFrame("Test"));
            frame.TransformationMatrix = Matrix.PerspectiveRH(1, 2, 3, 4);
            DerivedNode node1 = new DerivedNode("NewNewNodeName1");
            node1.EnableFrameHandling(frame);
            Assert.AreEqual(Matrix.PerspectiveRH(1, 2, 3, 4), node1.WorldMatrix, 
                "Transformation matrix should match frame.");
        }

    }
}
