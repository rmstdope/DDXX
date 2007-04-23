using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface ITexture : IBaseTexture, IDisposable
    {
        void FillTexture(Fill2DTextureCallback callbackFunction);

        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        bool Disposed { get; }
        // Summary:
        //     Adds a dirty region to a texture resource.
        void AddDirtyRectangle();
        //
        // Summary:
        //     Adds a dirty region to a texture resource.
        //
        // Parameters:
        //   rect:
        //     A System.Drawing.Rectangle structure that specifies the dirty region to add.
        void AddDirtyRectangle(Rectangle rect);
        //
        // Summary:
        //     Retrieves a level description of a texture resource.
        //
        // Parameters:
        //   level:
        //     Level of the texture resource. This method returns a surface description
        //     for the level specified by this parameter.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.SurfaceDescription structure that describes
        //     the returned level.
        SurfaceDescription GetLevelDescription(int level);
        //
        // Summary:
        //     Retrieves the specified texture surface level.
        //
        // Parameters:
        //   level:
        //     Level of the texture resource. This method returns a surface for the level
        //     specified by this parameter. The top-level surface is denoted by 0.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the returned
        //     surface.
        ISurface GetSurfaceLevel(int level);
        //
        // Summary:
        //     Locks a rectangle on a texture resource.
        //
        // Parameters:
        //   level:
        //     A level of the texture resource to lock.
        //
        //   flags:
        //     Zero or more locking flags that describe the type of lock to perform. For
        //     this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that describes the locked region.
        GraphicsStream LockRectangle(int level, LockFlags flags);
        //
        // Summary:
        //     Locks a rectangle on a texture resource.
        //
        // Parameters:
        //   level:
        //     A level of the texture resource to lock.
        //
        //   flags:
        //     Zero or more locking flags that describe the type of lock to perform. For
        //     this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        //   pitch:
        //     Pitch of the returning data.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that describes the locked region.
        GraphicsStream LockRectangle(int level, LockFlags flags, out int pitch);
        //
        // Summary:
        //     Locks a rectangle on a texture resource.
        //
        // Parameters:
        //   level:
        //     A level of the texture resource to lock.
        //
        //   rect:
        //     A System.Drawing.Rectangle to lock. To expand the dirty region to cover the
        //     entire texture, omit this parameter.
        //
        //   flags:
        //     Zero or more locking flags that describe the type of lock to perform. For
        //     this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that describes the locked region.
        GraphicsStream LockRectangle(int level, Rectangle rect, LockFlags flags);
        //
        // Summary:
        //     Locks a rectangle on a texture resource.
        //
        // Parameters:
        //   level:
        //     A level of the texture resource to lock.
        //
        //   rect:
        //     A System.Drawing.Rectangle to lock. To expand the dirty region to cover the
        //     entire texture, omit this parameter.
        //
        //   flags:
        //     Zero or more locking flags that describe the type of lock to perform. For
        //     this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        //   pitch:
        //     Pitch of the returning data.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that describes the locked region.
        GraphicsStream LockRectangle(int level, Rectangle rect, LockFlags flags, out int pitch);
        //
        // Summary:
        //     Locks a rectangle on a texture resource.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that indicates the type of data to return. This can
        //     be a value type or any type that contains only value types.
        //
        //   level:
        //     A level of the texture resource to lock.
        //
        //   flags:
        //     Zero or more locking flags that describe the type of lock to perform. For
        //     this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returning System.Array.
        //
        // Returns:
        //     An System.Array object that describes the locked region.
        Array LockRectangle(Type typeLock, int level, LockFlags flags, params int[] ranks);
        //
        // Summary:
        //     Locks a rectangle on a texture resource.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that indicates the type of data to return. This can
        //     be a value type or any type that contains only value types.
        //
        //   level:
        //     A level of the texture resource to lock.
        //
        //   flags:
        //     Zero or more locking flags that describe the type of lock to perform. For
        //     this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        //   pitch:
        //     Pitch of the returning data.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returning System.Array.
        //
        // Returns:
        //     An System.Array object that describes the locked region.
        Array LockRectangle(Type typeLock, int level, LockFlags flags, out int pitch, params int[] ranks);
        //
        // Summary:
        //     Locks a rectangle on a texture resource.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that indicates the type of data to return. This can
        //     be a value type or any type that contains only value types.
        //
        //   level:
        //     A level of the texture resource to lock.
        //
        //   rect:
        //     A System.Drawing.Rectangle to lock. To expand the dirty region to cover the
        //     entire texture, omit this parameter.
        //
        //   flags:
        //     Zero or more locking flags that describe the type of lock to perform. For
        //     this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returning System.Array.
        //
        // Returns:
        //     An System.Array object that describes the locked region.
        Array LockRectangle(Type typeLock, int level, Rectangle rect, LockFlags flags, params int[] ranks);
        //
        // Summary:
        //     Locks a rectangle on a texture resource.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that indicates the type of data to return. This can
        //     be a value type or any type that contains only value types.
        //
        //   level:
        //     A level of the texture resource to lock.
        //
        //   rect:
        //     A System.Drawing.Rectangle to lock. To expand the dirty region to cover the
        //     entire texture, omit this parameter.
        //
        //   flags:
        //     Zero or more locking flags that describe the type of lock to perform. For
        //     this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        //   pitch:
        //     Pitch of the returning data.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returning System.Array.
        //
        // Returns:
        //     An System.Array object that describes the locked region.
        Array LockRectangle(Type typeLock, int level, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks);
        //
        // Summary:
        //     Unlocks a rectangle on a texture resource.
        //
        // Parameters:
        //   level:
        //     Level of the texture resource to unlock.
        void UnlockRectangle(int level);
    }
}
