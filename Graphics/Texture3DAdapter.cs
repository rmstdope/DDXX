using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class Texture3DAdapter : TextureAdapter, ITexture3D
    {
        private Texture3D texture;

        public Texture3DAdapter(Texture3D texture)
            : base(texture)
        {
            this.texture = texture;
        }

        #region ITexture3D Members

        public int Depth
        {
            get { return texture.Depth; }
        }

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

        public void GetData<T>(int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount)
            where T : struct
        {
            texture.GetData<T>(level, left, top, right, bottom, front, back, data, startIndex, elementCount);
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

        public void SetData<T>(int level, int left, int top, int right, int bottom, int front, int back, T[] data, int startIndex, int elementCount, SetDataOptions options)
            where T : struct
        {
            texture.SetData<T>(level, left, top, right, bottom, front, back, data, startIndex, elementCount, options);
        }

        #endregion
    }
}
