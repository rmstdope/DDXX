using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.MidiExtractor
{
    public class TimeInfo
    {
        public int TickNumber;
        public int MicroSecondsPerTick;
        public TimeInfo(int tickNumber, int microSecondsPerTick)
        {
            TickNumber = tickNumber;
            MicroSecondsPerTick = microSecondsPerTick;
        }
    }
}
