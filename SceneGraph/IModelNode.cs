using System;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public interface IModelNode : INode
    {
        RasterizerState RasterizerState { get; set; }
        DepthStencilState DepthStencilState { get; set; }
        CustomModel Model { get; set; }
    }
}
