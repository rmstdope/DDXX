using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using NMock2;
using Dope.DDXX.Graphics.Skinning;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class XLoaderTest : D3DMockTest, IScene
    {
        private XLoader loader;
        private IAnimationRootFrame hierarchy;
        private IFrame rootFrame;
        private IFrame rootChild1;
        private List<INode> nodes;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            hierarchy = mockery.NewMock<IAnimationRootFrame>();
            rootFrame = mockery.NewMock<IFrame>();
            rootChild1 = mockery.NewMock<IFrame>();
            nodes = new List<INode>();

            Stub.On(hierarchy).GetProperty("FrameHierarchy").
                Will(Return.Value(rootFrame));
            Stub.On(rootFrame).GetProperty("Name").
                Will(Return.Value("RootFrame"));
            Stub.On(rootChild1).GetProperty("Name").
                Will(Return.Value("RootChild1"));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        /// <summary>
        /// Test loading of a X file.
        /// </summary>
        [Test]
        public void TestLoad()
        {
            Expect.Once.On(factory).Method("LoadHierarchy").
                With(Is.EqualTo("file.x"), Is.EqualTo(device), Is.NotNull, Is.NotNull).
                Will(Return.Value(hierarchy));
            loader = new XLoader(factory, device, "file.x");
            loader.Load();
        }

        /// <summary>
        /// Test that Loading of a root frame only
        /// </summary>
        [Test]
        public void TestOneNode()
        {
            TestLoad();

            Expect.Once.On(rootFrame).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Expect.Once.On(rootFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            loader.AddToScene(this);
            Assert.AreEqual(1, nodes.Count, "One node should have been added.");
            Assert.AreEqual(0, nodes[0].Children.Count, "Added node should have no children.");
        }

        /// <summary>
        /// Test that Loading of two Cameras
        /// </summary>
        [Test]
        public void TestTwoNodes()
        {
            TestLoad();

            Stub.On(rootFrame).GetProperty("FrameFirstChild").Will(Return.Value(rootChild1));
            Stub.On(rootFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(rootChild1).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(rootChild1).GetProperty("FrameSibling").Will(Return.Value(null));
            loader.AddToScene(this);
            Assert.AreEqual(1, nodes.Count, "One node should have been added.");
            Assert.AreEqual(1, nodes[0].Children.Count, "Added node should have one child.");
            Assert.AreEqual(0, nodes[0].Children[0].Children.Count, "Child node should have no children.");
        }

        #region IScene Members

        public IRenderableCamera ActiveCamera
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public Microsoft.DirectX.Direct3D.ColorValue AmbientColor
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public int NumNodes
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void AddNode(INode node)
        {
            nodes.Add(node);
        }

        public void Step()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Render()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
