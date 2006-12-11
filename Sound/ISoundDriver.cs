using System;
using FMOD;

namespace Dope.DDXX.Sound
{
    public interface ISoundDriver
    {
        FMOD.Sound CreateSound(string file);
        float GetPosition(Channel channel);
        void Initialize();
        void PauseChannel(Channel channel);
        Channel PlaySound(FMOD.Sound sound);
        void ResumeChannel(Channel channel);
        void SetPosition(Channel channel, float s);
    }
}
