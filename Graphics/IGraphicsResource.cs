using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IGraphicsResource : IDisposable
    {
        // Summary:
        //     Gets the GraphicsDevice associated with this GraphicsResource.
        //
        // Returns:
        //     The GraphicsDevice associated with this GraphicsResource.
        IGraphicsDevice GraphicsDevice { get; }
        //
        // Summary:
        //     Gets the name of the resource.
        //
        // Returns:
        //     The name of the resource.
        string Name { get; set; }
        //
        // Summary:
        //     Gets or sets the resource-management priority for this resource.
        //
        // Returns:
        //     The new resource-management priority for this resource.
        int Priority { get; set; }
        //
        // Summary:
        //     Gets the type of this resource.
        //
        // Returns:
        //     The resource type.
        ResourceType ResourceType { get; }
        //
        // Summary:
        //     Gets the resource tags for this resource.
        //
        // Returns:
        //     The resource tags.
        object Tag { get; set; }
    }
}
