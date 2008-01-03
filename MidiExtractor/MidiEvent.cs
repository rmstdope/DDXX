using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.MidiExtractor
{
    public class MidiEvent
    {
        private int time;
        private byte eventType;
        private byte arg1;
        private byte arg2;
        private float realTime;

        public MidiEvent(int time, byte eventType, byte arg1, byte arg2)
        {
            this.time = time;
            this.eventType = eventType;
            this.arg1 = arg1;
            this.arg2 = arg2;
        }

        public int Time
        {
            get { return time; }
        }

        public byte EventType
        {
            get { return eventType; }
        }

        public byte Arg1
        {
            get { return arg1; }
        }

        public byte Arg2
        {
            get { return arg2; }
        }

        public float RealTime
        {
            get { return realTime; }
            set { realTime = value; }
        }

    }
}
