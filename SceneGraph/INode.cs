using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;

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
        void RemoveChild(INode node);
        bool HasChild(INode node);
        void SetLightState(LightState state);
        void Step();
        void Render(IScene scene, DrawPass drawPass);
        int CountNodes();
        INode GetNumber(int number);
        Vector3 Position { get; }
        DrawPass DrawPass { get; set; }
    }
}
