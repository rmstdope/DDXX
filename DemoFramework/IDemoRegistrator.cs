using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoRegistrator
    {
        float StartTime { get; }
        float EndTime { get; }
        List<ITrack> Tracks { get; }
        IDemoEffect[] Effects(int track);
        IDemoPostEffect[] PostEffects(int track);
        void Register(int track, IDemoEffect effect);
        void Register(int track, IDemoPostEffect postEffect);
        List<IRegisterable> GetAllRegisterables();
        ITextureFactory TextureFactory { get; }
        IDemoEffectTypes EffectTypes { get; }
    }
}
