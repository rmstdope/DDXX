using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class UserPrimitive<T> : IUserPrimitive<T>, IDisposable
        where T:struct
    {
        private IMaterialHandler material;
        private PrimitiveType primitiveType;
        private IVertexDeclaration vertexDeclaration;
        private int numVertices;
        private int bufferSize;
        private T[] vertices;

        public UserPrimitive(IVertexDeclaration vertexDeclaration, IMaterialHandler material, PrimitiveType primitiveType, int bufferSize)
        {
            this.material = material;
            this.primitiveType = primitiveType;
            this.bufferSize = bufferSize;
            this.vertices = new T[bufferSize];
            this.vertexDeclaration = vertexDeclaration;
        }

        public int NumPrimitives(int numVertices)
        {
            if (primitiveType == PrimitiveType.LineList)
                return numVertices / 2;
            else if (primitiveType == PrimitiveType.PointList)
                return numVertices;
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
                vertexDeclaration.GraphicsDevice.VertexDeclaration = vertexDeclaration;
                material.Effect.Begin();
                foreach (IEffectPass pass in material.Effect.CurrentTechnique.Passes)
                {
                    pass.Begin();
                    vertexDeclaration.GraphicsDevice.DrawUserPrimitives<T>(primitiveType, vertices, 0, NumPrimitives(numVertices));
                    pass.End();
                }
                material.Effect.End();
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
