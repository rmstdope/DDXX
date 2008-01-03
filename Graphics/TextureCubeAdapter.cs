using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class TextureCubeAdapter : TextureAdapter, ITextureCube
    {
        private TextureCube texture;

        public TextureCubeAdapter(TextureCube texture)
            : base(texture)
        {
            this.texture = texture;
        }

        #region ITextureCube Members

        public SurfaceFormat Format
        {
            get { return texture.Format; }
        }

        public TextureUsage TextureUsage
        {
            get { return texture.TextureUsage; }
        }

        public int Size
        {
            get { return texture.Size; }
        }

        public void GetData<T>(CubeMapFace faceType, T[] data)
            where T : struct
        {
            texture.GetData<T>(faceType, data);
        }

        public void GetData<T>(CubeMapFace faceType, T[] data, int startIndex, int elementCount)
            where T : struct
        {
            texture.GetData<T>(faceType, data, startIndex, elementCount);
        }

        public void GetData<T>(CubeMapFace faceType, int level, Rectangle? rect, T[] data, int startIndex, int elementCount)
            where T : struct
        {
            texture.GetData<T>(faceType, level, rect, data, startIndex, elementCount);
        }

        public void SetData<T>(CubeMapFace faceType, T[] data)
            where T : struct
        {
            texture.SetData<T>(faceType, data);
        }

        public void SetData<T>(CubeMapFace faceType, T[] data, int startIndex, int elementCount, SetDataOptions options)
            where T : struct
        {
            texture.SetData<T>(faceType, data, startIndex, elementCount, options);
        }

        public void SetData<T>(CubeMapFace faceType, int level, Rectangle? rect, T[] data, int startIndex, int elementCount, SetDataOptions options)
            where T : struct
        {
            texture.SetData<T>(faceType, level, rect, data, startIndex, elementCount, options);
        }

        #endregion
    }
}
