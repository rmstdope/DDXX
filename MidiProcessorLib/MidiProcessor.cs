using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Dope.DDXX.MidiExtractor;

namespace Dope.DDXX.MidiProcessorLib
{
    /// <summary>
    /// This class will be instantiated by the XNA Framework Content Pipeline
    /// to apply custom processing to content data, converting an object of
    /// type TInput to TOutput. The input and output types may be the same if
    /// the processor wishes to alter data without changing its type.
    ///
    /// This should be part of a Content Pipeline Extension Library project.
    ///
    /// </summary>
    [ContentProcessor(DisplayName = "MIDI Processor")]
    public class MidiProcessor : ContentProcessor<MidiSource, CompiledMidi>
    {
        public override CompiledMidi Process(MidiSource input,
            ContentProcessorContext context)
        {
            return Process(input);
        }

        public CompiledMidi Process(MidiSource input)
        {
            CompiledMidi compiledMidi = new CompiledMidi();
            foreach (IMTrk trk in input.Tracks)
            {
                List<float> notesAndTimes = new List<float>();
                foreach (MidiEvent e in trk.Events)
                {
                    if (e.EventType == 0x90)
                    {
                        notesAndTimes.Add(e.RealTime);
                        notesAndTimes.Add(e.Arg1);
                    }
                }
                float[] notesArray = notesAndTimes.ToArray();
                compiledMidi.AddTrack(new CompiledMidi.CompiledMidiTrack(notesArray));
            }
            return compiledMidi;
        }
    }
}