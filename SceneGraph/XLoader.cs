using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    public class XLoader : IXLoader
    {
        private DdxxAllocateHierarchy allocateHierarchy = new DdxxAllocateHierarchy();
        private IAnimationRootFrame rootFrame;

        private IGraphicsFactory factory;
        private INodeFactory nodeFactory;
        private IDevice device;
        private IEffect effect;
        private string filename;
        private MeshTechniqueChooser techniquePrefix;

        public IAnimationRootFrame RootFrame
        {
            get { return rootFrame; }
        }

        /// <summary>
        /// Constructor. Will not perform the actual loading.
        /// </summary>
        /// <param name="filename">The file name of the X file.</param>
        public XLoader(IGraphicsFactory factory, INodeFactory nodeFactory, IDevice device)
        {
            this.factory = factory;
            this.nodeFactory = nodeFactory;
            this.device = device;
        }

        public void Load(string filename, IEffect effect, MeshTechniqueChooser techniquePrefix)
        {
            this.filename = filename;
            this.effect = effect;
            this.techniquePrefix = techniquePrefix;
            rootFrame = factory.LoadHierarchy(filename, device, allocateHierarchy, null);
        }

        public void AddToScene(IScene scene)
        {
            scene.AddNode(AddToScene(rootFrame.FrameHierarchy, null));
            //scene.HandleHierarchy(rootFrame);
        }

        private INode AddToScene(IFrame frame, INode parentNode)
        {
            INode node = CreateNode(frame);
            if (frame.FrameFirstChild != null)
            {
                node.AddChild(AddToScene(frame.FrameFirstChild, node));
            }
            if (frame.FrameSibling != null)
            {
                parentNode.AddChild(AddToScene(frame.FrameSibling, parentNode));
            }
            return node;
        }

        private INode CreateNode(IFrame frame)
        {
            INode node;
            if (frame.Mesh != null)
            {
                if (frame.SkinInformation != null)
                    node = nodeFactory.CreateSkinnedModelNode(rootFrame, frame, effect, techniquePrefix);
                else
                    node = nodeFactory.CreateModelNode(frame, effect, techniquePrefix);
            }
            else if (frame.Name != null && frame.Name.ToLower().Contains("camera"))
                node = nodeFactory.CreateCameraNode(frame);
            else
                node = nodeFactory.CreateDummyNode(frame);
            return node;
        }

        public List<INode> GetNodeHierarchy()
        {
            List<INode> nodes = new List<INode>();
            GetNodeHierarchy(rootFrame.FrameHierarchy, null, nodes);
            return nodes;
        }

        private INode GetNodeHierarchy(IFrame frame, INode parentNode, List<INode> nodes)
        {
            INode node = CreateNode(frame);
            if (parentNode == null)
                nodes.Add(node);
            else
                parentNode.AddChild(node);
            if (frame.FrameFirstChild != null)
            {
                node.AddChild(AddToScene(frame.FrameFirstChild, node));
            }
            if (frame.FrameSibling != null)
            {
                parentNode.AddChild(AddToScene(frame.FrameSibling, parentNode));
            }
            return node;
        }
    }
}
