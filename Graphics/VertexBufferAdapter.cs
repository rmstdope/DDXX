using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public class VertexBufferAdapter :IVertexBuffer
    {
        private VertexBuffer vertexBuffer;

        public VertexBufferAdapter(VertexBuffer buffer)
        {
            vertexBuffer = buffer;
        }

        public VertexBuffer DXVertexBuffer { get { return vertexBuffer; } }

        #region IVertexBuffer Members

        public VertexBufferDescription Description
        {
            get { return vertexBuffer.Description; }
        }

        public bool Disposed
        {
            get { return vertexBuffer.Disposed; }
        }

        public int SizeInBytes
        {
            get { return vertexBuffer.SizeInBytes; }
        }

        public Array Lock(int offsetToLock, LockFlags flags)
        {
            return vertexBuffer.Lock(offsetToLock, flags);
        }

        public IGraphicsStream Lock(int offsetToLock, int sizeToLock, LockFlags flags)
        {
            return new GraphicsStreamAdapter(vertexBuffer.Lock(offsetToLock, sizeToLock, flags));
        }

        public Array Lock(int offsetToLock, Type typeVertex, LockFlags flags, params int[] ranks)
        {
            return vertexBuffer.Lock(offsetToLock, typeVertex, flags, ranks);
        }

        public void SetData(object data, int lockAtOffset, LockFlags flags)
        {
            vertexBuffer.SetData(data, lockAtOffset, flags);
        }

        public void Unlock()
        {
            vertexBuffer.Unlock();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            vertexBuffer.Dispose();
        }

        #endregion
    }
}
