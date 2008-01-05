using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Dope.DDXX.MidiProcessorLib
{
    public class MidiTypeReader : ContentTypeReader<CompiledMidi>
    {
        protected override CompiledMidi Read(ContentReader input, CompiledMidi existingInstance)
        {
            CompiledMidi compiledMidi = new CompiledMidi();
            int count = input.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                float[] notesArray = input.ReadObject<float[]>();
                compiledMidi.AddTrack(new CompiledMidi.CompiledMidiTrack(notesArray));
            }
            return compiledMidi;
        }
    }
}
