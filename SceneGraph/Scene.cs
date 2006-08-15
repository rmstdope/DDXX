using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Graphics;
using Utility;

namespace SceneGraph
{
    public class Scene
    {
        private NodeBase rootNode;
        private Camera activeCamera;
        private IDevice device;

        public Scene()
        {
            rootNode = new DummyNode("Scene Root Node");
            device = D3DDriver.GetInstance().GetDevice();
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
            if (ActiveCamera == null)
                throw new DDXXException("Must have an active camera set before a scene can be rendered.");
            device.SetTransform(TransformType.View, ActiveCamera.GetViewMatrix());
            device.SetTransform(TransformType.Projection, ActiveCamera.GetProjectionMatrix());

            rootNode.Render();
        }
    }
}
