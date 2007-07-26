using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface IIndexBuffer : IDisposable
    {
        // Summary:
        //     Retrieves a description of the index buffer resource.
        IndexBufferDescription Description { get; }
        int SizeInBytes { get; }
        //
        // Summary:
        //     Locks a range of index data and obtains a pointer to the index buffer memory.
        //
        // Parameters:
        //   offsetToLock:
        //     [in] Offset into the index data to lock, in bytes. To lock the entire index
        //     buffer, specify 0 for the sizeToLock and offsetToLock parameters.
        //
        //   flags:
        //     [in] Zero or more LockFlags that describe the type of lock to perform. For
        //     this method, the valid flags are Discard, NoDirtyUpdate, NoSystemLock, NoOverwrite,
        //     and ReadOnly.
        //
        // Returns:
        //     An Array that represents the locked index buffer.
        Array Lock(int offsetToLock, LockFlags flags);
        //
        // Summary:
        //     Locks a range of index data and obtains a pointer to the index buffer memory.
        //
        // Parameters:
        //   offsetToLock:
        //     [in] Offset into the index data to lock, in bytes. To lock the entire index
        //     buffer, specify 0 for the sizeToLock and offsetToLock parameters.
        //
        //   sizeToLock:
        //     [in] Size of the index data to lock, in bytes. To lock the entire index buffer,
        //     specify 0 for the param_Int32_sizeToLock and param_Int32_offsetToLock parameters.
        //
        //   flags:
        //     [in] Zero or more LockFlags that describe the type of lock to perform. For
        //     this method, the valid flags are Discard, NoDirtyUpdate, NoSystemLock, NoOverwrite,
        //     and ReadOnly.
        //
        // Returns:
        //     A GraphicsStream object that represents the locked index buffer.
        IGraphicsStream Lock(int offsetToLock, int sizeToLock, LockFlags flags);
        //
        // Summary:
        //     Locks a range of index data and obtains a pointer to the index buffer memory.
        //
        // Parameters:
        //   offsetToLock:
        //     [in] Offset into the index data to lock, in bytes. To lock the entire index
        //     buffer, specify 0 for the sizeToLock and offsetToLock parameters.
        //
        //   typeIndex:
        //     A Type object that indicates the type of array data to return. This can be
        //     a value type or any type that contains only value types.
        //
        //   flags:
        //     [in] Zero or more LockFlags that describe the type of lock to perform. For
        //     this method, the valid flags are Discard, NoDirtyUpdate, NoSystemLock, NoOverwrite,
        //     and ReadOnly.
        //
        //   ranks:
        //     [in] Array of 1 to 3 Int32 values that indicate the dimensions of the returning
        //     Array.
        //
        // Returns:
        //     An Array that represents the locked index buffer.
        Array Lock(int offsetToLock, Type typeIndex, LockFlags flags, params int[] ranks);
        //
        // Summary:
        //     Locks, sets, and unlocks a range of vertex data.
        //
        // Parameters:
        //   data:
        //     [in] An Object that contains the data to copy into the index buffer.
        //
        //   lockAtOffset:
        //     [in] Offset to set in the index buffer. To set the entire buffer, set this
        //     parameter to 0.
        //
        //   flags:
        //     [in] Zero or more LockFlags locking flags that describe the type of lock
        //     to perform when setting the buffer. For this method, the valid flags are
        //     Discard, NoDirtyUpdate, NoSystemLock, ReadOnly, and NoOverWrite. For a description
        //     of the flags, see LockFlags.
        void SetData(object data, int lockAtOffset, LockFlags flags);
        //
        // Summary:
        //     Unlocks index data.
        void Unlock();
    }
}
