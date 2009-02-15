using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.Sound
{
    public class SoundDriver : ISoundDriver
    {
        private static SoundDriver instance;
        //private ISoundSystem system;
        private static ISoundFactory factory;

        private SoundDriver(IContentManager contentManager)
        {
            factory = new SoundFactory(contentManager)
        }

        public static ISoundFactory Factory
        {
            set { factory = value; }
        }

        public static SoundDriver GetInstance(content)
        {
            if (instance == null)
            {
                instance = new SoundDriver();
            }
            return instance;
        }

        public void Initialize(string resourceName)
        {
            factory.Initialize(resourceName);
        }

        public ICue PlaySound(string name)
        {
            ICue cue = factory.CreateCue(name);
            cue.Play();
            return cue;
        }


        public void Step()
        {
            factory.Step();
        }
    }
}
