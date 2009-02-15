using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.MidiProcessorLib;

namespace Dope.DDXX.DemoFramework
{
    public interface IDemoMixer
    {
        Color ClearColor { get; set; }
        CompiledMidi CompiledMidi { get; }
    }
}
