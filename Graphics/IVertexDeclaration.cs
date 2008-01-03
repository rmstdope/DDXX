using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IVertexDeclaration : IDisposable
    {
        // Summary:
        //     Gets the GraphicsDevice associated with this vertex declaration.
        //
        // Returns:
        //     The GraphicsDevice associated with this vertex declaration.
        GraphicsDevice GraphicsDevice { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        //
        // Returns:
        //     true if the object is disposed; false otherwise.
        bool IsDisposed { get; }
        //
        // Summary:
        //     Returns the name of this vertex declaration.
        //
        // Returns:
        //     The name of this vertex declaration.
        string Name { get; set; }
        //
        // Summary:
        //     Returns the resource tags for this vertex declaration.
        //
        // Returns:
        //     The resource tags for this vertex declaration.
        object Tag { get; set; }
        //
        // Summary:
        //     Gets the vertex shader declaration.
        //
        // Returns:
        //     The array of vertex elements that make up the vertex shader declaration.
        VertexElement[] GetVertexElements();
        //
        // Summary:
        //     Gets the size of a vertex from the vertex declaration.
        //
        // Parameters:
        //   stream:
        //     The zero-based stream index.
        //
        // Returns:
        //     The vertex declaration size, in bytes.
        int GetVertexStrideSize(int stream);
    }
}
