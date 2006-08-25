using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public interface ICamera : INode
    {
        Matrix ProjectionMatrix
        {
            get;
        }
        Matrix ViewMatrix
        {
            get;
        }
    }
}
