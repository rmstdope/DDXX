using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace Direct3D
{
    public class D3DFactory : IFactory
    {
        #region IFactory Members

        public IDevice CreateDevice(int adapter, DeviceType deviceType, Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters)
        {
            return new DeviceAdapter(adapter, deviceType, renderWindow, behaviorFlags, presentationParameters);
        }

        public IManager CreateManager()
        {
            return new DeviceManager();
        }

        public ITexture CreateTexture(IDevice device, Bitmap image, Usage usage, Pool pool)
        {
            return new TextureAdapter(((DeviceAdapter)device).DXDevice, image, usage, pool);
        }

        public ITexture CreateTexture(IDevice device, Stream data, Usage usage, Pool pool)
        {
            return new TextureAdapter(((DeviceAdapter)device).DXDevice, data, usage, pool);
        }

        public ITexture CreateTexture(IDevice device, int width, int height, int numLevels, Usage usage, Format format, Pool pool)
        {
            return new TextureAdapter(((DeviceAdapter)device).DXDevice, width, height, numLevels, usage, format, pool);
        }

        #endregion
    }
}
