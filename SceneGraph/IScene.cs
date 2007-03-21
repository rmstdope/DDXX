using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    public interface IScene
    {
        IRenderableCamera ActiveCamera { get; set; }
        ColorValue AmbientColor { get; set; }
        int NumNodes { get; }
        void AddNode(INode node);
        void Step();
        void Render();
    }
}
