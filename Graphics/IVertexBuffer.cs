using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IVertexBuffer : IGraphicsResource
    {
        // Summary:
        //     Gets the state of the related BufferUsage enumeration.
        //
        // Returns:
        //     Indicates how the application uses buffer contents.
        BufferUsage BufferUsage { get; }
        //
        // Summary:
        //     Gets the size, in bytes, of this vertex buffer.
        //
        // Returns:
        //     The size, in bytes, of the vertex buffer.
        int SizeInBytes { get; }
        //
        // Summary:
        //     Gets a copy of the vertex buffer data.
        //
        // Parameters:
        //   data:
        //     The array into which to copy the vertex buffer data.
        void GetData<T>(T[] data) where T : struct;
        //
        // Summary:
        //     Gets a copy of the vertex buffer data, specifying the start index and number
        //     of elements in the vertex buffer.
        //
        // Parameters:
        //   data:
        //     The array into which to copy the vertex buffer data.
        //
        //   startIndex:
        //     Index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     Number of elements in the array.
        void GetData<T>(T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Gets a copy of the vertex buffer data, specifying the start index, starting
        //     offset, number of elements, and size of the vertex buffer elements.
        //
        // Parameters:
        //   offsetInBytes:
        //     Starting offset.
        //
        //   data:
        //     The array into which to copy the vertex buffer data.
        //
        //   startIndex:
        //     Index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     Number of elements in the array.
        //
        //   vertexStride:
        //     Size, in bytes, of an element in the vertex buffer.
        void GetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct;
        //
        // Summary:
        //     Sets the vertex buffer data.
        //
        // Parameters:
        //   data:
        //     The array from which to copy the vertex buffer data.
        void SetData<T>(T[] data) where T : struct;
        //
        // Summary:
        //     Sets the vertex buffer data, specifying the start index and number of elements
        //     in the vertex buffer.
        //
        // Parameters:
        //   data:
        //     The array from which to copy the vertex buffer data.
        //
        //   startIndex:
        //     Index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     Number of elements in the array.
        void SetData<T>(T[] data, int startIndex, int elementCount) where T : struct;
        //
        // Summary:
        //     Sets the vertex buffer data, specifying the start index, starting offset,
        //     number of elements, and size of the vertex buffer elements.
        //
        // Parameters:
        //   offsetInBytes:
        //     Starting offset.
        //
        //   data:
        //     The array from which to copy the vertex buffer data.
        //
        //   startIndex:
        //     Index of the element in the array at which to start copying.
        //
        //   elementCount:
        //     Number of elements in the array.
        //
        //   vertexStride:
        //     Size, in bytes, of an element in the vertex buffer.
        void SetData<T>(int offsetInBytes, T[] data, int startIndex, int elementCount, int vertexStride) where T : struct;
    }
}
