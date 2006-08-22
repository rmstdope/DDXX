using System;
using System.Collections.Generic;
using System.Text;
using FMOD;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Sound
{
    public class FMODSystem : ISoundSystem
    {
        private FMOD.System system = null;

        public FMODSystem()
        {
            RESULT result;

            result = Factory.System_Create(ref system);
            if (result != RESULT.OK)
                throw new DDXXException("Could not initialize FMOD driver.");
        }

        #region ISystem Members

        public RESULT GetVersion(ref uint version)
        {
            return system.getVersion(ref version);
        }

        public RESULT Init(int maxchannels, INITFLAG flags, IntPtr extradata)
        {
            return system.init(maxchannels, flags, extradata);
        }

        internal FMOD.System GetSystem()
        {
            return system;
        }

        public RESULT CreateSound(string name_or_data, MODE mode, ref FMOD.Sound sound)
        {
            return system.createSound(name_or_data, mode, ref sound);
        }

        public RESULT PlaySound(CHANNELINDEX channelid, FMOD.Sound sound, bool paused, ref Channel channel)
        {
            return system.playSound(channelid, sound, paused, ref channel);
        }

        public RESULT SetPaused(Channel channel, bool paused)
        {
            return channel.setPaused(paused);
        }

        public RESULT GetPosition(Channel channel, ref uint position, TIMEUNIT postype)
        {
            return channel.getPosition(ref position, postype);
        }

        public RESULT SetPosition(Channel channel, uint position, TIMEUNIT postype)
        {
            return channel.setPosition(position, postype);
        }

        #endregion
    }
}
