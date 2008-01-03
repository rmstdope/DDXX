using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class ResolveTexture2DAdapter : Texture2DAdapter, IResolveTexture2D
    {
        ResolveTexture2D resolveTexture2D;

        public ResolveTexture2D DxResolveTexture2D { get { return resolveTexture2D; } }

        public ResolveTexture2DAdapter(ResolveTexture2D resolveTexture2D)
            : base(resolveTexture2D)
        {
            this.resolveTexture2D = resolveTexture2D;
        }

        #region IResolveTexture2D Members

        public bool IsContentLost
        {
            get { return resolveTexture2D.IsContentLost; }
        }

        #endregion
    }
}
