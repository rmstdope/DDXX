using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.Graphics
{
    public interface ICubeTexture : IBaseTexture
    {
        // Summary:
        //     Adds a dirty region to a cube texture resource.
        //
        // Parameters:
        //   faceType:
        //     A Microsoft.DirectX.Direct3D.CubeMapFace value that identifies the face where
        //     the dirty region will be added. Omitting the Microsoft.DirectX.Direct3D.CubeTexture.AddDirtyRectangle()
        //     parameter indicates that the dirty region should expand to cover the full
        //     face.
        void AddDirtyRectangle(CubeMapFace faceType);
        //
        // Summary:
        //     Adds a dirty region to a cube texture resource.
        //
        // Parameters:
        //   faceType:
        //     A Microsoft.DirectX.Direct3D.CubeMapFace value that identifies the face where
        //     the dirty region will be added. Omitting the Microsoft.DirectX.Direct3D.CubeTexture.AddDirtyRectangle()
        //     parameter indicates that the dirty region should expand to cover the full
        //     face.
        //
        //   rect:
        //     A System.Drawing.Rectangle that specifies the dirty region.
        void AddDirtyRectangle(CubeMapFace faceType, Rectangle rect);
        //
        // Summary:
        //     Retrieves a cube texture map surface.
        //
        // Parameters:
        //   faceType:
        //     Member of the Microsoft.DirectX.Direct3D.CubeMapFace enumerated type that
        //     identifies a cube map face.
        //
        //   level:
        //     Level of a mipmapped cube texture.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Surface object that represents the returned
        //     cube texture map surface.
        Surface GetCubeMapSurface(CubeMapFace faceType, int level);
        //
        // Summary:
        //     Locks a rectangle on a cube texture resource.
        //
        // Parameters:
        //   faceType:
        //     Member of the Microsoft.DirectX.Direct3D.CubeMapFace enumerated type that
        //     identifies a cube map face.
        //
        //   level:
        //     Level of a cube texture.
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags enumerated values that
        //     describe the type of lock to perform.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that represents the locked rectangle.
        GraphicsStream LockRectangle(CubeMapFace faceType, int level, LockFlags flags);
        //
        // Summary:
        //     Locks a rectangle on a cube texture resource.
        //
        // Parameters:
        //   faceType:
        //     Member of the Microsoft.DirectX.Direct3D.CubeMapFace enumerated type that
        //     identifies a cube map face.
        //
        //   level:
        //     Level of a cube texture.
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags enumerated values that
        //     describe the type of lock to perform.
        //
        //   pitch:
        //     Pitch of the returning data.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that represents the locked rectangle.
        GraphicsStream LockRectangle(CubeMapFace faceType, int level, LockFlags flags, out int pitch);
        //
        // Summary:
        //     Locks a rectangle on a cube texture resource.
        //
        // Parameters:
        //   faceType:
        //     Member of the Microsoft.DirectX.Direct3D.CubeMapFace enumerated type that
        //     identifies a cube map face.
        //
        //   level:
        //     Level of a cube texture.
        //
        //   rect:
        //     A System.Drawing.Rectangle to lock. To expand the dirty region to cover the
        //     entire cube texture, omit this parameter.
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags enumerated values that
        //     describe the type of lock to perform.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that represents the locked rectangle.
        GraphicsStream LockRectangle(CubeMapFace faceType, int level, Rectangle rect, LockFlags flags);
        //
        // Summary:
        //     Locks a rectangle on a cube texture resource.
        //
        // Parameters:
        //   faceType:
        //     Member of the Microsoft.DirectX.Direct3D.CubeMapFace enumerated type that
        //     identifies a cube map face.
        //
        //   level:
        //     Level of a cube texture.
        //
        //   rect:
        //     A System.Drawing.Rectangle to lock. To expand the dirty region to cover the
        //     entire cube texture, omit this parameter.
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags enumerated values that
        //     describe the type of lock to perform.
        //
        //   pitch:
        //     Pitch of the returning data.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that represents the locked rectangle.
        GraphicsStream LockRectangle(CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, out int pitch);
        //
        // Summary:
        //     Locks a rectangle on a cube texture resource.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that indicates the type of array data to return. This
        //     can be a value type or any type that contains only value types.
        //
        //   faceType:
        //     Member of the Microsoft.DirectX.Direct3D.CubeMapFace enumerated type that
        //     identifies a cube map face.
        //
        //   level:
        //     Level of a cube texture.
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags enumerated values that
        //     describe the type of lock to perform.
        //
        //   ranks:
        //     Array of 1 to 3 System.Int32 values that indicate the dimensions of the returning
        //     System.Array.
        //
        // Returns:
        //     An System.Array that represents the locked rectangle.
        Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, LockFlags flags, params int[] ranks);
        //
        // Summary:
        //     Locks a rectangle on a cube texture resource.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that indicates the type of array data to return. This
        //     can be a value type or any type that contains only value types.
        //
        //   faceType:
        //     Member of the Microsoft.DirectX.Direct3D.CubeMapFace enumerated type that
        //     identifies a cube map face.
        //
        //   level:
        //     Level of a cube texture.
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags enumerated values that
        //     describe the type of lock to perform.
        //
        //   pitch:
        //     Pitch of the returning data.
        //
        //   ranks:
        //     Array of 1 to 3 System.Int32 values that indicate the dimensions of the returning
        //     System.Array.
        //
        // Returns:
        //     An System.Array that represents the locked rectangle.
        Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, LockFlags flags, out int pitch, params int[] ranks);
        //
        // Summary:
        //     Locks a rectangle on a cube texture resource.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that indicates the type of array data to return. This
        //     can be a value type or any type that contains only value types.
        //
        //   faceType:
        //     Member of the Microsoft.DirectX.Direct3D.CubeMapFace enumerated type that
        //     identifies a cube map face.
        //
        //   level:
        //     Level of a cube texture.
        //
        //   rect:
        //     A System.Drawing.Rectangle to lock. To expand the dirty region to cover the
        //     entire cube texture, omit this parameter.
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags enumerated values that
        //     describe the type of lock to perform.
        //
        //   ranks:
        //     Array of 1 to 3 System.Int32 values that indicate the dimensions of the returning
        //     System.Array.
        //
        // Returns:
        //     An System.Array that represents the locked rectangle.
        Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, params int[] ranks);
        //
        // Summary:
        //     Locks a rectangle on a cube texture resource.
        //
        // Parameters:
        //   typeLock:
        //     A System.Type object that indicates the type of array data to return. This
        //     can be a value type or any type that contains only value types.
        //
        //   faceType:
        //     Member of the Microsoft.DirectX.Direct3D.CubeMapFace enumerated type that
        //     identifies a cube map face.
        //
        //   level:
        //     Level of a cube texture.
        //
        //   rect:
        //     A System.Drawing.Rectangle to lock. To expand the dirty region to cover the
        //     entire cube texture, omit this parameter.
        //
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags enumerated values that
        //     describe the type of lock to perform.
        //
        //   pitch:
        //     Pitch of the returning data.
        //
        //   ranks:
        //     Array of 1 to 3 System.Int32 values that indicate the dimensions of the returning
        //     System.Array.
        //
        // Returns:
        //     An System.Array that represents the locked rectangle.
        Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks);
        //
        // Summary:
        //     Unlocks a rectangle on a cube texture resource.
        //
        // Parameters:
        //   faceType:
        //     Member of the Microsoft.DirectX.Direct3D.CubeMapFace enumerated type that
        //     identifies a cube map face.
        //
        //   level:
        //     Level of a mipmapped cube texture.
        void UnlockRectangle(CubeMapFace faceType, int level);
    }
}
