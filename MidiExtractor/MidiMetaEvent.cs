using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.MidiExtractor
{
    public class MidiMetaEvent : MidiEvent
    {
        private byte metaEventType;
        private byte[] metaData;

        public MidiMetaEvent(int time, byte metaEventType, params byte[] metaData)
            : base(time, 0xFF, 0, 0)
        {
            this.metaEventType = metaEventType;
            this.metaData = metaData;
        }

        public int MetaEventType
        {
            get { return metaEventType; }
        }

        public byte[] MetaData
        {
            get { return metaData; }
        }
    }
}
