using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.SceneGraph
{
    public class Scene : IScene
    {
        private NodeBase rootNode;
        private IRenderableCamera activeCamera;
        private Color ambientColor;
        private LightState lightState;

        public Scene()
        {
            rootNode = new DummyNode("Scene Root Node");
            ambientColor = new Color(200, 200, 200);
        }

        public int NumNodes
        {
            get
            {
                return rootNode.CountNodes();
            }
        }

        public void AddNode(INode node)
        {
            rootNode.AddChild(node);
        }

        public void Step()
        {
            rootNode.Step();

            lightState = new LightState();
            rootNode.SetLightState(lightState);

            Vector3 eyePos = ActiveCamera.Position;
        }

        public void Render()
        {
            if (ActiveCamera == null)
                throw new DDXXException("Must have an active camera set before a scene can be rendered.");

            rootNode.Render(this);
        }

        public IRenderableCamera ActiveCamera
        {
            get { return activeCamera; }
            set
            {
                if (GetNodeByName(value.Name) != null)
                {
                    activeCamera = value;
                }
                else
                {
                    throw new DDXXException("The active camera must be part of the scene graph.");
                }
            }
        }

        public Color AmbientColor
        {
            get { return ambientColor; }
            set { ambientColor = value; }
        }

        public INode GetNodeByName(string name)
        {
            return FindNodeByName(rootNode, name, null);
        }

        private INode FindNodeByName(INode node, string name, INode exclude)
        {
            if (node != exclude && node.Name == name)
                return node;
            foreach (INode child in node.Children)
            {
                INode result = FindNodeByName(child, name, exclude);
                if (result != null)
                    return result;
            }
            return null;
        }

        public void Validate()
        {
            ValidateNode(rootNode);
        }

        private void ValidateNode(INode node)
        {
            if (FindNodeByName(rootNode, node.Name, node) != null)
                throw new DDXXException("Two nodes with the same name (" + node.Name + 
                    ") exist in the same Scene.");
            foreach (INode child in node.Children)
            {
                ValidateNode(child);
            }
        }

        //public void DebugPrintGraph()
        //{
        //    PrintNode(rootNode, 0);
        //}

        //private void PrintNode(INode node, int indent)
        //{
        //    for (int i = 0; i < indent; i++)
        //        Debug.Write('|');
        //    Debug.WriteLine(node.Name);
        //    foreach (INode child in node.Children)
        //    {
        //        PrintNode(child, indent + 1);
        //    }
        //}

    }
}
