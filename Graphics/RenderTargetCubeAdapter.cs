using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class RenderTargetCubeAdapter : RenderTargetAdapter, IRenderTargetCube
    {
        private RenderTargetCube renderTarget;

        public RenderTargetCubeAdapter(RenderTargetCube renderTarget)
            : base(renderTarget)
        {
            this.renderTarget = renderTarget;
        }

        public RenderTargetCube DxRenderTargetCube { get { return renderTarget; } }

        #region IRenderTargetCube Members

        public ITextureCube GetTexture()
        {
            return new TextureCubeAdapter(renderTarget.GetTexture());
        }

        #endregion
    }
}
