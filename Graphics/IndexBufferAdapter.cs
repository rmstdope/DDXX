using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class IndexBufferAdapter : IIndexBuffer
    {
        private IndexBuffer indexBuffer;

        public IndexBufferAdapter(IndexBuffer buffer)
        {
            indexBuffer = buffer;
        }

        #region IIndexBuffer Members

        public IndexBufferDescription Description
        {
            get { return indexBuffer.Description; }
        }

        public int SizeInBytes
        {
            get { return indexBuffer.SizeInBytes; }
        }

        public Array Lock(int offsetToLock, LockFlags flags)
        {
            return indexBuffer.Lock(offsetToLock, flags);
        }

        public IGraphicsStream Lock(int offsetToLock, int sizeToLock, LockFlags flags)
        {
            return new GraphicsStreamAdapter(indexBuffer.Lock(offsetToLock, sizeToLock, flags));
        }

        public Array Lock(int offsetToLock, Type typeIndex, LockFlags flags, params int[] ranks)
        {
            return indexBuffer.Lock(offsetToLock, typeIndex, flags, ranks);
        }

        public void SetData(object data, int lockAtOffset, LockFlags flags)
        {
            indexBuffer.SetData(data, lockAtOffset, flags);
        }

        public void Unlock()
        {
            indexBuffer.Unlock();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            indexBuffer.Dispose();
        }

        #endregion
    }
}
