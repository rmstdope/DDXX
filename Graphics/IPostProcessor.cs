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
        RenderTarget2D OutputTexture { get; }
        void StartFrame(RenderTarget2D startTexture);
        void Process(string technique, RenderTarget2D sourceTexture, RenderTarget2D destinationTexture);
        void Process(string technique, Texture2D sourceTexture, RenderTarget2D destinationTexture);
        BlendState BlendState { set; }
        void SetValue(string parameter, float value);
        void SetValue(string parameter, float[] value);
        void SetValue(string parameter, Vector2 value);
        void SetValue(string parameter, Vector4 value);
        void SetValue(string parameter, Texture2D value);
        List<RenderTarget2D> GetTemporaryTextures(int num, bool skipOutput);
        void AllocateTexture(RenderTarget2D texture);
        void FreeTexture(RenderTarget2D texture);
    }
}
