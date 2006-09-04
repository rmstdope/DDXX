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
        private Scene graph;
        private INode node1;
        private INode node2;
        private LightNode light1;
        private LightNode light2;
        private IEffect effect;
        private EffectHandle lightDiffuse;
        private EffectHandle lightSpecular;
        private EffectHandle lightPosition;
        private EffectHandle eyePosition;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            node1 = mockery.NewMock<INode>();
            node2 = mockery.NewMock<INode>();
            light1 = new LightNode("LightNode", new Light());
            light2 = new LightNode("LightNode", new Light());
            effect = mockery.NewMock<IEffect>();

            lightDiffuse = EffectHandle.FromString("LightDiffuseColor");
            lightSpecular = EffectHandle.FromString("LightSpecularColor");
            lightPosition = EffectHandle.FromString("LightPosition");
            eyePosition = EffectHandle.FromString("EyePosition");

            DeviceDescription desc = new DeviceDescription();
            desc.deviceType = DeviceType.Hardware;
            D3DDriver.GetInstance().Initialize(null, desc);
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
                With(null, "LightPosition").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColor").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColor").
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
                With(null, "LightPosition").
                Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColor").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColor").
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
                With(null, "LightPosition").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColor").
                Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColor").
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
                With(null, "LightPosition").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColor").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColor").
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
                With(null, "LightPosition").
                Will(Return.Value(lightPosition));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightDiffuseColor").
                Will(Return.Value(lightDiffuse));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "LightSpecularColor").
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
            light1.Light.DiffuseColor = diffuse1;
            light1.Light.SpecularColor = specular1;
            light1.Light.Position = position1;
            light2.Light.DiffuseColor = diffuse2;
            light2.Light.SpecularColor = specular2;
            light2.Light.Position = position2;

            CameraNode camera = new CameraNode("Camera");
            camera.WorldState.Position = new Vector3(1, 2, 3);
            graph.AddNode(camera);
            graph.ActiveCamera = camera;

            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(light1);
            graph.AddNode(light2);

            Expect.Once.On(node1).
                Method("Step");
            Expect.Once.On(node2).
                Method("Step");
            Expect.Once.On(node1).
                Method("SetLightState");
            Expect.Once.On(node2).
                Method("SetLightState");
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

            Expect.Once.On(node1).
                Method("Render");
            Expect.Once.On(node2).
                Method("Render");
            graph.Render();
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
    }
}
