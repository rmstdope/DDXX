using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    internal interface IPrimitiveFactory
    {
        IPrimitive CreateCloth(IBody body, float width, float height,
           int widthSegments, int heightSegments, int[] pinnedParticles, bool textured);
        IPrimitive CreateCloth(IBody body, float width, float height,
            int widthSegments, int heightSegments, bool textured);
        IPrimitive CreatePlane(float width, float height,
            int widthSegments, int heightSegments, bool textured);
        IPrimitive CreateBox(float length, float width, float height,
            int lengthSegments, int widthSegments, int heightSegments);
    }
}
