using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.ModelBuilder
{
    public class NormalFlip : ModifierBase
    {
        public NormalFlip()
            : base(1)
        {
        }

        public override IPrimitive Generate()
        {
            IPrimitive primitive = GetInput(0);
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                Vertex vertex = primitive.Vertices[i];
                vertex.Normal = -vertex.Normal;
                primitive.Vertices[i] = vertex;
            }
            for (int i = 0; i < primitive.Indices.Length; i += 3)
            {
                short i2 = primitive.Indices[i+ 1];
                short i3 = primitive.Indices[i + 2];
                primitive.Indices[i + 1] = i3;
                primitive.Indices[i + 2] = i2;
            }
            return primitive;
        }
    }
}
