using System;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class RenderTarget2DAdapter : RenderTargetAdapter, IRenderTarget2D
    {
        private RenderTarget2D renderTarget;

        public RenderTarget2DAdapter(RenderTarget2D renderTarget)
            : base(renderTarget)
        {
            this.renderTarget = renderTarget;
        }

        public RenderTarget2D DxRenderTarget2D { get { return renderTarget; } }

        #region IRenderTarget2D Members

        public ITexture2D GetTexture()
        {
            return new Texture2DAdapter(renderTarget.GetTexture());
        }

        #endregion
    }
}
