using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using FMOD;

namespace Dope.DDXX.Sound
{
    public class SoundDriver : Dope.DDXX.Sound.ISoundDriver
    {
        private static SoundDriver instance;
        private ISoundSystem system;
        private static ISoundFactory factory = new FMODFactory();

        private SoundDriver()
        {
        }

        public static ISoundFactory Factory
        {
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
        
            result = sys.Init(64, FMOD.INITFLAG.NORMAL, (IntPtr)null);
            if (result != RESULT.OK)
                throw new DDXXException("Could not initialize FMOD.");

            system = sys;
        }

        public FMOD.Sound CreateSound(string file)
        {
            RESULT result;

            FMOD.Sound sound = new FMOD.Sound();

            result = system.CreateSound(FileUtility.FilePath(file), FMOD.MODE._2D | FMOD.MODE.HARDWARE | FMOD.MODE.CREATESTREAM | FMOD.MODE.ACCURATETIME, ref sound);

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


        public Channel PlaySound(FMOD.Sound sound)
        {
            RESULT result;
            Channel channel = new Channel();
            
            result = system.PlaySound(FMOD.CHANNELINDEX.FREE, sound, false, ref channel);

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

            //if (result != RESULT.OK)
            //    throw new DDXXException("Could not set FMOD channel position.");
        }
    }
}
