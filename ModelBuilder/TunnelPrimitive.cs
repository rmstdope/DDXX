using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.ModelBuilder
{
    public class TunnelPrimitive : CylinderPrimitive
    {
        public override IPrimitive Generate()
        {
            Lid = false;
            IPrimitive primitive = base.Generate();

            foreach (Vertex vertex in primitive.Vertices)
            {
                vertex.Normal = -vertex.Normal;
                vertex.BiNormal = -vertex.BiNormal;
                vertex.Tangent = -vertex.Tangent;
            }

            for (int i = 0; i < primitive.Indices.Length; i += 3)
            {
                short index = primitive.Indices[i + 1];
                primitive.Indices[i + 1] = primitive.Indices[i + 2];
                primitive.Indices[i + 2] = index;
            }

            return primitive;
        }
    }
}
