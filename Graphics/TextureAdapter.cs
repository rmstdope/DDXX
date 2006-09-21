using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class TextureAdapter : BaseTextureAdapter, ITexture
    {
        private Texture texture;

        public TextureAdapter(Texture texture) : 
            base(texture)
        {
            this.texture = texture;
        }

        public Texture TextureDX
        {
            get { return texture; }
        }

        public static ITexture FromFile(Device device, string srcFile, int width, int height, int mipLevels, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            return new TextureAdapter(TextureLoader.FromFile(device, srcFile, width, height, mipLevels, usage, format, pool, filter, mipFilter, colorKey));
        }
        
        #region ITexture Members

        public bool Disposed
        {
            get { return texture.Disposed; }
        }

        public void AddDirtyRectangle()
        {
            texture.AddDirtyRectangle();
        }

        public void AddDirtyRectangle(Rectangle rect)
        {
            texture.AddDirtyRectangle(rect);
        }

        public SurfaceDescription GetLevelDescription(int level)
        {
            return texture.GetLevelDescription(level);
        }

        public ISurface GetSurfaceLevel(int level)
        {
            return new SurfaceAdapter(texture.GetSurfaceLevel(level));
        }

        public GraphicsStream LockRectangle(int level, LockFlags flags)
        {
            return texture.LockRectangle(level, flags);
        }

        public GraphicsStream LockRectangle(int level, LockFlags flags, out int pitch)
        {
            return texture.LockRectangle(level, flags, out pitch);
        }

        public GraphicsStream LockRectangle(int level, Rectangle rect, LockFlags flags)
        {
            return texture.LockRectangle(level, rect, flags);
        }

        public GraphicsStream LockRectangle(int level, Rectangle rect, LockFlags flags, out int pitch)
        {
            return texture.LockRectangle(level, rect, flags, out pitch);
        }

        public Array LockRectangle(Type typeLock, int level, LockFlags flags, params int[] ranks)
        {
            return texture.LockRectangle(typeLock, level, flags, ranks);
        }

        public Array LockRectangle(Type typeLock, int level, LockFlags flags, out int pitch, params int[] ranks)
        {
            return texture.LockRectangle(typeLock, level, flags, out pitch, ranks);
        }

        public Array LockRectangle(Type typeLock, int level, Rectangle rect, LockFlags flags, params int[] ranks)
        {
            return texture.LockRectangle(typeLock, level, rect, flags, ranks);
        }

        public Array LockRectangle(Type typeLock, int level, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks)
        {
            return texture.LockRectangle(typeLock, level, rect, flags, out pitch, ranks);
        }

        public void UnlockRectangle(int level)
        {
            texture.UnlockRectangle(level);
        }

        #endregion
    }
}
