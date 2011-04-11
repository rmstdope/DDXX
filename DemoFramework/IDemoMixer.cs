using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
//using Dope.DDXX.MidiProcessor;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoMixer
    {
        void SetClearColor(int track, Color color);
        Color GetClearColor(int track);
        //CompiledMidi CompiledMidi { get; }
    }
}
