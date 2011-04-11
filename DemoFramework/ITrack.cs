using System;
using Dope.DDXX.Graphics;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
//using Dope.DDXX.MidiProcessor;
using Microsoft.Xna.Framework;

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
        void Initialize(IGraphicsFactory graphicsFactory, IDemoMixer mixer, PostProcessor postProcessor);
        bool IsActive(float p);
        IDemoPostEffect[] PostEffects { get; }
        List<IDemoPostEffect> PostEffectList { get; }
        void Register(IDemoPostEffect postEffect);
        void Register(IDemoEffect effect);
        RenderTarget2D Render(GraphicsDevice device, RenderTarget2D renderTarget, RenderTarget2D renderTargetNoMultiSampling, Color backgroundColor);
        void Step();
        bool IsEffectRegistered(string name, Type type);
        bool IsPostEffectRegistered(string p, Type type);
    }
}
