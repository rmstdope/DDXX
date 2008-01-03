using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace Dope.DDXX.ContentProcessor
{
    [ContentProcessor]
    public class DdxxMaterialProcessor : MaterialProcessor
    {
        protected override ExternalReference<TextureContent> BuildTexture(string textureName, 
            ExternalReference<TextureContent> texture, ContentProcessorContext context)
        {
            if (!texture.Filename.Contains("/textures/") &&
                !texture.Filename.Contains("\\textures\\"))
            {
                System.IO.FileInfo info = new System.IO.FileInfo(texture.Filename);
                texture.Filename = info.Directory.Parent.FullName + "\\textures\\" + info.Name;
                //texture.Filename.LastIndexOf("/")
                //texture.Filename += "../textures/";
            }
            System.Diagnostics.Debug.WriteLine("Processing texture \"" + textureName + "\" from \"" + texture.Filename + "\"");
            if (textureName == "NormalMap")
            {
                return context.BuildAsset<TextureContent, TextureContent>(texture,
                    typeof(NormalMapTextureProcessor).Name);
            }

            return base.BuildTexture(textureName, texture, context);
        }
    }
}
