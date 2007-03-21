using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Graphics.Skinning;

namespace Dope.DDXX.SceneGraph
{
    public class XLoader
    {
        private DdxxAllocateHierarchy allocateHierarchy = new DdxxAllocateHierarchy();
        private IAnimationRootFrame rootFrame;

        private IGraphicsFactory factory;
        private IDevice device;
        private string filename;

        /// <summary>
        /// Constructor. Will not perform the actual loading.
        /// </summary>
        /// <param name="filename">The file name of the X file.</param>
        public XLoader(IGraphicsFactory factory, IDevice device, string filename)
        {
            this.factory = factory;
            this.device = device;
            this.filename = filename;
        }

        public void Load()
        {
            DdxxLoadUserData loadUserData = new DdxxLoadUserData();
            rootFrame = factory.LoadHierarchy(filename, device, allocateHierarchy, loadUserData);
        }

        public void AddToScene(IScene scene)
        {
            scene.AddNode(AddToScene(rootFrame.FrameHierarchy, null, scene));
        }

        private INode AddToScene(IFrame frame, INode parentNode, IScene scene)
        {
            INode node = new DummyNode(frame.Name);
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
    }
}
