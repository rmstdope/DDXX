using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class CustomModelMeshPart
    {
        //private Effect effect;
        private IndexBuffer indexBuffer;
        private int numVertices;
        private int primitiveCount;
        private PrimitiveType primitiveType;
        private int startIndex;
        //private object tag;
        private VertexBuffer vertexBuffer;
        private int vertexOffset;
        private MaterialHandler materialHandler;

        //public Effect Effect { get { return effect; } set { effect = value; } }
        public VertexBuffer VertexBuffer { get { return vertexBuffer; } }
        public int VertexOffset { get { return vertexOffset; } }
        public int NumVertices { get { return numVertices; } }
        public IndexBuffer IndexBuffer { get { return indexBuffer; } }
        public int StartIndex { get { return startIndex; } }
        public int PrimitiveCount { get { return primitiveCount; } }
        public PrimitiveType PrimitiveType { get { return primitiveType; } }
        public MaterialHandler MaterialHandler { get { return materialHandler; } }
        //public object Tag { get; set; }

        public CustomModelMeshPart(VertexBuffer vertexBuffer, int vertexOffset, int numVertices, 
            IndexBuffer indexBuffer, int startIndex, 
            int primitiveCount, PrimitiveType primitiveType,
            MaterialHandler material)
        {
            this.vertexBuffer = vertexBuffer;
            this.vertexOffset = vertexOffset;
            this.numVertices = numVertices;
            this.indexBuffer = indexBuffer;
            this.startIndex = startIndex;
            this.primitiveCount = primitiveCount;
            this.primitiveType = primitiveType;
            this.materialHandler = material;
        }
    }
}
