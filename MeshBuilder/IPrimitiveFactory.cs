using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Dope.DDXX.TextureBuilder;

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
        IPrimitive CreateSphere(float radius, short Nu, short Nv);
        IPrimitive CreateSphere2(float radius, int rings);
        IPrimitive CreateChamferBox(float length, float width, float height, float fillet,
            int lengthSegments, int widthSegments, int heightSegments, int filletSegments);
        IPrimitive CreateTerrain(IGenerator heightMapGenerator, float heightScale, float width, float height, int widthSegments, int heightSegments, bool textured);
    }
}
