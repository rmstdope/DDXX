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

        protected override Vector4 GetPixel()
        {
            float s1 = GetInputPixel(0, -1, 0).X;
            float s2 = GetInputPixel(0, 0, -1).X;
            float s3 = GetInputPixel(0, 1, 0).X;
            float s4 = GetInputPixel(0, 0, 1).X;

            Vector3 normal = new Vector3((s1 - s3), 1.0f, (s2 - s4));
            normal.Normalize();

            return new Vector4(normal, 0);
        }
    }
}
