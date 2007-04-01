using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.MeshBuilder
{
    public interface IPrimitveFactory
    {
        Primitive CreateBox(float length, float width, float height,
            int lengthSegments, int widthSegments, int heightSegments);
    }
}
