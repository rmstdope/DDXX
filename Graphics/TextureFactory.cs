using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class TextureFactory
    {
        private class FileEntry
        {
            public string file;
            public ITexture texture;

            public FileEntry(string file, ITexture texture)
            {
                this.file = file;
                this.texture = texture;
            }
        }

        private IGraphicsFactory factory;
        private IDevice device;
        private List<FileEntry> files = new List<FileEntry>();
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

        public ITexture CreateFullsizeRenderTarget()
        {
            return factory.CreateTexture(device, presentParameters.BackBufferWidth, presentParameters.BackBufferHeight, 1, Usage.RenderTarget, presentParameters.BackBufferFormat, Pool.Default);
        }

        public ITexture CreateFullsizeRenderTarget(Format format)
        {
            return factory.CreateTexture(device, presentParameters.BackBufferWidth, presentParameters.BackBufferHeight, 1, Usage.RenderTarget, format, Pool.Default);
        }
    }
}
