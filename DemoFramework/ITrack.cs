using System;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    public interface ITrack
    {
        IDemoEffect[] Effects { get; }
        float EndTime { get; }
        float StartTime { get; }
        IDemoEffect[] GetEffects(float time);
        IDemoEffect[] GetEffects(float startTime, float endTime);
        IDemoPostEffect[] GetPostEffects(float startTime, float endTime);
        IDemoPostEffect[] GetPostEffects(float time);
        void Initialize(IGraphicsFactory graphicsFactory, IDevice device, ITextureFactory textureFactory, 
            IEffectFactory effectFactory, ITextureBuilder textureBuilder, IDemoMixer mixer, 
            IPostProcessor postProcessor);
        bool IsActive(float p);
        IDemoPostEffect[] PostEffects { get; }
        void Register(IDemoPostEffect postEffect);
        void Register(IDemoEffect effect);
        ITexture Render(IDevice device, ITexture renderTarget, Color backgroundColor);
        void Step();
        void UpdateListener(IEffectChangeListener effectChangeListener);
        bool IsEffectRegistered(string name, Type type);
        bool IsPostEffectRegistered(string p, Type type);
    }
}
