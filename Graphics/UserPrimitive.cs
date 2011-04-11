using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class UserPrimitive<T> : IDisposable
        where T:struct
    {
        private MaterialHandler material;
        private PrimitiveType primitiveType;
        private VertexDeclaration vertexDeclaration;
        private int numVertices;
        private int bufferSize;
        private T[] vertices;

        public UserPrimitive(VertexDeclaration vertexDeclaration, MaterialHandler material, PrimitiveType primitiveType, int bufferSize)
        {
            this.material = material;
            this.primitiveType = primitiveType;
            this.bufferSize = bufferSize;
            this.vertices = new T[bufferSize];
            this.vertexDeclaration = vertexDeclaration;
        }

        public MaterialHandler Material 
        {
            get { return material; }
        }

        public int BufferSize 
        {
            get { return bufferSize; }
        }

        public int NumPrimitives(int numVertices)
        {
            if (primitiveType == PrimitiveType.LineList)
                return numVertices / 2;
            else if (primitiveType == PrimitiveType.LineStrip)
                return numVertices - 1;
            else if (primitiveType == PrimitiveType.TriangleList)
                return numVertices / 3;
            else if (primitiveType == PrimitiveType.TriangleStrip)
                return numVertices - 2;
            throw new Exception("The method or operation is not implemented.");
        }

        public void Begin()
        {
            numVertices = 0;
        }

        public void End()
        {
            if (numVertices > 0)
            {
                foreach (EffectPass pass in material.Effect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    vertexDeclaration.GraphicsDevice.DrawUserPrimitives<T>(primitiveType, vertices, 0, NumPrimitives(numVertices), vertexDeclaration);
                }
                numVertices = 0;
            }
        }

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddVertex(T vertex)
        {
            if (NumPrimitives(numVertices) == NumPrimitives(bufferSize))
            {
                if (primitiveType == PrimitiveType.LineStrip)
                {
                    T nextStart = vertices[numVertices - 1];
                    End();
                    vertices[numVertices++] = nextStart;
                }
                else if (primitiveType == PrimitiveType.TriangleStrip)
                {
                    T nextStart1 = vertices[numVertices - 2];
                    T nextStart2 = vertices[numVertices - 1];
                    End();
                    vertices[numVertices++] = nextStart1;
                    vertices[numVertices++] = nextStart2;
                }
                else
                {
                    End();
                }
            }
            vertices[numVertices++] = vertex;
        }
    }
}
