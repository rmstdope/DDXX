using System;
using System.Collections.Generic;
using System.Text;
using Utility;

namespace SceneGraph
{
    public class Scene
    {
        private NodeBase rootNode;
        private Camera activeCamera;

        public Scene()
        {
            rootNode = new DummyNode("Scene Root Node");
        }

        public Camera ActiveCamera
        {
            get { return activeCamera; }
            set
            {
                if (rootNode.HasChild(value))
                    activeCamera = value;
                else
                    throw new DDXXException("The active camera must be part of the scene graph.");
            }
        }

        public void AddNode(INode node1)
        {
            rootNode.AddChild(node1);
        }

        public void Step()
        {
            rootNode.Step();
        }

        public void Render()
        {
        }
    }
}
