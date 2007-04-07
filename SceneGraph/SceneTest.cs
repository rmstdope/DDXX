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
using Dope.DDXX.Graphics.Skinning;

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
        private LightNode light1;
        private LightNode light2;
        private EffectHandle lightDiffuse;
        private EffectHandle lightSpecular;
        private EffectHandle lightPosition;
        private EffectHandle eyePosition;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            node1 = new TestNode("TestNode1");
            node2 = new TestNode("TestNode2");
            light1 = new PointLightNode("LightNode1");
            light2 = new PointLightNode("LightNode2");
            effect = mockery.NewMock<IEffect>();

            lightDiffuse = EffectHandle.FromString("LightDiffuseColors");
            lightSpecular = EffectHandle.FromString("LightSpecularColors");
            lightPosition = EffectHandle.FromString("LightPositions");
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
            Expect.Once.On(factory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(lightSpecular));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(eyePosition));
            graph = new Scene();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestConstructorFail1()
        {
            Expect.Once.On(factory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(lightSpecular));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(eyePosition));
            graph = new Scene();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestConstructorFail2()
        {
            Expect.Once.On(factory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(lightSpecular));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(eyePosition));
            graph = new Scene();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestConstructorFail3()
        {
            Expect.Once.On(factory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(eyePosition));
            graph = new Scene();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestConstructorFail4()
        {
            Expect.Once.On(factory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightPositions").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColors").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColors").
                Will(Return.Value(lightSpecular));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "EyePosition").
                Will(Return.Value(null));
            graph = new Scene();
        }

        [Test]
        public void TestStep()
        {
            TestConstructorOK();

            ColorValue diffuse1 = new ColorValue(0.11f, 0.21f, 0.31f, 0.41f);
            ColorValue specular1 = new ColorValue(0.12f, 0.22f, 0.32f, 0.42f);
            Vector3 position1 = new Vector3(0.13f, 0.23f, 0.33f);
            ColorValue diffuse2 = new ColorValue(0.111f, 0.211f, 0.311f, 0.411f);
            ColorValue specular2 = new ColorValue(0.121f, 0.221f, 0.321f, 0.421f);
            Vector3 position2 = new Vector3(0.131f, 0.231f, 0.331f);
            light1.DiffuseColor = diffuse1;
            light1.SpecularColor = specular1;
            light1.Position = position1;
            light2.DiffuseColor = diffuse2;
            light2.SpecularColor = specular2;
            light2.Position = position2;
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
                With(eyePosition, new Vector4(1, 2, 3, 1));

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
            IAnimationRootFrame hierarchy2 = mockery.NewMock<IAnimationRootFrame>();
            IAnimationController controller2 = mockery.NewMock<IAnimationController>();
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
            Expect.Once.On(controller1).Method("AdvanceTime").With((double)Time.DeltaTime);
            Expect.Once.On(controller2).Method("AdvanceTime").With((double)Time.DeltaTime);
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
