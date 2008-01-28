using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Graphics
{
    public class TextureParameters
    {
        private string name;

        public TextureParameters(string fileName)
        {
            this.name = fileName;
        }

        public string Name
        {
            get { return name; }
        }

    }

    public class Texture2DParameters : TextureParameters
    {
        private ITexture2D texture;

        public Texture2DParameters(string fileName, ITexture2D texture)
            : base(fileName)
        {
            this.texture = texture;
        }

        public ITexture2D Texture
        {
            get { return texture; }
        }

    }

    public class TextureCubeParameters : TextureParameters
    {
        private ITextureCube texture;

        public TextureCubeParameters(string fileName, ITextureCube texture)
            : base(fileName)
        {
            this.texture = texture;
        }

        public ITextureCube Texture
        {
            get { return texture; }
        }

    }
}
