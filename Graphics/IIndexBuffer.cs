using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IIndexBuffer : IGraphicsResource
    {
        // Summary:
        //     Gets the state of the related BufferUsage enumeration.
        //
        // Returns:
        //     Indicates how the application uses buffer contents.
        BufferUsage BufferUsage { get; }
        // Summary:
        //     Gets or sets a value indicating the size of this index element.
        //
        // Returns:
        //     The size of this index element.
        IndexElementSize IndexElementSize { get; }
        //
        // Summary:
        //     Gets the size, in bytes, of this IndexBuffer.
        //
        // Returns:
        //     The size, in bytes, of this IndexBuffer.
        int SizeInBytes { get; }
        //
        // Summary:
        //     Copies the index buffer into an array.
        //
        // Parameters:
        //   data:
        //     The array to receive index buffer data.
        void GetData<T>(T[] data) where T : struct;
        //
        // Summary:
        //     Copies the index buffer into an array.
        //
        // Parameters:
        //   data:
        //     The array to receive index buffer data.
        //
        //   startIndex:
        //     The index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy.
        void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Copies the index buffer into an array.
        //
        // Parameters:
        //   offsetInBytes:
        //     The number of bytes into the index buffer where copying will start.
        //
        //   data:
        //     The array to receive index buffer data.
        //
        //   startIndex:
        //     The index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy.
        void GetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Copies array data to the index buffer.
        //
        // Parameters:
        //   data:
        //     The array of data to copy.
        void SetData<T>(T[] data) where T : struct;
        //
        // Summary:
        //     Copies array data to the index buffer.
        //
        // Parameters:
        //   data:
        //     The array of data to copy.
        //
        //   startIndex:
        //     The index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy.
        void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Copies array data to the index buffer.
        //
        // Parameters:
        //   offsetInBytes:
        //     The number of bytes into the index buffer where copying will start.
        //
        //   data:
        //     The array of data to copy.
        //
        //   startIndex:
        //     The index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     The number of elements to copy.
        void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount) where T : struct;
    }
}
