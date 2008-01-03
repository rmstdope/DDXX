using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class DepthStencilBufferAdapter : IDepthStencilBuffer
    {
        private DepthStencilBuffer depthStencilBuffer;

        public DepthStencilBufferAdapter(DepthStencilBuffer depthStencilBuffer)
        {
            this.depthStencilBuffer = depthStencilBuffer;
        }

        public DepthStencilBuffer DxDepthStencilBuffer { get { return depthStencilBuffer; } }

        #region IDepthStencilBuffer Members

        public DepthFormat Format
        {
            get { return depthStencilBuffer.Format; }
        }

        public IGraphicsDevice GraphicsDevice
        {
            get { return new GraphicsDeviceAdapter(depthStencilBuffer.GraphicsDevice); }
        }

        public int Height
        {
            get { return depthStencilBuffer.Height; }
        }

        public bool IsDisposed
        {
            get { return depthStencilBuffer.IsDisposed; }
        }

        public int MultiSampleQuality
        {
            get { return depthStencilBuffer.MultiSampleQuality; }
        }

        public MultiSampleType MultiSampleType
        {
            get { return depthStencilBuffer.MultiSampleType; }
        }

        public string Name
        {
            get
            {
                return depthStencilBuffer.Name;
            }
            set
            {
                depthStencilBuffer.Name = value;
            }
        }

        public object Tag
        {
            get
            {
                return depthStencilBuffer.Tag;
            }
            set
            {
                depthStencilBuffer.Tag = value;
            }
        }

        public int Width
        {
            get { return depthStencilBuffer.Width; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            depthStencilBuffer.Dispose();
        }

        #endregion
    }
}
