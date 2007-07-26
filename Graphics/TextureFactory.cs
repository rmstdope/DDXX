using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class TextureFactory : ITextureFactory
    {
        private class BaseEntry
        {
            public string file;
            public BaseEntry(string file)
            {
                this.file = file;
            }
        }

        private class FileEntry : BaseEntry
        {
            public ITexture texture;
            public FileEntry(string file, ITexture texture)
                : base(file)
            {
                this.texture = texture;
            }
        }

        private class CubeFileEntry : BaseEntry
        {
            public ICubeTexture texture;
            public CubeFileEntry(string file, ICubeTexture texture)
                : base(file)
            {
                this.texture = texture;
            }
        }

        private IGraphicsFactory factory;
        private IDevice device;
        private List<FileEntry> files = new List<FileEntry>();
        private List<CubeFileEntry> cubeFiles = new List<CubeFileEntry>();
        private PresentParameters presentParameters;

        public TextureFactory(IDevice device, IGraphicsFactory factory, PresentParameters presentParameters)
        {
            this.device = device;
            this.factory = factory;
            this.presentParameters = presentParameters;
        }

        public ITexture CreateFromFile(string file)
        {
            FileEntry needle = new FileEntry(file, null);
            FileEntry result = files.Find(delegate(FileEntry item)
            {
                if (needle.file == item.file)
                    return true;
                else
                    return false;
            });
            if (result != null)
            {
                return result.texture;
            }
            ITexture texture = factory.TextureFromFile(device, file, 0, 0, 0, 0, Format.Unknown, Pool.Managed, Filter.Linear, Filter.Linear, 0);
            needle.texture = texture;
            files.Add(needle);
            return texture;

        }

        public ICubeTexture CreateCubeFromFile(string file)
        {
            CubeFileEntry needle = new CubeFileEntry(file, null);
            CubeFileEntry result = cubeFiles.Find(delegate(CubeFileEntry item)
            {
                if (needle.file == item.file)
                    return true;
                else
                    return false;
            });
            if (result != null)
            {
                return result.texture;
            }
            ICubeTexture texture = factory.CubeTextureFromFile(device, file, 0, 0, Usage.None, Format.Unknown, Pool.Managed, Filter.Linear, Filter.Linear, 0);
            needle.texture = texture;
            cubeFiles.Add(needle);
            return texture;
        }

        public ITexture CreateRenderTarget(int width, int height) {
            return factory.CreateTexture(device, width, height, 1, Usage.RenderTarget, presentParameters.BackBufferFormat, Pool.Default);
        }

        public ITexture CreateRenderTarget(int width, int height, Format format) {
            return factory.CreateTexture(device, width, height, 1, Usage.RenderTarget, presentParameters.BackBufferFormat, Pool.Default);
        }

        public ITexture CreateFullsizeRenderTarget() {
            return this.CreateRenderTarget(presentParameters.BackBufferWidth, 
                presentParameters.BackBufferHeight);
        }

        public ITexture CreateFullsizeRenderTarget(Format format)
        {
            return this.CreateRenderTarget(presentParameters.BackBufferWidth, 
                presentParameters.BackBufferHeight, format);
        }

        public ITexture CreateFromFunction(int width, int height, int numLevels, Usage usage, Format format, Pool pool, Fill2DTextureCallback callbackFunction)
        {
            ITexture texture = factory.CreateTexture(device, width, height, numLevels, usage, format, pool);
            texture.FillTexture(callbackFunction);
            return texture;
        }

        public void RegisterTexture(string name, ITexture texture)
        {
            files.Add(new FileEntry(name, texture));
        }

    }
}
