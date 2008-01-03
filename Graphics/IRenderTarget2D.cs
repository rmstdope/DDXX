using System;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IRenderTarget2D : IRenderTarget
    {
        // Summary:
        //     Gets a copy of the 2D texture associated with this render target.
        //
        // Returns:
        //     A copy of the 2D texture associated with this render target.
        ITexture2D GetTexture();
    }
}
