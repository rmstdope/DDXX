using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Dope.DDXX.Graphics;
//using Dope.DDXX.MidiProcessor;
using Microsoft.Xna.Framework.Content;

namespace Dope.DDXX.Sound
{
    public class SoundFactory : ISoundFactory
    {
        private ContentManager contentManager;
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;
        //private CompiledMidi midi;

        public SoundFactory(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        //public CompiledMidi Midi { get { return midi; } }

        public void Initialize(string resourceName)
        {
            audioEngine = new AudioEngine("Content\\sound\\" + resourceName + ".xgs");
            waveBank = new WaveBank(audioEngine, "Content\\sound\\" + resourceName + ".xwb");
            soundBank = new SoundBank(audioEngine, "Content\\sound\\" + resourceName + ".xsb");
            try
            {
                //midi = contentManager.Load<CompiledMidi>("Content\\sound\\" + resourceName);
            }
            catch (Exception) { }
        }

        public ICue CreateCue(string name)
        {
            return new CueAdapter(soundBank.GetCue(name));
        }

        public void Step()
        {
            if (audioEngine != null)
                audioEngine.Update();
        }
    }
}
