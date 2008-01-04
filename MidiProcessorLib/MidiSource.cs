using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Dope.DDXX.MidiExtractor;

namespace Dope.DDXX.MidiProcessorLib
{
    public class MidiSource
    {
        private List<IMTrk> tracks = new List<IMTrk>();
        private MThd header;

        public MThd Header
        {
            get { return header; }
        }

        public List<IMTrk> Tracks
        {
            get { return tracks; }
        }

        public MidiSource(MThd header, List<IMTrk> tracks)
        {
            this.header = header;
            this.tracks = tracks;
        }
    }
}
