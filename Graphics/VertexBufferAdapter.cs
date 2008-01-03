using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class VertexBufferAdapter : GraphicsResourceAdapter, IVertexBuffer
    {
        private VertexBuffer vertexBuffer;

        public VertexBufferAdapter(VertexBuffer vertexBuffer)
            : base(vertexBuffer)
        {
            this.vertexBuffer = vertexBuffer;
        }

        public VertexBuffer DxVertexBuffer { get { return vertexBuffer; } }

        #region IVertexBuffer Members

        public BufferUsage BufferUsage
        {
            get { return vertexBuffer.BufferUsage; }
        }

        public int SizeInBytes
        {
            get { return vertexBuffer.SizeInBytes; }
        }

        public void GetData<T>(T[] data) 
            where T:struct
        {
            vertexBuffer.GetData<T>(data);
        }

        public void GetData<T>(T[] data, int startIndex, int elementCount)
            where T : struct
        {
            vertexBuffer.GetData<T>(data, startIndex, elementCount);
        }

        public void GetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride)
            where T : struct
        {
            vertexBuffer.GetData<T>(offsetInBytes, data, startIndex, elementCount, vertexStride);
        }

        public void SetData<T>(T[] data)
            where T : struct
        {
            vertexBuffer.SetData<T>(data);
        }

        public void SetData<T>(T[] data, int startIndex, int elementCount)
            where T : struct
        {
            vertexBuffer.SetData<T>(data, startIndex, elementCount);
        }

        public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride)
            where T : struct
        {
            vertexBuffer.SetData<T>(offsetInBytes, data, startIndex, elementCount, vertexStride);
        }

        #endregion
    }
}
