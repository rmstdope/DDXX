using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class IndexBufferAdapter : GraphicsResourceAdapter, IIndexBuffer
    {
        private IndexBuffer indexBuffer;

        public IndexBufferAdapter(IndexBuffer indexBuffer)
            : base(indexBuffer)
        {
            this.indexBuffer = indexBuffer;
        }

        public IndexBuffer DxIndexBuffer { get { return indexBuffer; } }

        #region IIndexBuffer Members

        public IndexElementSize IndexElementSize
        {
            get { return indexBuffer.IndexElementSize; }
        }

        public BufferUsage BufferUsage
        {
            get { return indexBuffer.BufferUsage; }
        }

        public int SizeInBytes
        {
            get { return indexBuffer.SizeInBytes; }
        }

        public void GetData<T>(T[] data)
            where T : struct
        {
            indexBuffer.GetData<T>(data);
        }

        public void GetData<T>(T[] data, int startIndex, int elementCount)
            where T : struct
        {
            indexBuffer.GetData<T>(data, startIndex, elementCount);
        }

        public void GetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount)
            where T : struct
        {
            indexBuffer.GetData<T>(offsetInBytes, data, startIndex, elementCount);
        }

        public void SetData<T>(T[] data)
            where T : struct
        {
            indexBuffer.SetData<T>(data);
        }

        public void SetData<T>(T[] data, int startIndex, int elementCount)
            where T : struct
        {
            indexBuffer.SetData<T>(data, startIndex, elementCount);
        }

        public void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount)
            where T : struct
        {
            indexBuffer.SetData<T>(offsetInBytes, data, startIndex, elementCount);
        }

        #endregion

    }
}
