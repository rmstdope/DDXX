using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{

    public interface IPostProcessor
    {
        void Initialize(IGraphicsFactory graphicsFactory);
        IRenderTarget2D OutputTexture { get; }
        void StartFrame(IRenderTarget2D startTexture);
        void Process(string technique, IRenderTarget2D sourceTexture, IRenderTarget2D destinationTexture);
        void Process(string technique, ITexture2D sourceTexture, IRenderTarget2D destinationTexture);
        void SetBlendParameters(BlendFunction blendFunctions, Blend sourceBlend, Blend destinatonBlend, Color blendFactor);
        void SetValue(string parameter, float value);
        void SetValue(string parameter, float[] value);
        void SetValue(string parameter, Vector2 value);
        void SetValue(string parameter, Vector4 value);
        void SetValue(string parameter, ITexture2D value);
        List<IRenderTarget2D> GetTemporaryTextures(int num, bool skipOutput);
        void AllocateTexture(IRenderTarget2D texture);
        void FreeTexture(IRenderTarget2D texture);
    }
}
