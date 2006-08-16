using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.FMOD;

namespace Dope.DDXX.Sound
{
    public interface ISoundSystem
    {
        RESULT GetVersion(ref uint version);
        RESULT Init(int maxchannels, INITFLAG flags, IntPtr extradata);
        RESULT CreateSound(string name_or_data, MODE mode, ref Dope.DDXX.FMOD.Sound sound);
        RESULT PlaySound(CHANNELINDEX channelid, Dope.DDXX.FMOD.Sound sound, bool paused, ref Channel channel);
        RESULT SetPaused(Channel channel, bool paused);
        RESULT GetPosition(Channel channel, ref uint position, TIMEUNIT postype);
        RESULT SetPosition(Channel channel, uint position, TIMEUNIT postype);
    }
}
