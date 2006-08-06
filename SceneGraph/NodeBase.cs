using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Physics;

namespace SceneGraph
{
    public class NodeBase : INode
    {
        private NodeBase parent = null;
        private WorldState worldState = new WorldState();
        private String name;

        public NodeBase(String name) : base()
        {
            this.name = name;
        }

        #region INode Members

        public string Name
        {
            get { return name; }
        }

        public NodeBase Parent
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

        public void AddChild(NodeBase child)
        {
            child.parent = this;
        }

        #endregion

    }
}
