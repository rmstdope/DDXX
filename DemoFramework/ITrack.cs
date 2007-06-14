using System;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoFramework
{
    public interface ITrack
    {
        IDemoEffect[] Effects { get; }
        float EndTime { get; }
        IDemoEffect[] GetEffects(float time);
        IDemoEffect[] GetEffects(float startTime, float endTime);
        IDemoPostEffect[] GetPostEffects(float startTime, float endTime);
        IDemoPostEffect[] GetPostEffects(float time);
        void Initialize(IGraphicsFactory graphicsFactory, IDevice device, ITextureFactory textureFactory, 
            ITextureBuilder textureBuilder, IDemoMixer mixer, IPostProcessor postProcessor);
        bool IsActive(float p);
        IDemoPostEffect[] PostEffects { get; }
        void Register(IDemoPostEffect postEffect);
        void Register(IDemoEffect effect);
        void Render(IDevice device);
        void Step();
        void UpdateListener(IEffectChangeListener effectChangeListener);
    }
}
