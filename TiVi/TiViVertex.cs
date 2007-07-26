using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace TiVi
{
    public struct TiViVertex
    {
        public TiViVertex(int i)
        {
            Position = new Vector3();
            BlendWeight1 = 0;
            BlendWeight2 = 0;
            BlendWeight3 = 0;
            BlendIndices = 0;
            Normal = new Vector3();
            U = 0;
            V = 0;
            //BiNormal = new Vector3();
            //Tangent = new Vector3();
        }
        public VertexFormats VertexFormats
        {
            get
            {
                return VertexFormats.PositionBlend4 | VertexFormats.LastBetaUByte4 |
                    VertexFormats.Normal | VertexFormats.Texture0 |
                    (VertexFormats)(1 << (int)VertexFormats.TextureCountShift);
            }
        }
        public Vector3 Position;
        public float BlendWeight1;
        public float BlendWeight2;
        public float BlendWeight3;
        public UInt32 BlendIndices;
        public Vector3 Normal;
        public float U;
        public float V;
        //public Vector3 BiNormal;
        //public Vector3 Tangent;
    }
}
