using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{

    public interface IPostProcessor
    {
        /// <summary>
        /// Initializes the post processor by getting the needed DX objects.
        /// </summary>
        /// <param name="device"></param>
        void Initialize(IGraphicsFactory graphicsFactory);

        /// <summary>
        /// Get the ITexture of the texture that was last rendered.
        /// </summary>
        IRenderTarget2D OutputTexture { get; }
        
        /// <summary>
        /// Set the texture to use as startTexture. It is assumed that this texture is of full size.
        /// </summary>
        /// <param name="startTexture"></param>
        void StartFrame(IRenderTarget2D startTexture);

        /// <summary>
        /// Generate an output texture using an external texture as source.
        /// </summary>
        /// <param name="technique">The technique to use.</param>
        /// <param name="sourceTexture">The IRenderTarget2D to use as source.</param>
        /// <param name="destinationTexture">The IRenderTarget2D to use as destination. Can not be same as source.</param>
        void Process(string technique, IRenderTarget2D sourceTexture, IRenderTarget2D destinationTexture);

        /// <summary>
        /// Generate an output texture using an external texture as source.
        /// </summary>
        /// <param name="technique">The technique to use.</param>
        /// <param name="sourceTexture">The ITexture2D to use as source.</param>
        /// <param name="destinationTexture">The IRenderTarget2D to use as destination. Can not be same as source.</param>
        void Process(string technique, ITexture2D sourceTexture, IRenderTarget2D destinationTexture);

        /// <summary>
        /// Set the blend parameters to use when processing.
        /// </summary>
        /// <param name="blendOperation">The operation to use.</param>
        /// <param name="sourceBlend">The blend technique for the source.</param>
        /// <param name="destinatonBlend">The blend technique for the destination.</param>
        /// <param name="blendFactor">The blend factor. Must be specified as 0xRRGGBBAA.</param>
        void SetBlendParameters(BlendFunction blendFunctions, Blend sourceBlend, Blend destinatonBlend, Color blendFactor);
        
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

        /// <summary>
        /// Get a number of textures that are currently allocated.
        /// These textures are for temporary use.
        /// </summary>
        /// <param name="num">The number of textures to retrieve.</param>
        /// <returns>A list containing the textures</returns>
        List<IRenderTarget2D> GetTemporaryTextures(int num, bool skipOutput);

        /// <summary>
        /// Allocates a full size texture exclusively until it is freed.
        /// </summary>
        /// <returns>The allocated texture.</returns>
        void AllocateTexture(IRenderTarget2D texture);

        /// <summary>
        /// Free an allocated texture for use.
        /// </summary>
        /// <param name="texture"></param>
        void FreeTexture(IRenderTarget2D texture);
    }
}