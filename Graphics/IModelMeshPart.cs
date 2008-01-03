using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IModelMeshPart
    {
        // Summary:
        //     Gets the offset to add to each vertex index in the index buffer.
        //
        // Returns:
        //     Offset to add to each vertex index in the index buffer.
        int BaseVertex { get; }
        //
        // Summary:
        //     Gets or sets the material Effect for this mesh part.
        //
        // Returns:
        //     The material effect for this mesh part.
        IEffect Effect { get; set; }
        //
        // Summary:
        //     Gets the number of vertices used during a draw call.
        //
        // Returns:
        //     The number of vertices used during the call.
        int NumVertices { get; }
        //
        // Summary:
        //     Gets the number of primitives to render.
        //
        // Returns:
        //     The number of primitives to render. The number of vertices used is a function
        //     of primitiveCount and primitiveType.
        int PrimitiveCount { get; }
        //
        // Summary:
        //     Gets the location in the index array at which to start reading vertices.
        //
        // Returns:
        //     Location in the index array at which to start reading vertices.
        int StartIndex { get; }
        //
        // Summary:
        //     Gets the offset in bytes from the beginning of the VertexBuffer.
        //
        // Returns:
        //     The offset in bytes from the beginning of the VertexBuffer.
        int StreamOffset { get; }
        //
        // Summary:
        //     Gets or sets an object identifying this model mesh part.
        //
        // Returns:
        //     An object identifying this model mesh part.
        object Tag { get; set; }
        //
        // Summary:
        //     Gets the vertex declaration for this model mesh part.
        //
        // Returns:
        //     The vertex declaration for this model mesh part.
        IVertexDeclaration VertexDeclaration { get; }
        //
        // Summary:
        //     Gets the size, in bytes, of the elements in this vertex stream.
        //
        // Returns:
        //     The size, in bytes, of the elements in this vertex stream.
        int VertexStride { get; }

        IMaterialHandler MaterialHandler { get; }
    }
}
