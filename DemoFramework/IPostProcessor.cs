using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using Microsoft.DirectX;

namespace Dope.DDXX.DemoFramework
{
    public enum TextureID
    {
        FULLSIZE_TEXTURE_1 = 0,
        FULLSIZE_TEXTURE_2,
        FULLSIZE_TEXTURE_3,
        INPUT_TEXTURE,
        NUM_TEXTURES
    };

    public interface IPostProcessor
    {
        /// <summary>
        /// Initializes the post processor by getting the needed DX objects.
        /// </summary>
        /// <param name="device"></param>
        void Initialize(IDevice device);

        /// <summary>
        /// Get the ITexture of the texture that was last rendered.
        /// </summary>
        ITexture OutputTexture { get; }
        
        /// <summary>
        /// Get the ID of the texture that was last rendered.
        /// </summary>
        TextureID OutputTextureID { get; }
        
        /// <summary>
        /// Get the ITexture of a specific ID.
        /// </summary>
        /// <param name="textureID"></param>
        /// <returns></returns>
        ITexture GetTexture(TextureID textureID);
        
        /// <summary>
        /// Set the texture to use as startTexture. It is assumed that this texture is of full size.
        /// </summary>
        /// <param name="startTexture"></param>
        void StartFrame(ITexture startTexture);

        /// <summary>
        /// Generate an output texture.
        /// </summary>
        /// <param name="technique">The technique to use.</param>
        /// <param name="sourceTextureId">The ID of the texture to use as source.</param>
        /// <param name="destinationTextureId">The ID of the texture to use as destination. Can not be same as source.</param>
        void Process(string technique, TextureID sourceTextureId, TextureID destinationTextureId);
        
        /// <summary>
        /// Generate an output texture using an external texture as source.
        /// </summary>
        /// <param name="technique">The technique to use.</param>
        /// <param name="sourceTextureId">The ITexture to use as source.</param>
        /// <param name="destinationTextureId">The ID of the texture to use as destination. Can not be same as source.</param>
        void Process(string technique, ITexture sourceTexture, TextureID destinationTextureId);
        
        /// <summary>
        /// Set the blend parameters to use when processing.
        /// </summary>
        /// <param name="blendOperation">The operation to use.</param>
        /// <param name="sourceBlend">The blend technique for the source.</param>
        /// <param name="destinatonBlend">The blend technique for the destination.</param>
        /// <param name="blendFactor">The blend factor. Must be specified as 0xRRGGBBAA.</param>
        void SetBlendParameters(BlendOperation blendOperation, Blend sourceBlend, Blend destinatonBlend, Color blendFactor);
        
        /// <summary>
        /// Set the value of a parameter in the effects.
        /// </summary>
        /// <param name="parameter">The name of the parameter.</param>
        /// <param name="value">The value to set it to.</param>
        void SetValue(string parameter, float value);

        /// <summary>
        /// Set the value of a parameter in the effects.
        /// </summary>
        /// <param name="parameter">The name of the parameter.</param>
        /// <param name="value">The value to set it to.</param>
        void SetValue(string parameter, float[] value);

        /// <summary>
        /// Set the value of a parameter in the effects.
        /// </summary>
        /// <param name="parameter">The name of the parameter.</param>
        /// <param name="value">The value to set it to.</param>
        void SetValue(string parameter, Vector2 value);

        /// <summary>
        /// Set the value of a parameter in the effects.
        /// </summary>
        /// <param name="parameter">The name of the parameter.</param>
        /// <param name="value">The value to set it to.</param>
        void SetValue(string parameter, Vector4 value);

        void WriteToFile(TextureID textureID, string filename);
    }
}
