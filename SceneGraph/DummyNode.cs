using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Physics;
using Utility;

namespace SceneGraph
{
    public class DummyNode : INode
    {
        private DummyNode parent = null;
        private WorldState worldState = new WorldState();
        private String name;
        private List<INode> children = new List<INode>();

        public DummyNode(String name) : base()
        {
            this.name = name;
        }

        #region INode Members

        public string Name
        {
            get { return name; }
        }

        public  INode Parent
        {
            get { return parent; }
        }

        public Matrix WorldMatrix
        {
            get
            {
                if (parent != null)
                    return worldState.GetWorldMatrix() * parent.WorldMatrix;
                else
                    return worldState.GetWorldMatrix();
            }
        }

        public WorldState WorldState
        {
            get { return worldState; }
        }

        public void AddChild(INode child)
        {
            children.Add(child);

            if (child is DummyNode)
            {
                DummyNode dummyChild = (DummyNode)child;
                dummyChild.parent = this;
            }
        }

        public virtual void Step()
        {
            foreach (INode node in children)
            {
                node.Step();
            }
        }

        #endregion
    }
}
