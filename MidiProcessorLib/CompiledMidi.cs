using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.MidiProcessorLib
{
    public class CompiledMidi
    {
        public class CompiledMidiTrack
        {
            private float[] notesAndTimes;

            public float[] NotesAndTimes
            {
                get { return notesAndTimes; }
            }

            public CompiledMidiTrack(float[] notesAndTimes)
            {
                this.notesAndTimes = notesAndTimes;
            }
        }

        private List<CompiledMidiTrack> tracks;
        public List<CompiledMidiTrack> Tracks
        {
            get { return tracks; }
        }

        public CompiledMidi()
        {
            this.tracks = new List<CompiledMidiTrack>();
        }

        public CompiledMidi(List<CompiledMidiTrack> tracks)
        {
            this.tracks = tracks;
        }

        public void AddTrack(CompiledMidiTrack track)
        {
            tracks.Add(track);
        }
    }
}
