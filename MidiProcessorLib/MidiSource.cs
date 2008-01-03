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

        public List<IMTrk> Tracks
        {
            get { return tracks; }
        }
    }
}
