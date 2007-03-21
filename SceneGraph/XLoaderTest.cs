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
        private IFrame rootChild2;
        private IFrame rootChild2Child1;
        private IFrame rootChild2Child2;
        private IMeshContainer meshContainer;
        private List<INode> nodes;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            hierarchy = mockery.NewMock<IAnimationRootFrame>();
            rootFrame = mockery.NewMock<IFrame>();
            rootChild1 = mockery.NewMock<IFrame>();
            rootChild2 = mockery.NewMock<IFrame>();
            rootChild2Child1 = mockery.NewMock<IFrame>();
            rootChild2Child2 = mockery.NewMock<IFrame>();
            meshContainer = mockery.NewMock<IMeshContainer>();
            nodes = new List<INode>();

            Stub.On(hierarchy).GetProperty("FrameHierarchy").
                Will(Return.Value(rootFrame));
            Stub.On(rootFrame).GetProperty("Name").
                Will(Return.Value("RootFrame"));
            Stub.On(rootFrame).GetProperty("MeshContainer").
                Will(Return.Value(null));
            Stub.On(rootChild1).GetProperty("Name").
                Will(Return.Value("RootChild1(Camera)"));
            Stub.On(rootChild1).GetProperty("MeshContainer").
                Will(Return.Value(null));
            Stub.On(rootChild2).GetProperty("Name").
                Will(Return.Value("RootChild2(Mesh)"));
            Stub.On(rootChild2).GetProperty("MeshContainer").
                Will(Return.Value(meshContainer));
            Stub.On(rootChild2Child1).GetProperty("Name").
                Will(Return.Value("RootChild2Child1(Mesh)"));
            Stub.On(rootChild2Child1).GetProperty("MeshContainer").
                Will(Return.Value(meshContainer));
            Stub.On(rootChild2Child2).GetProperty("Name").
                Will(Return.Value("RootChild2Sibling1(Camera)"));
            Stub.On(rootChild2Child2).GetProperty("MeshContainer").
                Will(Return.Value(null));
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
                With(Is.EqualTo("file.x"), Is.EqualTo(device), Is.NotNull, Is.Null).
                Will(Return.Value(hierarchy));
            loader = new XLoader(factory, device, "file.x");
            loader.Load();
        }

        /// <summary>
        /// Test loading of a root frame only
        /// </summary>
        [Test]
        public void TestOneNode()
        {
            TestLoad();

            Expect.Once.On(rootFrame).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Expect.Once.On(rootFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            loader.AddToScene(this);
            Assert.AreEqual(1, nodes.Count, "One node should have been added.");
            Assert.AreEqual(typeof(DummyNode), nodes[0].GetType(), "Root node shall be a dummy node.");
            Assert.AreEqual(0, nodes[0].Children.Count, "Added node should have no children.");
        }

        /// <summary>
        /// Test loading of a Camera 
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
            Assert.AreEqual(typeof(DummyNode), nodes[0].GetType(), "Root node shall be a dummy node.");
            Assert.AreEqual(1, nodes[0].Children.Count, "Added node should have one child.");
            Assert.AreEqual(typeof(CameraNode), nodes[0].Children[0].GetType(), "Child node shall be a camera node.");
            Assert.AreEqual(0, nodes[0].Children[0].Children.Count, "Child node should have no children.");
        }

        /// <summary>
        /// Test loading of two Cameras and two Meshes
        /// </summary>
        [Test]
        public void TestMoreNodes()
        {
            TestLoad();

            Stub.On(rootFrame).GetProperty("FrameFirstChild").Will(Return.Value(rootChild1));
            Stub.On(rootFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(rootChild1).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(rootChild1).GetProperty("FrameSibling").Will(Return.Value(rootChild2));
            Stub.On(rootChild2).GetProperty("FrameFirstChild").Will(Return.Value(rootChild2Child1));
            Stub.On(rootChild2).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(rootChild2Child1).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(rootChild2Child1).GetProperty("FrameSibling").Will(Return.Value(rootChild2Child2));
            Stub.On(rootChild2Child2).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(rootChild2Child2).GetProperty("FrameSibling").Will(Return.Value(null));
            loader.AddToScene(this);
            Assert.AreEqual(1, nodes.Count, "One node should have been added.");
            Assert.AreEqual(typeof(DummyNode), nodes[0].GetType(), "Root node shall be a dummy node.");
            Assert.AreEqual(2, nodes[0].Children.Count, "Added node should have two children.");
            Assert.AreEqual(typeof(ModelNode), nodes[0].Children[0].GetType(), "Child 1 shall be a model node.");
            Assert.AreEqual(typeof(CameraNode), nodes[0].Children[1].GetType(), "Child 2 shall be a camera node.");
            Assert.AreEqual(2, nodes[0].Children[0].Children.Count, "Child 1 should have two children.");
            Assert.AreEqual(0, nodes[0].Children[1].Children.Count, "Child 2 should have no children.");
            Assert.AreEqual(typeof(CameraNode), nodes[0].Children[0].Children[0].GetType(), "Child 1's child 1 shall be a cameranode.");
            Assert.AreEqual(typeof(ModelNode), nodes[0].Children[0].Children[1].GetType(), "Child 1's child 2 shall be a model node.");
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
