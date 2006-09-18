using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

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
        void Initialize(IDevice device);
        ITexture OutputTexture { get; }
        TextureID OutputTextureID { get; }
        void StartFrame(ITexture startTexture);
        void Process(string technique, TextureID textureID, TextureID textureID_2);
        void SetBlendParameters(BlendOperation blendOperation, Blend sourceBlend, Blend destinatonBlend, Color blendFactor);

        void SetValue(string p, float luminance);
    }
}
