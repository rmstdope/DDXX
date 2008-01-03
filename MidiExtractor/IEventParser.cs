using System;
using System.IO;
namespace Dope.DDXX.MidiExtractor
{
    public interface IEventParser
    {
        MidiEvent ParseEvent(BinaryReader reader, int time, byte oldEventType, out int readBytes);
    }
}
