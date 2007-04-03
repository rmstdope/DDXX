using System;
using Microsoft.DirectX;

namespace Dope.DDXX.Physics
{
    public interface IBoundingObject
    {
        Vector3 Center { get; set; }
        Vector3 ConstrainOutside(Vector3 position);
        bool IsInside(Vector3 position);
    }
}
