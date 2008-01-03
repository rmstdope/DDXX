using System;

namespace Dope.DDXX.Graphics
{
    public interface IVertexStream
    {
        // Summary:
        //     Gets the starting offset of the vertex stream.
        //
        // Returns:
        //     Starting offset of the vertex stream.
        int OffsetInBytes { get; }
        //
        // Summary:
        //     Gets the vertex buffer associated with this vertex stream.
        //
        // Returns:
        //     Vertex buffer associated with this vertex stream.
        IVertexBuffer VertexBuffer { get; }
        //
        // Summary:
        //     Gets the size, in bytes, of the elements in this vertex stream.
        //
        // Returns:
        //     Size, in bytes, of the elements in this vertex stream.
        int VertexStride { get; }
#if (!XBOX)
        // Summary:
        //     [Windows Only] Sets the stream source frequency divider value. This may be
        //     used to draw several instances of geometry.
        //
        // Parameters:
        //   frequency:
        //     Frequency divider value.
        void SetFrequency(int frequency);
        //
        // Summary:
        //     [Windows Only] Sets the stream source frequency divider value for the index
        //     data. This may be used to draw several instances of geometry.
        //
        // Parameters:
        //   frequency:
        //     Frequency of index data.
        void SetFrequencyOfIndexData(int frequency);
        //
        // Summary:
        //     [Windows Only] Sets the stream source frequency divider value for the instance
        //     data. This may be used to draw several instances of geometry.
        //
        // Parameters:
        //   frequency:
        //     Frequency of instance data.
        void SetFrequencyOfInstanceData(int frequency);
#endif
        //
        // Summary:
        //     Sets the source of the vertex stream.
        //
        // Parameters:
        //   vb:
        //     The vertex buffer source.
        //
        //   offsetInBytes:
        //     The starting offset.
        //
        //   vertexStride:
        //     The size, in bytes, of the elements in the vertex buffer.
        void SetSource(IVertexBuffer vb, int offsetInBytes, int vertexStride);
    }
}
