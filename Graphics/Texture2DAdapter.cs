using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class Texture2DAdapter : TextureAdapter, ITexture2D
    {
        private Texture2D texture;

        public Texture2DAdapter(Texture2D texture)
            : base(texture)
        {
            this.texture = texture;
        }

        public Texture2D DxTexture2D { get { return texture; } }

        #region ITexture2D Members

        public SurfaceFormat Format
        {
            get { return texture.Format; }
        }

        public int Height
        {
            get { return texture.Height; }
        }

        public TextureUsage TextureUsage
        {
            get { return texture.TextureUsage; }
        }

        public int Width
        {
            get { return texture.Width; }
        }

        public void GetData<T>(T[] data)
            where T : struct
        {
            texture.GetData<T>(data);
        }

        public void GetData<T>(T[] data, int startIndex, int elementCount)
            where T : struct
        {
            texture.GetData<T>(data, startIndex, elementCount);
        }

        public void GetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount)
            where T : struct
        {
            texture.GetData<T>(level, rect, data, startIndex, elementCount);
        }

        public void SetData<T>(T[] data)
            where T : struct
        {
            texture.SetData<T>(data);
        }

        public void SetData<T>(T[] data, int startIndex, int elementCount, SetDataOptions options)
            where T : struct
        {
            texture.SetData<T>(data, startIndex, elementCount, options);
        }

        public void SetData<T>(int level, Rectangle? rect, T[] data, int startIndex, int elementCount, SetDataOptions options)
            where T : struct
        {
            texture.SetData<T>(level, rect, data, startIndex, elementCount, options);
        }

        #endregion
    }
}
