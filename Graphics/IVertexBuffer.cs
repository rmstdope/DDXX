using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public interface IVertexBuffer : IDisposable
    {
        // Summary:
        //     Retrieves a description of the vertex buffer resource.
        VertexBufferDescription Description { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        bool Disposed { get; }
        //
        // Summary:
        //     Retrieves the size of the Microsoft.DirectX.Direct3D.VertexBuffer data, in
        //     bytes.
        int SizeInBytes { get; }
        //
        // Summary:
        //     Locks a range of vertex data and obtains the vertex buffer memory.
        //
        // Parameters:
        //   offsetToLock:
        //     Offset into the vertex data to lock, in bytes. To lock the entire vertex
        //     buffer, specify 0 for both Microsoft.DirectX.Direct3D.VertexBuffer.Lock()
        //     and Microsoft.DirectX.Direct3D.VertexBuffer.Lock().
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags locking flags that describe
        //     the type of lock to perform. For this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     Microsoft.DirectX.Direct3D.LockFlags.ReadOnly, and Microsoft.DirectX.Direct3D.LockFlags.
        //     For a description of the flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        // Returns:
        //     An System.Array that represents the locked vertex buffer.
        Array Lock(int offsetToLock, LockFlags flags);
        //
        // Summary:
        //     Locks a range of vertex data and obtains the vertex buffer memory.
        //
        // Parameters:
        //   offsetToLock:
        //     Offset into the vertex data to lock, in bytes. To lock the entire vertex
        //     buffer, specify 0 for both Microsoft.DirectX.Direct3D.VertexBuffer.Lock()
        //     and Microsoft.DirectX.Direct3D.VertexBuffer.Lock().
        //
        //   sizeToLock:
        //     Size of the vertex data to lock, in bytes. To lock the entire vertex buffer,
        //     specify 0 for both Microsoft.DirectX.Direct3D.VertexBuffer.Lock() and Microsoft.DirectX.Direct3D.VertexBuffer.Lock().
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags locking flags that describe
        //     the type of lock to perform. For this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     Microsoft.DirectX.Direct3D.LockFlags.ReadOnly, and Microsoft.DirectX.Direct3D.LockFlags.
        //     For a description of the flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that represents the locked vertex
        //     buffer.
        IGraphicsStream Lock(int offsetToLock, int sizeToLock, LockFlags flags);
        //
        // Summary:
        //     Locks a range of vertex data and obtains the vertex buffer memory.
        //
        // Parameters:
        //   offsetToLock:
        //     Offset into the vertex data to lock, in bytes. To lock the entire vertex
        //     buffer, specify 0 for both Microsoft.DirectX.Direct3D.VertexBuffer.Lock()
        //     and Microsoft.DirectX.Direct3D.VertexBuffer.Lock().
        //
        //   typeVertex:
        //     A System.Type object that indicates the type of array data to return. This
        //     can be a value type or any type that contains only value types.
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags locking flags that describe
        //     the type of lock to perform. For this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     Microsoft.DirectX.Direct3D.LockFlags.ReadOnly, and Microsoft.DirectX.Direct3D.LockFlags.
        //     For a description of the flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returning System.Array.
        //
        // Returns:
        //     An System.Array that represents the locked vertex buffer.
        Array Lock(int offsetToLock, Type typeVertex, LockFlags flags, params int[] ranks);
        //
        // Summary:
        //     Locks, sets, and unlocks a range of vertex data.
        //
        // Parameters:
        //   data:
        //     An System.Object that contains the data to copy into the vertex buffer. This
        //     can be any value type or any type that contains only value types.
        //
        //   lockAtOffset:
        //     Offset in the vertex buffer to set. To set the entire buffer, set this parameter
        //     to 0.
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags locking flags that describe
        //     the type of lock to perform when setting the buffer. For this method, the
        //     valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard, Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock, Microsoft.DirectX.Direct3D.LockFlags.ReadOnly,
        //     and Microsoft.DirectX.Direct3D.LockFlags. For a description of the flags,
        //     see Microsoft.DirectX.Direct3D.LockFlags.
        void SetData(object data, int lockAtOffset, LockFlags flags);
        //
        // Summary:
        //     Unlocks vertex data.
        void Unlock();
    }
}
