using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class SceneTest : D3DMockTest
    {
        private class TestNode : NodeBase
        {
            public bool renderCalled = false;
            public bool stepCalled = false;
            public bool lightStateCalled = false;

            public TestNode(string name)
                : base(name)
            {
            }

            protected override void StepNode()
            {
                stepCalled = true;
            }

            protected override void RenderNode(IScene scene)
            {
                renderCalled = true;
            }

            protected override void SetLightStateNode(LightState state)
            {
                lightStateCalled = true;
            }
        }

        private Scene graph;
        private TestNode node1;
        private TestNode node2;
        private PointLightNode light1;
        private DirectionalLightNode light2;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            node1 = new TestNode("TestNode1");
            node2 = new TestNode("TestNode2");
            light1 = new PointLightNode("LightNode1");
            light2 = new DirectionalLightNode("LightNode2");
            effect = mockery.NewMock<IEffect>();

            graph = new Scene();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void AmbientLightDefault()
        {
            // Verify
            Assert.AreEqual(new Color(200, 200, 200), graph.AmbientColor);
        }

        [Test]
        public void AmbientLightSetGet()
        {
            // Exercise SUT
            graph.AmbientColor = new Color(1, 2, 3);
            // Verify
            Assert.AreEqual(new Color(1, 2, 3), graph.AmbientColor);
        }

        [Test]
        public void TestStepWithLights()
        {
            Assert.AreEqual(1, graph.NumNodes);

            CameraNode camera = new CameraNode("Camera", 1);
            camera.WorldState.Position = new Vector3(1, 2, 3);
            graph.AddNode(camera);
            graph.ActiveCamera = camera;
            Assert.AreEqual(2, graph.NumNodes);

            graph.AddNode(node1);
            node1.AddChild(node2);
            graph.AddNode(light1);
            light1.AddChild(light2);
            Assert.AreEqual(6, graph.NumNodes);

            graph.Step();
            Assert.IsTrue(node1.stepCalled, "Step() should have been called.");
            Assert.IsTrue(node2.stepCalled, "Step() should have been called.");
            Assert.IsTrue(node1.lightStateCalled, "SetLightState() should have been called.");
            Assert.IsTrue(node2.lightStateCalled, "SetLightState() should have been called.");
        }

        //[Test]
        //public void TestHierarchyStep()
        //{
        //    IAnimationRootFrame hierarchy1 = mockery.NewMock<IAnimationRootFrame>();
        //    IAnimationController controller1 = mockery.NewMock<IAnimationController>();
        //    IAnimationSet animationSet1 = mockery.NewMock<IAnimationSet>();
        //    IAnimationRootFrame hierarchy2 = mockery.NewMock<IAnimationRootFrame>();
        //    IAnimationController controller2 = mockery.NewMock<IAnimationController>();
        //    IAnimationSet animationSet2 = mockery.NewMock<IAnimationSet>();
        //    IAnimationRootFrame hierarchy3 = mockery.NewMock<IAnimationRootFrame>();
        //    CameraNode camera = new CameraNode("Camera");

        //    TestConstructorOK();

        //    Time.Initialize();
        //    Time.Step();
        //    graph.AddNode(camera);
        //    graph.ActiveCamera = camera;
        //    graph.HandleHierarchy(hierarchy1);
        //    graph.HandleHierarchy(hierarchy2);
        //    graph.HandleHierarchy(hierarchy3);

        //    Stub.On(effect).Method("SetValue");
        //    Stub.On(hierarchy1).GetProperty("AnimationController").Will(Return.Value(controller1));
        //    Stub.On(hierarchy2).GetProperty("AnimationController").Will(Return.Value(controller2));
        //    Stub.On(hierarchy3).GetProperty("AnimationController").Will(Return.Value(null));
        //    Stub.On(controller1).GetProperty("Time").Will(Return.Value(2.5));
        //    Stub.On(controller2).GetProperty("Time").Will(Return.Value(3.0));
        //    Stub.On(animationSet1).GetProperty("Period").Will(Return.Value(2.0));
        //    Stub.On(animationSet1).GetProperty("Period").Will(Return.Value(4.0));
        //    Stub.On(controller1).Method("GetAnimationSet").With(0).Will(Return.Value(animationSet1));
        //    Stub.On(controller2).Method("GetAnimationSet").With(0).Will(Return.Value(animationSet1));
        //    Expect.Once.On(controller1).Method("AdvanceTime").With(1.5);
        //    Expect.Once.On(controller1).Method("AdvanceTime").With((double)Time.StepTime);
        //    Expect.Once.On(controller2).Method("AdvanceTime").With(1.0);
        //    Expect.Once.On(controller2).Method("AdvanceTime").With((double)Time.StepTime);
        //    graph.Step();
        //}

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestRenderNoCamera()
        {
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.Render();
        }

        [Test]
        public void TestRenderWithLights()
        {
            TestStepWithLights();

            graph.Render();
        }

        [Test]
        public void TestRenderOK()
        {
            CameraNode camera = new CameraNode("Camera", 1);
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(camera);
            graph.AddNode(light1);
            graph.ActiveCamera = camera;

            graph.Render();
            Assert.IsTrue(node1.renderCalled, "Render() should have been called.");
            Assert.IsTrue(node2.renderCalled, "Render() should have been called.");
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestActiveCameraFail()
        {
            CameraNode camera = new CameraNode("Camera", 1);
            graph.ActiveCamera = camera;
        }

        [Test]
        public void TestActiveCamera()
        {
            CameraNode camera = new CameraNode("Camera", 1);
            graph.AddNode(camera);
            graph.ActiveCamera = camera;
            Assert.AreSame(camera, graph.ActiveCamera);
        }

        [Test]
        public void TestActiveCameraAsChild()
        {
            CameraNode camera = new CameraNode("Camera", 1);
            node1.AddChild(camera);
            graph.AddNode(node1);
            graph.ActiveCamera = camera;
            Assert.AreSame(camera, graph.ActiveCamera);
        }

        [Test]
        public void TestGetNodeByName()
        {
            graph.AddNode(node1);
            node1.AddChild(node2);
            graph.AddNode(light1);
            light1.AddChild(light2);
            Assert.IsNull(graph.GetNodeByName("InvalidNodeName"), "GetNodeByName should return null.");
            Assert.IsNotNull(graph.GetNodeByName("Scene Root Node"), "GetNodeByName should not return null.");
            Assert.AreSame(light1, graph.GetNodeByName("LightNode1"), "GetNodeByName should return light1.");
            Assert.AreSame(light2, graph.GetNodeByName("LightNode2"), "GetNodeByName should return light2.");
            Assert.AreSame(node1, graph.GetNodeByName("TestNode1"), "GetNodeByName should return node1.");
            Assert.AreSame(node2, graph.GetNodeByName("TestNode2"), "GetNodeByName should return node2.");
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestValidateSceneFail()
        {
            TestNode node3 = new TestNode("TestNode1");
            graph.AddNode(node1);
            node1.AddChild(node2);
            node2.AddChild(node3);
            graph.AddNode(light1);
            light1.AddChild(light2);
            graph.Validate();
        }

        [Test]
        public void TestValidateSceneOk()
        {
            graph.AddNode(node1);
            node1.AddChild(node2);
            graph.AddNode(light1);
            light1.AddChild(light2);
            graph.Validate();
        }

    }
}
