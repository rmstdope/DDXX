using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class RenderTargetAdapter : IRenderTarget
    {
        private RenderTarget renderTarget;

        public RenderTargetAdapter(RenderTarget renderTarget)
        {
            this.renderTarget = renderTarget;
        }

        #region IRenderTarget Members

        public SurfaceFormat Format
        {
            get { return renderTarget.Format; }
        }

        public IGraphicsDevice GraphicsDevice
        {
            get { return new GraphicsDeviceAdapter(renderTarget.GraphicsDevice); }
        }

        public int Height
        {
            get { return renderTarget.Height; }
        }

        public int MultiSampleQuality
        {
            get { return renderTarget.MultiSampleQuality; }
        }

        public MultiSampleType MultiSampleType
        {
            get { return renderTarget.MultiSampleType; }
        }

        public string Name
        {
            get
            {
                return renderTarget.Name;
            }
            set
            {
                renderTarget.Name = value;
            }
        }

        public RenderTargetUsage RenderTargetUsage
        {
            get { return renderTarget.RenderTargetUsage; }
        }

        public object Tag
        {
            get
            {
                return renderTarget.Tag;
            }
            set
            {
                renderTarget.Tag = value;
            }
        }

        public int Width
        {
            get { return renderTarget.Width; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            renderTarget.Dispose();
        }

        #endregion
    }
}
