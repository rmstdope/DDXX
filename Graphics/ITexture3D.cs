using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface ITexture3D : ITexture
    {
        // Summary:
        //     Gets the depth of this volume texture resource, in pixels.
        //
        // Returns:
        //     The depth of this volume texture resource, in pixels.
        int Depth { get; }
        //
        // Summary:
        //     Gets the pixel format for this texture resource.
        //
        // Returns:
        //     The pixel format of this texture resource.
        SurfaceFormat Format { get; }
        //
        // Summary:
        //     Gets the height of this texture resource, in pixels.
        //
        // Returns:
        //     The height of this texture resource, in pixels.
        int Height { get; }
        //
        // Summary:
        //     Gets the state of the related TextureUsage enumeration.
        //
        // Returns:
        //     Indicates how the application uses the related texture.
        TextureUsage TextureUsage { get; }
        //
        // Summary:
        //     Gets the width of this texture resource, in pixels.
        //
        // Returns:
        //     The width of this texture resource, in pixels.
        int Width { get; }
        //
        // Summary:
        //     Gets a copy of the texture data.
        //
        // Parameters:
        //   data:
        //     An array to fill with data.
        void GetData<T>(T[] data) where T : struct;
        //
        // Summary:
        //     Gets a copy of the texture data, specifying the starting index and number
        //     of elements to copy.
        //
        // Parameters:
        //   data:
        //     An array to fill with data.
        //
        //   startIndex:
        //     Index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy.
        void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Gets a copy of the texture data, specifying the level and dimensions of the
        //     volume texture to copy.
        //
        // Parameters:
        //   level:
        //     The mipmap level where the data will be placed.
        //
        //   left:
        //     Position of the left side of the box on the x-axis.
        //
        //   top:
        //     Position of the top of the box on the y-axis.
        //
        //   right:
        //     Position of the right side of the box on the x-axis.
        //
        //   bottom:
        //     Position of the bottom of the box on the y-axis.
        //
        //   front:
        //     Position of the front of the box on the z-axis.
        //
        //   back:
        //     Position of the back of the box on the z-axis.
        //
        //   data:
        //     An array to fill with data.
        //
        //   startIndex:
        //     The index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy.
        void GetData<T>(int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Copies array data to the texture at mipmap level 0.
        //
        // Parameters:
        //   data:
        //     The array of data to copy. The number of elements in the array must be equal
        //     to the size of the texture, which is Texture3D.Width×Texture3D.Height×Texture3D.Depth.
        void SetData<T>(T[] data) where T : struct;
        //
        // Summary:
        //     Copies array data to the texture at mipmap level 0, specifying the starting
        //     index and number of elements to copy.
        //
        // Parameters:
        //   data:
        //     The array of data to copy.
        //
        //   startIndex:
        //     The index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy. This must be equal to the size of the texture,
        //     which is Texture3D.Width×Texture3D.Height×Texture3D.Depth.
        //
        //   options:
        //     Option specifying if existing data in the buffer will be kept after this
        //     operation.
        void SetData<T>(T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct;
        //
        // Summary:
        //     Copies array data to the texture, specifying the dimensions of the volume
        //     and the mipmap level where the data is to be placed.
        //
        // Parameters:
        //   level:
        //     The mipmap level where the data will be placed.
        //
        //   left:
        //     Position of the left side of the box on the x-axis.
        //
        //   top:
        //     Position of the top of the box on the y-axis.
        //
        //   right:
        //     Position of the right side of the box on the x-axis.
        //
        //   bottom:
        //     Position of the bottom of the box on the y-axis.
        //
        //   front:
        //     Position of the front of the box on the z-axis.
        //
        //   back:
        //     Position of the back of the box on the z-axis.
        //
        //   data:
        //     The array of data to copy. The number of elements in the array should be
        //     equal to the size of the box where the data will be placed.
        //
        //   startIndex:
        //     The index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy.
        //
        //   options:
        //     Option specifying whether existing data in the buffer will be kept after
        //     this operation.
        void SetData<T>(int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct;
    }
}
