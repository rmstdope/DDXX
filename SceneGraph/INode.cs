using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.SceneGraph
{
    public interface INode
    {
        string Name { get; }
        INode Parent { get; }
        List<INode> Children { get; }
        WorldState WorldState { get; }
        Matrix WorldMatrix { get; }
        void AddChild(INode child);
        bool HasChild(INode node);
        void SetLightState(LightState state);
        void Step();
        void Render(IScene scene);
        int CountNodes();
    }
}
