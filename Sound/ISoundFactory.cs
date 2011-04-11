using System;
using System.Collections.Generic;
using System.Text;
//using Dope.DDXX.MidiProcessor;

namespace Dope.DDXX.Sound
{
    public interface ISoundFactory
    {
        //CompiledMidi Midi { get; }
        void Initialize(string resourceName);
        ICue CreateCue(string name);
        void Step();
    }
}
