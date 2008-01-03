using System;
using System.Collections.Generic;

namespace Dope.DDXX.MidiExtractor
{
    public interface IMTrk
    {
        void AddEvent(MidiEvent midiEvent);
        void CalcRealTime(System.Collections.Generic.List<TimeInfo> info);
        System.Collections.Generic.List<TimeInfo> ExtractTimeInfo(int division);
        List<MidiEvent> Events { get; }
        MidiEvent GetEvent(int index);
        string InstrumentName { get; }
        string Name { get; }
        int NumEvents { get; }
        void WriteMetaData();
    }
}
