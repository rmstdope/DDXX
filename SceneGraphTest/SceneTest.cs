using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

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
        private EffectHandle numLights;
        private EffectHandle lightDiffuse;
        private EffectHandle lightSpecular;
        private EffectHandle lightPosition;
        private EffectHandle lightDirection;
        private EffectHandle eyePosition;

        private ColorValue diffuse1 = new ColorValue(0.11f, 0.21f, 0.31f, 0.41f);
        private ColorValue specular1 = new ColorValue(0.12f, 0.22f, 0.32f, 0.42f);
        private Vector3 position1 = new Vector3(0.13f, 0.23f, 0.33f);
        private ColorValue diffuse2 = new ColorValue(0.111f, 0.211f, 0.311f, 0.411f);
        private ColorValue specular2 = new ColorValue(0.121f, 0.221f, 0.321f, 0.421f);
        private Vector3 direction2 = new Vector3(0.131f, 0.231f, 0.331f);

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            node1 = new TestNode("TestNode1");
            node2 = new TestNode("TestNode2");
            light1 = new PointLightNode("LightNode1");
            light2 = new DirectionalLightNode("LightNode2");
            effect = mockery.NewMock<IEffect>();

            numLights = EffectHandle.FromString("NumLights");
            lightDiffuse = EffectHandle.FromString("LightDiffuseColors");
            lightSpecular = EffectHandle.FromString("LightSpecularColors");
            lightPosition = EffectHandle.FromString("LightPositions");
            lightDirection = EffectHandle.FromString("LightDirections");
            eyePosition = EffectHandle.FromString("EyePosition");

            DeviceDescription desc = new DeviceDescription();
            desc.deviceType = DeviceType.Hardware;
            Expect.Once.On(prerequisits).Method("CheckPrerequisits").WithAnyArguments();
            D3DDriver.GetInstance().Initialize(null, desc, prerequisits);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestConstructorOK()
        {
            Expect.Once.On(graphicsFactory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDirections").
                Will(Return.Value(lightDirection));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(lightSpecular));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "NumLights").
                Will(Return.Value(numLights));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(eyePosition));
            graph = new Scene();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestConstructorFail1()
        {
            Expect.Once.On(graphicsFactory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDirections").
                Will(Return.Value(lightDirection));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(lightSpecular));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "NumLights").
                Will(Return.Value(numLights));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(eyePosition));
            graph = new Scene();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestConstructorFail2()
        {
            Expect.Once.On(graphicsFactory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDirections").
                Will(Return.Value(lightDirection));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(lightSpecular));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "NumLights").
                Will(Return.Value(numLights));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(eyePosition));
            graph = new Scene();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestConstructorFail3()
        {
            Expect.Once.On(graphicsFactory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDirections").
                Will(Return.Value(lightDirection));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "NumLights").
                Will(Return.Value(numLights));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(eyePosition));
            graph = new Scene();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestConstructorFail4()
        {
            Expect.Once.On(graphicsFactory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDirections").
                Will(Return.Value(lightDirection));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(lightSpecular));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "NumLights").
                Will(Return.Value(numLights));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(null));
            graph = new Scene();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestConstructorFail5()
        {
            Expect.Once.On(graphicsFactory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDirections").
                Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(lightSpecular));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "NumLights").
                Will(Return.Value(numLights));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(eyePosition));
            graph = new Scene();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestConstructorFail6()
        {
            Expect.Once.On(graphicsFactory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDirections").
                Will(Return.Value(lightDirection));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(lightSpecular));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(eyePosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "NumLights").
                Will(Return.Value(null));
            graph = new Scene();
        }

        [Test]
        public void TestStepWithLights()
        {
            TestConstructorOK();

            light1.DiffuseColor = diffuse1;
            light1.SpecularColor = specular1;
            light1.Position = position1;
            light2.DiffuseColor = diffuse2;
            light2.SpecularColor = specular2;
            light2.Direction = direction2;
            Assert.AreEqual(1, graph.NumNodes);

            CameraNode camera = new CameraNode("Camera");
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

        [Test]
        public void TestHierarchyStep()
        {
            IAnimationRootFrame hierarchy1 = mockery.NewMock<IAnimationRootFrame>();
            IAnimationController controller1 = mockery.NewMock<IAnimationController>();
            IAnimationSet animationSet1 = mockery.NewMock<IAnimationSet>();
            IAnimationRootFrame hierarchy2 = mockery.NewMock<IAnimationRootFrame>();
            IAnimationController controller2 = mockery.NewMock<IAnimationController>();
            IAnimationSet animationSet2 = mockery.NewMock<IAnimationSet>();
            IAnimationRootFrame hierarchy3 = mockery.NewMock<IAnimationRootFrame>();
            CameraNode camera = new CameraNode("Camera");

            TestConstructorOK();

            Time.Initialize();
            Time.Step();
            graph.AddNode(camera);
            graph.ActiveCamera = camera;
            graph.HandleHierarchy(hierarchy1);
            graph.HandleHierarchy(hierarchy2);
            graph.HandleHierarchy(hierarchy3);

            Stub.On(effect).Method("SetValue");
            Stub.On(hierarchy1).GetProperty("AnimationController").Will(Return.Value(controller1));
            Stub.On(hierarchy2).GetProperty("AnimationController").Will(Return.Value(controller2));
            Stub.On(hierarchy3).GetProperty("AnimationController").Will(Return.Value(null));
            Stub.On(controller1).GetProperty("Time").Will(Return.Value(2.5));
            Stub.On(controller2).GetProperty("Time").Will(Return.Value(3.0));
            Stub.On(animationSet1).GetProperty("Period").Will(Return.Value(2.0));
            Stub.On(animationSet1).GetProperty("Period").Will(Return.Value(4.0));
            Stub.On(controller1).Method("GetAnimationSet").With(0).Will(Return.Value(animationSet1));
            Stub.On(controller2).Method("GetAnimationSet").With(0).Will(Return.Value(animationSet1));
            Expect.Once.On(controller1).Method("AdvanceTime").With(1.5);
            Expect.Once.On(controller1).Method("AdvanceTime").With((double)Time.StepTime);
            Expect.Once.On(controller2).Method("AdvanceTime").With(1.0);
            Expect.Once.On(controller2).Method("AdvanceTime").With((double)Time.StepTime);
            graph.Step();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestRenderNoCamera()
        {
            TestConstructorOK();

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.Render();
        }

        public void ExpectNoLights(Vector4 eyePos)
        {
            Expect.Once.On(effect).
                Method("SetValue").
                With(numLights, 0);
            Expect.Once.On(effect).
                Method("SetValue").
                With(lightDiffuse, new ColorValue[] { });
            Expect.Once.On(effect).
                Method("SetValue").
                With(lightSpecular, new ColorValue[] { });
            Expect.Once.On(effect).
                Method("SetValue").
                With(lightPosition, new Vector3[] { });
            Expect.Once.On(effect).
                Method("SetValue").
                With(lightDirection, new Vector3[] { });
            Expect.Once.On(effect).
                Method("SetValue").
                With(eyePosition, eyePos);
        }

        [Test]
        public void TestRenderWithLights()
        {
            TestStepWithLights();

            Expect.Once.On(effect).
                Method("SetValue").
                With(numLights, 2);
            Expect.Once.On(effect).
                Method("SetValue").
                With(Is.EqualTo(lightDiffuse), Is.EqualTo(new ColorValue[] { diffuse1, diffuse2 }));
            Expect.Once.On(effect).
                Method("SetValue").
                With(Is.EqualTo(lightSpecular), Is.EqualTo(new ColorValue[] { specular1, specular2 }));
            Expect.Once.On(effect).
                Method("SetValue").
                With(Is.EqualTo(lightPosition), Is.Anything);
            Expect.Once.On(effect).
                Method("SetValue").
                With(Is.EqualTo(lightDirection), Is.Anything);
            Expect.Once.On(effect).
                Method("SetValue").
                With(eyePosition, new Vector4(1, 2, 3, 1));

            graph.Render();
        }

        [Test]
        public void TestRenderOK()
        {
            TestConstructorOK();

            CameraNode camera = new CameraNode("Camera");
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(camera);
            graph.AddNode(light1);
            graph.ActiveCamera = camera;

            ExpectNoLights(new Vector4(0, 0, 0, 1));

            graph.Render();
            Assert.IsTrue(node1.renderCalled, "Render() should have been called.");
            Assert.IsTrue(node2.renderCalled, "Render() should have been called.");
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestActiveCameraFail()
        {
            TestConstructorOK();

            CameraNode camera = new CameraNode("Camera");
            graph.ActiveCamera = camera;
        }

        [Test]
        public void TestActiveCamera()
        {
            TestConstructorOK();

            CameraNode camera = new CameraNode("Camera");
            graph.AddNode(camera);
            graph.ActiveCamera = camera;
            Assert.AreSame(camera, graph.ActiveCamera);
        }

        [Test]
        public void TestActiveCameraAsChild()
        {
            TestConstructorOK();

            CameraNode camera = new CameraNode("Camera");
            node1.AddChild(camera);
            graph.AddNode(node1);
            graph.ActiveCamera = camera;
            Assert.AreSame(camera, graph.ActiveCamera);
        }

        [Test]
        public void TestGetNodeByName()
        {
            TestConstructorOK();

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
            TestConstructorOK();

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
            TestConstructorOK();

            graph.AddNode(node1);
            node1.AddChild(node2);
            graph.AddNode(light1);
            light1.AddChild(light2);
            graph.Validate();
        }

    }
}
