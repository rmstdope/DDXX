using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DreamBuildPlay2009
{
    public struct TerrainVertex
    {
        public Vector4 Position;
        public static int SizeInBytes = (4) * sizeof(float);
        
        public static VertexElement[] VertexElements = new VertexElement[]
         {
             new VertexElement( 0, 0, VertexElementFormat.Vector4, 
                                      VertexElementMethod.Default, 
                                      VertexElementUsage.Position, 0),
         };
    }
}
