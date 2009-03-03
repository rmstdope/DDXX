using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.MidiProcessorLib;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoMixer
    {
        void SetClearColor(int track, Color color);
        Color GetClearColor(int track);
        CompiledMidi CompiledMidi { get; }
    }
}
