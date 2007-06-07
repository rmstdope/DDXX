using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    public class Bricks : Generator
    {
        private float brickWidth;
        private float brickHeight;
        private float gapWidth;
        private int yBrick;

        private enum RowType
        {
            NORMAL_LAYER,
            SHIFTED_LAYER,
            GAP
        }

        public Bricks(int numBricksX, int numBricksY, float gapWidth)
            : base(0)
        {
            float width = 1.0f / numBricksX;
            this.brickWidth = width - gapWidth;
            float height = 1.0f / numBricksY;
            this.brickHeight = height - gapWidth;
            this.gapWidth = gapWidth;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            PerlinNoise g = new PerlinNoise();
            g.NumOctaves = 2;
            g.BaseFrequency = 8;
            g.Persistence = 0.5f;
            textureCoordinate.X += 0.01f * g.GetPixel(textureCoordinate, texelSize).X;
            textureCoordinate.Y += 0.01f * g.GetPixel(textureCoordinate, texelSize).X;
            RowType row = GetRowType(textureCoordinate.Y);
            if (row == RowType.GAP)
                return new Vector4();
            else
                return GetBlockColor(row, textureCoordinate.X);
        }

        private Vector4 GetBlockColor(RowType row, float x)
        {
            if (row == RowType.SHIFTED_LAYER)
                x += (brickWidth + gapWidth) / 2.0f;
            int xBrick = (int)(x / (brickWidth + gapWidth));
            float rest = x - xBrick * (brickWidth + gapWidth);
            if (rest < brickWidth)
            {
                Random r = new Random(xBrick + yBrick * 256);
                float v = (float)(r.NextDouble() * 0.4);
                if (r.NextDouble() < 0.1f)
                    v -= 0.3f;
                return new Vector4(1 + v, 1 + v, 1 + v, 1);
            }
            return new Vector4();
        }

        private RowType GetRowType(float y)
        {
            yBrick = (int)(y / (brickHeight + gapWidth));
            float rest = y - yBrick * (brickHeight + gapWidth);
            if (rest < brickHeight)
            {
                if ((yBrick % 2) == 0)
                    return RowType.NORMAL_LAYER;
                else
                    return RowType.SHIFTED_LAYER;
            }
            return RowType.GAP;
        }
    }
}
