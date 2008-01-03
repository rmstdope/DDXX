using System;

namespace Dope.DDXX.Graphics
{
    public interface IVertexStreamCollection
    {
        // Summary:
        //     Returns the VertexStream at the specified index.
        //
        // Parameters:
        //   index:
        //     Index of the VertexStream to return.
        //
        // Returns:
        //     The vertex stream at the requested index.
        IVertexStream this[int index] { get; }
    }
}
