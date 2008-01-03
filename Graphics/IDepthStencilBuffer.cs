using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IDepthStencilBuffer : IDisposable
    {
        // Summary:
        //     Gets the data format of this depth stencil buffer.
        //
        // Returns:
        //     The data format of this depth stencil buffer.
        DepthFormat Format { get; }
        //
        // Summary:
        //     Gets the graphics device associated with this depth stencil buffer.
        //
        // Returns:
        //     The graphics device associated with this depth stencil buffer.
        IGraphicsDevice GraphicsDevice { get; }
        //
        // Summary:
        //     Gets the height, in pixels, of the surface.
        //
        // Returns:
        //     The height, in pixels, of the surface.
        int Height { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        //
        // Returns:
        //     true if the object is disposed. false if the object is not disposed.
        bool IsDisposed { get; }
        //
        // Summary:
        //     Gets the number of quality stops available for a given multisample type.
        //
        // Returns:
        //     The number of quality stops available for a given multisample type.
        int MultiSampleQuality { get; }
        //
        // Summary:
        //     Gets the levels of full-scene multisampling that the device can apply.
        //
        // Returns:
        //     The levels of full-scene multisampling that the device can apply.
        MultiSampleType MultiSampleType { get; }
        //
        // Summary:
        //     Gets the name of this depth stencil buffer.
        //
        // Returns:
        //     The name of this depth stencil buffer.
        string Name { get; set; }
        //
        // Summary:
        //     Gets the resource tag for this depth stencil buffer.
        //
        // Returns:
        //     The resource tag for this depth stencil buffer.
        object Tag { get; set; }
        //
        // Summary:
        //     Returns the width, in pixels, of this DepthStencilBuffer.
        //
        // Returns:
        //     The width, in pixels, of this DepthStencilBuffer.
        int Width { get; }
    }
}
