using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Dope.DDXX.MidiExtractor;
using System.Runtime.Serialization;

namespace Dope.DDXX.MidiProcessorLib
{
    [Serializable]
    public class MidiSource : ISerializable
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

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
