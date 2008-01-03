using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface ITexture2D : ITexture
    {
        // Summary:
        //     Gets the pixel format of this texture resource.
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
        //     Indicates how the application uses the texture.
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
        //     Copies texture data into an array.
        //
        // Parameters:
        //   data:
        //     The array to receive texture data.
        void GetData<T>(T[] data) where T : struct;
        //
        // Summary:
        //     Copies texture data into an array.
        //
        // Parameters:
        //   data:
        //     The array to receive texture data.
        //
        //   startIndex:
        //     The index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy.
        void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Copies texture data into an array.
        //
        // Parameters:
        //   level:
        //     The mipmap level to copy from.
        //
        //   rect:
        //     The section of the texture to copy. null indicates the data will be copied
        //     from the entire texture.
        //
        //   data:
        //     The array to receive texture data.
        //
        //   startIndex:
        //     The index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy.
        void GetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Copies array data to the texture at mipmap level 0.
        //
        // Parameters:
        //   data:
        //     The array of data to copy. The number of elements in the array must be equal
        //     to the size of the texture, which is Texture2D.Width×Texture2D.Height.
        void SetData<T>(T[] data) where T : struct;
        //
        // Summary:
        //     Copies array data to the texture at mipmap level 0.
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
        //     which is Texture2D.Width×Texture2D.Height.
        //
        //   options:
        //     Option that specifies whether existing data in the buffer will be kept after
        //     this operation.
        void SetData<T>(T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct;
        //
        // Summary:
        //     Copies array data to the texture.
        //
        // Parameters:
        //   level:
        //     The mipmap level where the data will be placed.
        //
        //   rect:
        //     The section of the texture where the data will be placed. null indicates
        //     the data will be copied over the entire texture.
        //
        //   data:
        //     The array of data to copy. If rect is null, the number of elements in the
        //     array must be equal to the size of the texture, which is Texture2D.Width×Texture2D.Height;
        //     otherwise, the number of elements in the array should equal the size of the
        //     rectangle specified.
        //
        //   startIndex:
        //     The index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy.
        //
        //   options:
        //     Option that specifies whether existing data in the buffer will be kept after
        //     this operation.
        void SetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount, SetDataOptions options) where T : struct;
    }
}
