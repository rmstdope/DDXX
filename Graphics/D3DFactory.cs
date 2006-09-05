using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class D3DFactory : IGraphicsFactory
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

        public IMesh MeshFromFile(IDevice device, string fileName, out EffectInstance[] effectInstance)
        {
            return MeshAdapter.FromFile(fileName, MeshFlags.Managed, ((DeviceAdapter)device).DXDevice, out effectInstance); 
        }

        public IMesh MeshFromFile(IDevice device, string fileName, out ExtendedMaterial[] materials)
        {
            return MeshAdapter.FromFile(fileName, MeshFlags.Managed, ((DeviceAdapter)device).DXDevice, out materials);
        }

        public IMesh CreateBoxMesh(IDevice device, float width, float height, float depth)
        {
            return MeshAdapter.Box(((DeviceAdapter)device).DXDevice, width, height, depth);
        }

        public IEffect EffectFromFile(IDevice device, string sourceDataFile, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        {
            return EffectAdapter.FromFile(((DeviceAdapter)device).DXDevice, sourceDataFile, includeFile, skipConstants, flags, pool);
        }

        public ITexture TextureFromFile(IDevice device, string srcFile, int width, int height, int mipLevels, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            return TextureAdapter.FromFile(((DeviceAdapter)device).DXDevice, srcFile, width, height, mipLevels, usage, format, pool, filter, mipFilter, colorKey);
        }

        public ISprite CreateSprite(IDevice device)
        {
            return new SpriteAdapter(((DeviceAdapter)device).DXDevice);
        }

        #endregion
    }
}
