using System;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.ModelBuilder
{
    public interface IModelDirector
    {
        ModelBuilder ModelBuilder { get; }
        void Amplitude(AmplitudeFunction function);
        void CreateBox(float width, float length, float height);
        void CreateChamferBox(float width, float length, float height, float fillet, int filletSegments);
        void CreateCylinder(float radius, int segments, float height, int heightSegments, bool lid, int wrapU, int wrapV);
        void CreateDisc(float outerRadius, float innerRadius, int segments);
        void CreatePlane(float width, float height, int widthSegments, int heightSegments);
        void CreateSphere(float radius, int rings);
        void CreateTerrain(ITextureGenerator generator, float heightScale, float width, float height, int widthSegments, int heightSegments, bool textured);
        void CreateTorus(float smallRadius, float largeRadius, int side, int segments);
        void CreateTube(float innerRadius, float outerRadius, float height, int segments, int heightSegments);
        void CreateTunnel(float radius, int segments, float height, int heightSegments, int wrapU, int wrapV, bool mirrorTexture);
        CustomModel Generate(string materialName);
        CustomModel Generate(MaterialHandler material);
        void HeightMap(ITextureGenerator generator);
        void NormalFlip();
        void Rotate(double x, double y, double z);
        void Scale(float factor);
        void Scale(float x, float y, float z);
        void Translate(float x, float y, float z);
        void UvMapBox();
        void UvMapPlane(int alignToAxis, int tileU, int tileV);
        void UvMapSphere();
        void UvRemap(float translateU, float translateV, float scaleU, float scaleV);
    }
}
