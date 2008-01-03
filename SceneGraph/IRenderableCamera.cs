using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.SceneGraph
{
    public interface IRenderableCamera : INode
    {
        Matrix ProjectionMatrix { get; }
        Matrix ViewMatrix { get; }
        float AspectRatio { get; set; }
    }
}
