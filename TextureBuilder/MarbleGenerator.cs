using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace TextureBuilder
{
    public class MarbleGenerator : IGenerator
    {
        private IGenerator perlinGenerator;

        public MarbleGenerator(int numOctaves, float baseFrequency, float persistance)
        {
            perlinGenerator = new PerlinNoiseGenerator(numOctaves, baseFrequency, persistance);
        }

        public Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float value = perlinGenerator.GetPixel(textureCoordinate, texelSize).X;
            value = (float)Math.Cos((textureCoordinate.X / 256 + value) * 12 * Math.PI);
            value = (value + 1) / 2.0f;
            return new Vector4(value, value, value, value);
        }
    }
}
