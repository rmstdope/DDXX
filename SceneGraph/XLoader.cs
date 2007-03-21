using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Graphics.Skinning;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    public class XLoader
    {
        private DdxxAllocateHierarchy allocateHierarchy = new DdxxAllocateHierarchy();
        private IAnimationRootFrame rootFrame;

        private IGraphicsFactory factory;
        private INodeFactory nodeFactory;
        private IDevice device;
        private IEffect effect;
        private string filename;
        private string techniquePrefix;

        /// <summary>
        /// Constructor. Will not perform the actual loading.
        /// </summary>
        /// <param name="filename">The file name of the X file.</param>
        public XLoader(IGraphicsFactory factory, INodeFactory nodeFactory, IDevice device, string filename)
        {
            this.factory = factory;
            this.nodeFactory = nodeFactory;
            this.device = device;
            this.filename = filename;
        }

        public void Load(IEffect effect, string techniquePrefix)
        {
            this.effect = effect;
            this.techniquePrefix = techniquePrefix;
            rootFrame = factory.LoadHierarchy(filename, device, allocateHierarchy, null);
        }

        public void AddToScene(IScene scene)
        {
            scene.AddNode(AddToScene(rootFrame.FrameHierarchy, null, scene));
        }

        private INode AddToScene(IFrame frame, INode parentNode, IScene scene)
        {
            INode node = CreateNode(frame);
            if (frame.FrameFirstChild != null)
            {
                node.AddChild(AddToScene(frame.FrameFirstChild, node, scene));
            }
            if (frame.FrameSibling != null)
            {
                parentNode.AddChild(AddToScene(frame.FrameSibling, parentNode, scene));
            }
            return node;
        }

        private INode CreateNode(IFrame frame)
        {
            INode node;
            if (frame.Mesh != null)
            {
                node = nodeFactory.CreateModelNode(frame, effect, techniquePrefix);
            }
            else if (frame.Name != null && frame.Name.ToLower().Contains("camera"))
                node = nodeFactory.CreateCameraNode(frame);
            else
                node = nodeFactory.CreateDummyNode(frame);
            return node;
        }
    }
}
