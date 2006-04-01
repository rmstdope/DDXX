using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace SceneGraph
{
    public class Node : Physics.WorldState
    {
        private Node parent = null;

        private String name;

        public Node(String name) : base()
        {
            this.name = name;
        }

        public Node(String name, Node parent) : base()
        {
            this.name = name;
            this.parent = parent;
        }

        internal object GetParent()
        {
            return parent;
        }

        public override Matrix GetWorldMatrix()
        {
            if (parent != null)
                return base.GetWorldMatrix() * parent.GetWorldMatrix();
            else
                return base.GetWorldMatrix();
        }
    
        internal object GetName()
        {
            return name;
        }
    }
}
