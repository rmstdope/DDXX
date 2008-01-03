using System;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface ITexture : IGraphicsResource
    {
        // Summary:
        //     Gets the number of texture levels in a multilevel texture.
        //
        // Returns:
        //     The number of texture levels in the multilevel texture.
        int LevelCount { get; }
        //
        // Summary:
        //     Gets or sets the most detailed level-of-detail for a managed texture.
        //
        // Returns:
        //     The most detailed level-of-detail for a managed texture.
        int LevelOfDetail { get; set; }
        //
        // Summary:
        //     Request generation of mipmap sublevels for a render target texture.
        //
        // Parameters:
        //   filterType:
        //     Determines how the texture will be filtered for each mipmap level.
        void GenerateMipMaps(TextureFilter filterType);
#if (!XBOX)
        //
        // Summary:
        //     [Windows Only] Saves a texture to a file.
        //
        // Parameters:
        //   filename:
        //     The file name of the destination image.
        //
        //   format:
        //     The file format to use when saving. This function supports saving to all
        //     ImageFileFormat formats except Portable Pixmap (.ppm) and Targa/Truevision
        //     Graphics Adapter (.tga).
        void Save(string filename, ImageFileFormat format);
#endif
    }
}
