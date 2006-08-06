using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;

namespace SceneGraph
{
    [TestFixture]
    public class SceneGraphTest
    {
        SceneGraph graph;

        Mockery mockery;
        INode node1;
        INode node2;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            node1 = mockery.NewMock<INode>();
            node2 = mockery.NewMock<INode>();

            graph = new SceneGraph();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void NodeTest()
        {
            graph.AddNode(node1);
            graph.AddNode(node2);

            Expect.Once.On(node1).Method("Step");
            Expect.Once.On(node2).Method("Step");

            graph.Step();
        }
    }
}
