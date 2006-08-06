using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Physics;

namespace SceneGraph
{
    public interface INode
    {
        string Name { get; }
        NodeBase Parent { get; }
        WorldState WorldState { get; }
        Matrix WorldMatrix { get; }
        void AddChild(NodeBase child);
    }
}
