using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class RoundedRectangle : Generator
    {
        private Vector4 corners;
        private Vector4 innerCorners;
        private float roundRadius;

        public RoundedRectangle(Vector2 size, Vector2 center, float roundRadius)
            : base(0)
        {
            corners = new Vector4(center.X - size.X / 2.0f, center.Y - size.Y / 2.0f, center.X + size.X / 2.0f, center.Y + size.Y / 2.0f);
            innerCorners = new Vector4(corners.X + roundRadius, corners.Y + roundRadius, corners.Z - roundRadius, corners.W - roundRadius);
            this.roundRadius = roundRadius;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            if (textureCoordinate.X < corners.X ||
                textureCoordinate.Y < corners.Y ||
                textureCoordinate.X > corners.Z ||
                textureCoordinate.Y > corners.W)
                return new Vector4(1, 1, 1, 1);
            float absDiffX1 = (float)Math.Abs(textureCoordinate.X - corners.X);
            float absDiffY1 = (float)Math.Abs(textureCoordinate.Y - corners.Y);
            float absDiffX2 = (float)Math.Abs(textureCoordinate.X - corners.Z);
            float absDiffY2 = (float)Math.Abs(textureCoordinate.Y - corners.W);
            // Top left
            if (absDiffX1 <= roundRadius && absDiffY1 <= roundRadius)
                if (new Vector2(innerCorners.X - textureCoordinate.X, innerCorners.Y - textureCoordinate.Y).Length() > roundRadius)
                    return new Vector4(1, 1, 1, 1);
            // Bottom left
            if (absDiffX1 <= roundRadius && absDiffY2 <= roundRadius)
                if (new Vector2(innerCorners.X - textureCoordinate.X, innerCorners.W - textureCoordinate.Y).Length() > roundRadius)
                    return new Vector4(1, 1, 1, 1);
            // Top right
            if (absDiffX2 <= roundRadius && absDiffY1 <= roundRadius)
                if (new Vector2(innerCorners.Z - textureCoordinate.X, innerCorners.Y - textureCoordinate.Y).Length() > roundRadius)
                    return new Vector4(1, 1, 1, 1);
            // Bottom right
            if (absDiffX2 <= roundRadius && absDiffY2 <= roundRadius)
                if (new Vector2(innerCorners.Z - textureCoordinate.X, innerCorners.W - textureCoordinate.Y).Length() > roundRadius)
                    return new Vector4(1, 1, 1, 1);
            return new Vector4();
        }
    }
}
