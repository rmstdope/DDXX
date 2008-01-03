using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IResolveTexture2D : ITexture2D, IGraphicsResource
    {
        // Summary:
        //     Determines if the render target data has been lost due to a lost device event.
        //
        // Returns:
        //     true if the content was lost; false otherwise.
        bool IsContentLost { get; }
    }
}
