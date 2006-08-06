using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;

namespace SceneGraph
{
    [TestFixture]
    class SceneGraphTest
    {
        SceneGraph graph;

        [SetUp]
        public void SetUp()
        {
            graph = new SceneGraph();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void CameraTest()
        {
            //graph.AddCamera();
        }
    }
}
