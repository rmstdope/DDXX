using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Dope.DDXX.FMOD;

namespace Dope.DDXX.Sound
{
    public class SoundDriver
    {
        private static SoundDriver instance;
        private ISoundSystem system;
        private static ISoundFactory factory = new FMODFactory();

        private SoundDriver()
        {
        }

        public static ISoundFactory Factory
        {
            get { return factory; }
            set { factory = value; }
        }

        public static SoundDriver GetInstance()
        {
            if (instance == null)
            {
                instance = new SoundDriver();
            }
            return instance;
        }

        public void Initialize()
        {
            RESULT result;

            ISoundSystem sys = factory.CreateSystem();

            CheckVersion(sys);
        
            result = sys.Init(64, Dope.DDXX.FMOD.INITFLAG.NORMAL, (IntPtr)null);
            if (result != RESULT.OK)
                throw new DDXXException("Could not initialize FMOD.");

            system = sys;
        }

        public Dope.DDXX.FMOD.Sound CreateSound(string file)
        {
            RESULT result;

            Dope.DDXX.FMOD.Sound sound = new Dope.DDXX.FMOD.Sound();

            result = system.CreateSound("../../Data/" + file, Dope.DDXX.FMOD.MODE._2D | Dope.DDXX.FMOD.MODE.HARDWARE | Dope.DDXX.FMOD.MODE.CREATESTREAM, ref sound);

            if (result != RESULT.OK)
                throw new DDXXException("Could not load FMOD file " + file + ".");

            return sound;
        }

        private void CheckVersion(ISoundSystem sys)
        {
            RESULT result;
            uint version = VERSION.number;

            result = sys.GetVersion(ref version);
            
            if (result != RESULT.OK)
                throw new DDXXException("Could not get FMOD version.");

            if (version != VERSION.number)
                throw new DDXXException("Mismatch in FMOD version between compiled binary and dll.");
        }


        public Channel PlaySound(Dope.DDXX.FMOD.Sound sound)
        {
            RESULT result;
            Channel channel = new Channel();
            
            result = system.PlaySound(Dope.DDXX.FMOD.CHANNELINDEX.FREE, sound, false, ref channel);

            if (result != RESULT.OK)
                throw new DDXXException("Could not start playing FMOD sound.");

            return channel;
        }

        public void PauseChannel(Channel channel)
        {
            RESULT result;

            result = system.SetPaused(channel, true);

            if (result != RESULT.OK)
                throw new DDXXException("Could not pause FMOD channel.");
        }

        public void ResumeChannel(Channel channel)
        {
            RESULT result;

            result = system.SetPaused(channel, false);

            if (result != RESULT.OK)
                throw new DDXXException("Could not resume FMOD channel.");
        }

        public float GetPosition(Channel channel)
        {
            RESULT result;
            uint ms = 0;

            result = system.GetPosition(channel, ref ms, TIMEUNIT.MS);

            if (result != RESULT.OK)
                throw new DDXXException("Could not get FMOD channel position.");

            return ms / 1000.0f;
        }

        public void SetPosition(Channel channel, float s)
        {
            RESULT result;

            result = system.SetPosition(channel, (uint)(s * 1000), TIMEUNIT.MS);

            if (result != RESULT.OK)
                throw new DDXXException("Could not set FMOD channel position.");
        }
    }
}
