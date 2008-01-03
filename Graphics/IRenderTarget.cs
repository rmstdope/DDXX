using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IRenderTarget : IDisposable
    {
        // Summary:
        //     Gets the format of the render target.
        //
        // Returns:
        //     The format of the render target.
        SurfaceFormat Format { get; }
        //
        // Summary:
        //     Gets the graphics device associated with this render target resource.
        //
        // Returns:
        //     The graphics device associated with this render target resource.
        IGraphicsDevice GraphicsDevice { get; }
        //
        // Summary:
        //     Gets the height, in pixels, of this render target.
        //
        // Returns:
        //     The height, in pixels, of this render target.
        int Height { get; }
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
        //     Gets the name of this render-target resource.
        //
        // Returns:
        //     The name of this render-target resource.
        string Name { get; set; }
        //
        // Summary:
        //     Gets or sets render target usage flags.
        //
        // Returns:
        //     Usage flags for the render target.
        RenderTargetUsage RenderTargetUsage { get; }
        //
        // Summary:
        //     Gets the resource tags for this render target.
        //
        // Returns:
        //     The resource tags for this render target.
        object Tag { get; set; }
        //
        // Summary:
        //     Gets the width, in pixels, of this render target.
        //
        // Returns:
        //     The width, in pixels, of this render target.
        int Width { get; }
    }
}
