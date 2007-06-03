using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;

namespace TextureBuilder
{
    public class ColorModulationGenerator : IGenerator
    {
        private IGenerator generator;
        private Vector4 color;

        public ColorModulationGenerator(IGenerator generator, Color color)
        {
            this.generator = generator;
            this.color = new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }

        public Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector4 generatedColor = generator.GetPixel(textureCoordinate, texelSize);
            return new Vector4(color.X * generatedColor.X, color.Y * generatedColor.Y,
                color.Z * generatedColor.Z, color.W * generatedColor.W);
        }
    }
}
