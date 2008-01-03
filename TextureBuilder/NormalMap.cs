using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    public class NormalMap : Generator
    {
        public NormalMap()
            : base(1)
        {
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            float s1 = GetInputPixel(0, textureCoordinate + new Vector2(-texelSize.X, 0), texelSize).X;
            float s2 = GetInputPixel(0, textureCoordinate + new Vector2(0, -texelSize.Y), texelSize).X;
            float s3 = GetInputPixel(0, textureCoordinate + new Vector2(texelSize.X, 0), texelSize).X;
            float s4 = GetInputPixel(0, textureCoordinate + new Vector2(0, texelSize.Y), texelSize).X;

            Vector3 normal = new Vector3((s1 - s3), 1.0f, (s2 - s4));
            normal.Normalize();

            return new Vector4(normal, 0);
        }
    }
}
