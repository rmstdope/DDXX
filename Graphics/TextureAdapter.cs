using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class TextureAdapter : GraphicsResourceAdapter, ITexture
    {
        private Texture texture;

        public TextureAdapter(Texture texture)
            : base(texture)
        {
            this.texture = texture;
        }

        public Texture DxTexture { get { return texture; } }

        #region ITexture Members

        public int LevelCount
        {
            get { return texture.LevelCount; }
        }

        public int LevelOfDetail
        {
            get
            {
                return texture.LevelOfDetail;
            }
            set
            {
                texture.LevelOfDetail = value;
            }
        }

        public void GenerateMipMaps(TextureFilter filterType)
        {
            texture.GenerateMipMaps(filterType);
        }

#if (!XBOX)
        public void Save(string filename, ImageFileFormat format)
        {
            texture.Save(filename, format);
        }
#endif

        #endregion

    }
}
