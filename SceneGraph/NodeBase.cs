using System;
using System.Collections.Generic;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.SceneGraph
{
    public abstract class NodeBase : INode
    {
        private NodeBase parent = null;
        private WorldState worldState = new WorldState();
        private String name;
        private List<INode> children = new List<INode>();
        private bool visible;

        public NodeBase(String name)
            : base()
        {
            this.name = name;
            this.visible = true;
        }

        protected virtual void SetLightStateNode(LightState state)
        {
        }

        protected abstract void StepNode();
        protected abstract void RenderNode(IScene scene);

        #region INode Members

        public string Name
        {
            get { return name; }
        }

        public INode Parent
        {
            get { return parent; }
        }

        public List<INode> Children
        {
            get { return children; }
        }

        public Matrix WorldMatrix
        {
            get
            {
                Matrix localMatrix;
                localMatrix = WorldState.GetWorldMatrix();
                if (parent != null)
                    return localMatrix * parent.WorldMatrix;
                return localMatrix;
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

        public void RemoveChild(INode child)
        {
            children.Remove(child);

            if (child is NodeBase)
            {
                NodeBase dummyChild = (NodeBase)child;
                dummyChild.parent = null;
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

        public void Render(IScene scene)
        {
            if (visible)
            {
                RenderNode(scene);

                BeforeRenderingChildren();
                foreach (INode node in children)
                {
                    node.Render(scene);
                }
                AfterRenderingChildren();
            }
        }

        protected virtual void AfterRenderingChildren()
        {
        }

        protected virtual void BeforeRenderingChildren()
        {
        }

        public void SetLightState(LightState state)
        {
            SetLightStateNode(state);

            foreach (INode node in children)
            {
                node.SetLightState(state);
            }
        }

        public INode GetNumber(int number)
        {
            if (number == 0)
                return this;
            foreach (INode child in Children)
            {
                if (number <= child.CountNodes())
                    return child.GetNumber(number - 1);
                number -= child.CountNodes();
                //if (number == 0)
                //    return child;
            }
            return null;
        }

        public int CountNodes()
        {
            int num = 1;
            foreach (INode node in children)
            {
                num += node.CountNodes();
            }
            return num;
        }

        public Vector3 Position
        {
            get
            {
                Matrix m = WorldMatrix;
                return new Vector3(m.M41, m.M42, m.M43);
            }
            set
            {
                WorldState.Position = value;
            }
        }

        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

        #endregion

    }
}
