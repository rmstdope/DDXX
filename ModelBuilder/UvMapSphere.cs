using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class UvMapSphere : ModifierBase
    {
        public UvMapSphere()
            : base(1)
        {
        }

        public override IPrimitive Generate()
        {
            IPrimitive primitive = GetInput(0);
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                Vector3 normal = primitive.Vertices[i].Position;
                if (normal != Vector3.Zero)
                    normal.Normalize();
                primitive.Vertices[i].U = (float)(Math.Asin(normal.X) / Math.PI) + 0.5f;
                primitive.Vertices[i].V = (float)(Math.Asin(normal.Y) / Math.PI) + 0.5f;
            }
            return primitive;
        }
    }
}
