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
        INode Parent { get; }
        WorldState WorldState { get; }
        Matrix WorldMatrix { get; }
        void AddChild(INode child);
        bool HasChild(INode node);
        void Step();
        void Render(CameraNode camera);
    }
}
