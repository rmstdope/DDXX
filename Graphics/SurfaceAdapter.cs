using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class SurfaceAdapter : ISurface
    {
        private Surface surface;

        public SurfaceAdapter(Surface dxSurface)
        {
            surface = dxSurface;
        }

        #region ISurface Members

        public Surface DXSurface 
        {
            get { return surface; } 
        }

        public SurfaceDescription Description
        {
            get { return surface.Description; }
        }

        public bool Disposed
        {
            get { return surface.Disposed; }
        }

        public void Dispose()
        {
            surface.Dispose();
        }

        public System.Drawing.Graphics GetGraphics()
        {
            return surface.GetGraphics();
        }

        public GraphicsStream LockRectangle(LockFlags flags)
        {
            return surface.LockRectangle(flags);
        }

        public GraphicsStream LockRectangle(LockFlags flags, out int pitch)
        {
            return surface.LockRectangle(flags, out pitch);
        }

        public GraphicsStream LockRectangle(Rectangle rect, LockFlags flags)
        {
            return surface.LockRectangle(rect, flags);
        }

        public GraphicsStream LockRectangle(Rectangle rect, LockFlags flags, out int pitch)
        {
            return surface.LockRectangle(rect, flags, out pitch);
        }

        public Array LockRectangle(Type typeLock, LockFlags flags, params int[] ranks)
        {
            return surface.LockRectangle(typeLock, flags, ranks);
        }

        public Array LockRectangle(Type typeLock, LockFlags flags, out int pitch, params int[] ranks)
        {
            return surface.LockRectangle(typeLock, flags, out pitch, ranks);
        }

        public Array LockRectangle(Type typeLock, Rectangle rect, LockFlags flags, params int[] ranks)
        {
            return surface.LockRectangle(typeLock, rect, flags, ranks);
        }

        public Array LockRectangle(Type typeLock, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks)
        {
            return surface.LockRectangle(typeLock, rect, flags, out pitch, ranks);
        }

        public void ReleaseGraphics()
        {
            surface.ReleaseGraphics();
        }

        public void UnlockRectangle()
        {
            surface.UnlockRectangle();
        }

        #endregion
    }
}
