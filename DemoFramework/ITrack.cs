using System;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Dope.DDXX.DemoFramework
{
    public interface ITrack
    {
        IDemoEffect[] Effects { get; }
        List<IDemoEffect> EffectList { get; }
        float EndTime { get; }
        float StartTime { get; }
        IDemoEffect[] GetEffects(float time);
        IDemoEffect[] GetEffects(float startTime, float endTime);
        IDemoPostEffect[] GetPostEffects(float startTime, float endTime);
        IDemoPostEffect[] GetPostEffects(float time);
        void Initialize(IGraphicsFactory graphicsFactory, IGraphicsDevice device, ITextureFactory textureFactory, 
            IEffectFactory effectFactory, ITextureBuilder textureBuilder, IDemoMixer mixer, 
            IPostProcessor postProcessor);
        bool IsActive(float p);
        IDemoPostEffect[] PostEffects { get; }
        List<IDemoPostEffect> PostEffectList { get; }
        void Register(IDemoPostEffect postEffect);
        void Register(IDemoEffect effect);
        IRenderTarget2D Render(IGraphicsDevice device, IRenderTarget2D renderTarget, IRenderTarget2D renderTargetNoMultiSampling, IDepthStencilBuffer depthStencilBuffer, Color backgroundColor);
        void Step();
        //void UpdateListener(IEffectChangeListener effectChangeListener);
        bool IsEffectRegistered(string name, Type type);
        bool IsPostEffectRegistered(string p, Type type);
    }
}
