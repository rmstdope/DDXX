using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public interface IRenderableCamera : INode
    {
        Matrix ProjectionMatrix
        {
            get;
        }
        Matrix ViewMatrix
        {
            get;
        }
        void SetClippingPlanes(float near, float far);
        void SetFOV(float fov);
        float GetFOV();
    }
}
