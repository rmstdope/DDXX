using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Physics;
using Utility;

namespace SceneGraph
{
    public abstract class NodeBase : INode
    {
        private NodeBase parent = null;
        private WorldState worldState = new WorldState();
        private String name;
        private List<INode> children = new List<INode>();

        public NodeBase(String name) : base()
        {
            this.name = name;
        }

        protected abstract void StepNode();
        protected abstract void RenderNode();

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

            if (child is NodeBase)
            {
                NodeBase dummyChild = (NodeBase)child;
                dummyChild.parent = this;
            }
        }

        public bool HasChild(INode node)
        {
            if (children.Exists(delegate(INode node2) { return node == node2; }))
                return true;
            else
                return false;
        }

        public void Step()
        {
            StepNode();

            foreach (INode node in children)
            {
                node.Step();
            }
        }

        public void Render()
        {
            RenderNode();

            foreach (INode node in children)
            {
                node.Render();
            }
        }

        #endregion

    }
}