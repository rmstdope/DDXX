using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using NMock2;
using Microsoft.DirectX.Direct3D;

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
        private IMesh mesh1;
        private IMesh mesh2;
        private ISkinInformation skin1;
        private List<INode> nodes;
        private IEffectHandler effectHandler;
        private INodeFactory nodeFactory;
        private DummyNode rootNode;
        private CameraNode rootChild1Node;
        private ModelNode rootChild2Node;
        private ModelNode rootChild2Child1Node;
        private CameraNode rootChild2Child2Node;
        //private IAnimationRootFrame addedHierarchy;
        private MeshTechniqueChooser prefix = TechniqueChooser.MeshPrefix("Prefix");
       
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
            mesh1 = mockery.NewMock<IMesh>();
            mesh2 = mockery.NewMock<IMesh>();
            skin1 = mockery.NewMock<ISkinInformation>();
            effectHandler = mockery.NewMock<IEffectHandler>();
            nodeFactory = mockery.NewMock<INodeFactory>();
            nodes = new List<INode>();
            rootNode = new DummyNode("dummyNode");
            rootChild1Node = new CameraNode("cameraNode");
            rootChild2Node = new ModelNode("modelNode", null, null, null);
            rootChild2Child1Node = new ModelNode("modelNode", null, null, null);
            rootChild2Child2Node = new CameraNode("cameraNode");

            Stub.On(hierarchy).GetProperty("FrameHierarchy").
                Will(Return.Value(rootFrame));
            Stub.On(rootFrame).GetProperty("Name").
                Will(Return.Value("RootFrame"));
            Stub.On(rootFrame).GetProperty("Mesh").
                Will(Return.Value(null));
            Stub.On(rootChild1).GetProperty("Name").
                Will(Return.Value("RootChild1(Camera)"));
            Stub.On(rootChild1).GetProperty("Mesh").
                Will(Return.Value(null));
            Stub.On(rootChild2).GetProperty("Name").
                Will(Return.Value("RootChild2(Mesh)"));
            Stub.On(rootChild2).GetProperty("Mesh").
                Will(Return.Value(mesh1));
            Stub.On(rootChild2).GetProperty("SkinInformation").
                Will(Return.Value(skin1));
            Stub.On(rootChild2Child1).GetProperty("Name").
                Will(Return.Value("RootChild2Child1(Mesh)"));
            Stub.On(rootChild2Child1).GetProperty("Mesh").
                Will(Return.Value(mesh2));
            Stub.On(rootChild2Child1).GetProperty("SkinInformation").
                Will(Return.Value(null));
            Stub.On(rootChild2Child2).GetProperty("Name").
                Will(Return.Value("RootChild2Sibling1(Camera)"));
            Stub.On(rootChild2Child2).GetProperty("Mesh").
                Will(Return.Value(null));
            Stub.On(nodeFactory).Method("CreateDummyNode").With(rootFrame).
                Will(Return.Value(rootNode));
            Stub.On(nodeFactory).Method("CreateCameraNode").With(rootChild1).
                Will(Return.Value(rootChild1Node));
            Stub.On(nodeFactory).Method("CreateSkinnedModelNode").
                With(hierarchy, rootChild2, effect, prefix).
                Will(Return.Value(rootChild2Node));
            Stub.On(nodeFactory).Method("CreateModelNode").
                With(rootChild2Child1, effect, prefix).
                Will(Return.Value(rootChild2Child1Node));
            Stub.On(nodeFactory).Method("CreateCameraNode").With(rootChild2Child2).
                Will(Return.Value(rootChild2Child2Node));
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
            Expect.Once.On(graphicsFactory).Method("LoadHierarchy").
                With(Is.EqualTo("file.x"), Is.EqualTo(device), Is.NotNull, Is.Null).
                Will(Return.Value(hierarchy));
            loader = new XLoader(graphicsFactory, nodeFactory, device);
            loader.Load("file.x", effect, prefix);
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
            //Assert.AreSame(hierarchy, addedHierarchy, "hierarchy should have been added to the scene.");
        }

        /// <summary>
        /// Test loading of a root frame only
        /// </summary>
        [Test]
        public void TestNodeHierarchyOneNode()
        {
            TestLoad();

            Expect.Once.On(rootFrame).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Expect.Once.On(rootFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            nodes = loader.GetNodeHierarchy();
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
            //Stub.On(effect).Method("FindNextValidTechnique").Will(Return.Value(technique));

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
            //Assert.AreSame(hierarchy, addedHierarchy, "hierarchy should have been added to the scene.");
        }

        /// <summary>
        /// Test loading of a Camera 
        /// </summary>
        [Test]
        public void TestGetHierarchyTwoNodes()
        {
            TestLoad();

            Stub.On(rootFrame).GetProperty("FrameFirstChild").Will(Return.Value(rootChild1));
            Stub.On(rootFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(rootChild1).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(rootChild1).GetProperty("FrameSibling").Will(Return.Value(null));
            nodes = loader.GetNodeHierarchy();
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
            //Assert.AreSame(hierarchy, addedHierarchy, "hierarchy should have been added to the scene.");
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

        public INode GetNodeByName(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Validate()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DebugPrintGraph()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void HandleHierarchy(IAnimationRootFrame hierarchy)
        {
            throw new Exception("The method or operation is not implemented.");
            //addedHierarchy = hierarchy;
        }

        #endregion

        #region IScene Members


        public void SetEffectParameters()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IScene Members


        public void RemoveNode(INode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
