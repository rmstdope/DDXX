using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IRenderTargetCube : IRenderTarget
    {
        // Summary:
        //     Gets a copy of the cube texture associated with this render target.
        //
        // Returns:
        //     A copy of the cube texture associated with this render target.
        ITextureCube GetTexture();
    }
}
