using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Utility;

namespace SceneGraph
{
    [TestFixture]
    public class SceneTest
    {
        Scene graph;

        Mockery mockery;
        INode node1;
        INode node2;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            node1 = mockery.NewMock<INode>();
            node2 = mockery.NewMock<INode>();

            graph = new Scene();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
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
        public void RenderTest()
        {
            graph.AddNode(node1);
            graph.AddNode(node2);

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
