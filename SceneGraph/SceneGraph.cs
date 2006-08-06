using System;
using System.Collections.Generic;
using System.Text;

namespace SceneGraph
{
    public class SceneGraph
    {
        private DummyNode rootNode;

        public SceneGraph()
        {
            rootNode = new DummyNode("Scene Root Node");
        }


        internal void AddNode(INode node1)
        {
            rootNode.AddChild(node1);
        }

        internal void Step()
        {
            rootNode.Step();
        }
    }
}
