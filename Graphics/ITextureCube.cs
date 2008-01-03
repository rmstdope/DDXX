using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface ITextureCube : ITexture
    {
        // Summary:
        //     Gets the pixel format for this texture resource.
        //
        // Returns:
        //     The pixel format of this texture resource.
        SurfaceFormat Format { get; }
        //
        // Summary:
        //     Gets the width and height of this texture resource, in pixels.
        //
        // Returns:
        //     The size of this texture resource, in pixels.
        int Size { get; }
        //
        // Summary:
        //     Gets the state of the related TextureUsage enumeration.
        //
        // Returns:
        //     Indicates how the application uses the related texture.
        TextureUsage TextureUsage { get; }
        //
        // Summary:
        //     Returns a copy of the texture data.
        //
        // Parameters:
        //   faceType:
        //     The cube map face type.
        //
        //   data:
        //     The array into which to copy the data.
        void GetData<T>(CubeMapFace faceType, T[] data) where T : struct;
        //
        // Summary:
        //     Returns a copy of the texture data, specifying the start index and number
        //     of elements in the vertex buffer.
        //
        // Parameters:
        //   faceType:
        //     The cube map face type.
        //
        //   data:
        //     The array into which to copy the data.
        //
        //   startIndex:
        //     Index in the array at which to begin the copy.
        //
        //   elementCount:
        //     Number of elements in the array.
        void GetData<T>(CubeMapFace faceType, T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Returns a copy of the texture data, specifying the start index, staring offset,
        //     number of elements, region to copy, and level where the data is to be placed.
        //
        // Parameters:
        //   faceType:
        //     The cube map face type.
        //
        //   level:
        //     The mipmap level where the data will be placed.
        //
        //   rect:
        //     The section of the texture where the data will be placed. null indicates
        //     the data will be copied over the entire texture.
        //
        //   data:
        //     The array into which to copy the data.
        //
        //   startIndex:
        //     Index in the array at which to begin the copy.
        //
        //   elementCount:
        //     Number of elements in the array.
        void GetData<T>(CubeMapFace faceType, int level, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Copies array data to the texture at mipmap level 0.
        //
        // Parameters:
        //   faceType:
        //     The cube map face type.
        //
        //   data:
        //     The array of data to copy. The number of elements in the array must be equal
        //     to the size of the texture.
        void SetData<T>(CubeMapFace faceType, T[] data) where T : struct;
        //
        // Summary:
        //     Copies array data to the texture at mipmap level 0, specifying a start offset.
        //
        // Parameters:
        //   faceType:
        //     The cube map face type.
        //
        //   data:
        //     The array of data to copy.
        //
        //   startIndex:
        //     Start offset in the array.
        //
        //   elementCount:
        //     Number of elements in the array. This must be equal to the size of the texture.
        //
        //   options:
        //     Option specifying if existing data in the buffer will be kept after this
        //     operation.
        void SetData<T>(CubeMapFace faceType, T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct;
        //
        // Summary:
        //     Copies array data to the texture, specifying a start offset, mipmap level,
        //     and subregion to copy.
        //
        // Parameters:
        //   faceType:
        //     The cube map face type.
        //
        //   level:
        //     The mipmap level where the data will be placed.
        //
        //   rect:
        //     The section of the texture where the data will be placed. null indicates
        //     the data will be copied over the entire texture.
        //
        //   data:
        //     The array of data to copy.
        //
        //   startIndex:
        //     Start offset in the array.
        //
        //   elementCount:
        //     Number of elements in the array. The number of elements to copy must be equal
        //     to the size of the texture.
        //
        //   options:
        //     Option specifying if existing data in the buffer will be kept after this
        //     operation.
        void SetData<T>(CubeMapFace faceType, int level, Rectangle? rect, T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct;
    }
}
