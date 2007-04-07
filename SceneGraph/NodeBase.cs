using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics.Skinning;

namespace Dope.DDXX.SceneGraph
{
    public abstract class NodeBase : INode
    {
        private NodeBase parent = null;
        private WorldState worldState = new WorldState();
        private String name;
        private List<INode> children = new List<INode>();
        private IFrame frame;

        public NodeBase(String name) : base()
        {
            this.name = name;
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

        public  INode Parent
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
                if (frame != null)
                {
                    localMatrix = frame.TransformationMatrix;
                }
                else
                    localMatrix = worldState.GetWorldMatrix();
                if (parent != null)
                    return localMatrix * parent.WorldMatrix;
                else
                    return localMatrix;
            }
        }

        public WorldState WorldState
        {
            get 
            {
                if (frame != null)
                    throw new DDXXException("NodeBase.WorldState can not be used for a node which is in frame mode.");
                return worldState; 
            }
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

        public void Render(IScene scene)
        {
            RenderNode(scene);

            foreach (INode node in children)
            {
                node.Render(scene);
            }
        }

        public void SetLightState(LightState state)
        {
            SetLightStateNode(state);

            foreach (INode node in children)
            {
                node.SetLightState(state);
            }
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

        public void EnableFrameHandling(IFrame frame)
        {
            this.frame = frame;
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

        #endregion
    }
}
