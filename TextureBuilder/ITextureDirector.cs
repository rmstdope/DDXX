using System;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.TextureBuilder
{
    public interface ITextureDirector
    {
        ITextureFactory TextureFactory { get; }
        void Add();
        void AddGenerator(ITextureGenerator generator);
        void ColorBlend(Vector4 zeroColor, Vector4 oneColor);
        void CreateBricks(int numBricksX, int numBricksY, float gapWidth);
        void CreateBrushNoise(int pointsPerLine);
        void CreateCircle(float solidRadius, float gradientRadius1, float gradientRadius2, float gradientBreak, Vector2 center);
        void CreateConstant(Vector4 color);
        void CreatePerlinNoise(int baseFrequncy, int numOctaves, float persistance);
        void CreateSquare(float size);
        void FactorBlend(float factor);
        void FromFile(string filename);
        void GaussianBlur();
        ITexture2D Generate(string name, int width, int height, int numMipLevels, SurfaceFormat format);
        ITexture2D GenerateChain(int width, int height);
        void Madd(float mul, float add);
        void Modulate();
        void ModulateColor(Vector4 color);
        void NormalMap();
        void Subtract();
    }
}
