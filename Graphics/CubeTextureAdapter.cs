using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.Graphics
{
    public class CubeTextureAdapter : BaseTextureAdapter, ICubeTexture
    {
        private CubeTexture texture;

        public CubeTextureAdapter(CubeTexture texture) :
            base(texture)
        {
            this.texture = texture;
        }

        public CubeTexture CubeTextureDX
        {
            get { return texture; }
        }

        #region ICubeTexture Members

        public void AddDirtyRectangle(CubeMapFace faceType)
        {
            texture.AddDirtyRectangle(faceType);
        }

        public void AddDirtyRectangle(CubeMapFace faceType, Rectangle rect)
        {
            texture.AddDirtyRectangle(faceType, rect);
        }

        public Surface GetCubeMapSurface(CubeMapFace faceType, int level)
        {
            return texture.GetCubeMapSurface(faceType, level);
        }

        public GraphicsStream LockRectangle(CubeMapFace faceType, int level, LockFlags flags)
        {
            return texture.LockRectangle(faceType, level, flags);
        }

        public GraphicsStream LockRectangle(CubeMapFace faceType, int level, LockFlags flags, out int pitch)
        {
            return texture.LockRectangle(faceType, level, flags, out pitch);
        }

        public GraphicsStream LockRectangle(CubeMapFace faceType, int level, Rectangle rect, LockFlags flags)
        {
            return texture.LockRectangle(faceType, level, rect, flags);
        }

        public GraphicsStream LockRectangle(CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, out int pitch)
        {
            return texture.LockRectangle(faceType, level, rect, flags, out pitch);
        }

        public Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, LockFlags flags, params int[] ranks)
        {
            return texture.LockRectangle(typeLock, faceType, level, flags, ranks);
        }

        public Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, LockFlags flags, out int pitch, params int[] ranks)
        {
            return texture.LockRectangle(typeLock, faceType, level, flags, out pitch, ranks);
        }

        public Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, params int[] ranks)
        {
            return texture.LockRectangle(typeLock, faceType, level, rect, flags, ranks);
        }

        public Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks)
        {
            return texture.LockRectangle(typeLock, faceType, level, rect, flags, out pitch, ranks);
        }

        public void UnlockRectangle(CubeMapFace faceType, int level)
        {
            texture.UnlockRectangle(faceType, level);
        }

        #endregion

    }
}
