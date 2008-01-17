using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Dope.DDXX.Sound
{
    internal class SoundFactory : ISoundFactory
    {
        private AudioEngine audioEngine;
        private WaveBank waveBank;
        private SoundBank soundBank;

        public void Initialize(string resourceName)
        {
            audioEngine = new AudioEngine("Content\\sound\\" + resourceName + ".xgs");
            waveBank = new WaveBank(audioEngine, "Content\\sound\\" + resourceName + ".xwb");
            soundBank = new SoundBank(audioEngine, "Content\\sound\\" + resourceName + ".xsb");
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
