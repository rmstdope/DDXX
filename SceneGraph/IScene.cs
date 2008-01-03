using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public interface IScene
    {
        IRenderableCamera ActiveCamera { get; set; }
        Color AmbientColor { get; set; }
        int NumNodes { get; }
        void AddNode(INode node);
        void Step();
        void Render();
        INode GetNodeByName(string name);
        void Validate();
    }
}