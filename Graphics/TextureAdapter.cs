using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class TextureAdapter : ITexture
    {
        Texture texture;

        //
        // Summary:
        //     Creates a new instance of the Texture class.
        //
        // Parameters:
        //   device:
        //     A Device object to associate with the Texture.
        //
        //   image:
        //     A System.Drawing.Bitmap used to create the texture.
        //
        //   usage:
        //     Usage can be 0, which indicates no usage value. However, if usage is desired,
        //     use one or more Usage constants. It is good practice
        //     to match the usage parameter with the CreateFlags
        //     in the Device.#ctor() constructor.
        //
        //   pool:
        //     Member of the Pool enumerated type that describes
        //     the memory class into which the texture should be placed.
        public TextureAdapter(Device device, Bitmap image, Usage usage, Pool pool)
        {
            texture = new Texture(device, image, usage, pool);
        }
        //
        // Summary:
        //     Creates a new instance of the Texture class.
        //
        // Parameters:
        //   device:
        //     A Device object to associate with the Texture.
        //
        //   data:
        //     A System.IO.Stream object that contains the image data. The texture is created
        //     with the data in the stream.
        //
        //   usage:
        //     Usage can be 0, which indicates no usage value. However, if usage is desired,
        //     use one or more Usage constants. It is good practice
        //     to match the usage parameter with the CreateFlags
        //     in the Device.#ctor() constructor.
        //
        //   pool:
        //     Member of the Pool enumerated type that describes
        //     the memory class into which the texture should be placed.
        public TextureAdapter(Device device, Stream data, Usage usage, Pool pool)
        {
            texture = new Texture(device, data, usage, pool);
        }
        //
        // Summary:
        //     Creates a new instance of the Texture class.
        //
        // Parameters:
        //   device:
        //     A Device object to associate with the Texture.
        //
        //   width:
        //     Width of the texture's top level, in pixels. The pixel dimensions of subsequent
        //     levels are the truncated value of half of the previous level's pixel dimension
        //     (independently). Each dimension clamps at a size of one pixel. Thus, if the
        //     division by 2 results in 0, 1 is taken instead.
        //
        //   height:
        //     Height of the texture's top level, in pixels. The pixel dimensions of subsequent
        //     levels are the truncated value of half of the previous level's pixel dimension
        //     (independently). Each dimension clamps at a size of one pixel. Thus, if the
        //     division by 2 results in 0, 1 is taken instead.
        //
        //   numLevels:
        //     Number of levels in the texture. If this is 0, Microsoft Direct3D generates
        //     all texture sublevels down to 1 by 1 pixels for hardware that supports mipmapped
        //     textures. Check the BaseTexture.LevelCount parameter
        //     to see the number of levels generated.
        //
        //   usage:
        //     Usage can be 0, which indicates no usage value. However, if usage is desired,
        //     use one or more Usage constants. It is good practice
        //     to match the usage parameter with the CreateFlags
        //     in the Device.#ctor() constructor.
        //
        //   format:
        //     Member of the Format enumerated type that describes
        //     the format of all levels in the texture.
        //
        //   pool:
        //     Member of the Pool enumerated type that describes
        //     the memory class into which the texture should be placed.
        public TextureAdapter(Device device, int width, int height, int numLevels, Usage usage, Format format, Pool pool)
        {
            texture = new Texture(device, width, height, numLevels, usage, format, pool);
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

        public void Dispose()
        {
            texture.Dispose();
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
