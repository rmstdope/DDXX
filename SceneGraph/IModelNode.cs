using System;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public interface IModelNode : INode
    {
        CullMode CullMode { get; set; }
        IModel Model { get; set; }
    }
}
