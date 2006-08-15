using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NUnit.Framework;
using NMock2;
using Graphics;
using Utility;

namespace SceneGraph
{
    [TestFixture]
    public class SceneTest : D3DMockTest
    {
        private Scene graph;
        private INode node1;
        private INode node2;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            node1 = mockery.NewMock<INode>();
            node2 = mockery.NewMock<INode>();

            DeviceDescription desc = new DeviceDescription();
            desc.deviceType = DeviceType.Hardware;
            D3DDriver.GetInstance().Initialize(null, desc);

            graph = new Scene();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void StepTest()
        {
            graph.AddNode(node1);
            graph.AddNode(node2);

            Expect.Once.On(node1).Method("Step");
            Expect.Once.On(node2).Method("Step");

            graph.Step();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void RenderTestFail()
        {
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.Render();
        }

        [Test]
        public void RenderTest()
        {
            Camera camera = new Camera("Camera");
            graph.AddNode(node1);
            graph.AddNode(node2);
            graph.AddNode(camera);
            graph.ActiveCamera = camera;

            Expect.Once.On(device).
                Method("SetTransform").
                With(TransformType.View, camera.GetViewMatrix());
            Expect.Once.On(device).
                Method("SetTransform").
                With(TransformType.Projection, camera.GetProjectionMatrix());
            Expect.Once.On(node1).
                Method("Render");
            Expect.Once.On(node2).
                Method("Render");
            graph.Render();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void CameraTestFail()
        {
            Camera camera = new Camera("Camera");
            graph.ActiveCamera = camera;
        }

        [Test]
        public void CameraTestOK()
        {
            Camera camera = new Camera("Camera");
            graph.AddNode(camera);
            graph.ActiveCamera = camera;
            Assert.AreSame(camera, graph.ActiveCamera);
        }
    }
}
