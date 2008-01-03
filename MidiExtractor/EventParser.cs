using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dope.DDXX.MidiExtractor
{
    public class EventParser : IEventParser
    {
        public MidiEvent ParseEvent(BinaryReader reader, int time, byte oldEventType, out int readBytes)
        {
            byte eventType;
            int deltaTime = ParseVariableLength(reader, out readBytes);
            time += deltaTime;
            eventType = reader.ReadByte();
            if (eventType < 0x80)
            {
                eventType = oldEventType;
                reader.BaseStream.Seek(-1, SeekOrigin.Current);
            }
            else
                readBytes++;
            if (eventType != 0xFF)
                return ParseNormalEvent(eventType, reader, time, ref readBytes);
            else
                return ParseMetaEvent(reader, time, ref readBytes);
        }

        private MidiEvent ParseMetaEvent(BinaryReader reader, int time, ref int readBytes)
        {
            byte eventType = reader.ReadByte();
            readBytes++;
            int variableBytes;
            int dataLength = ParseVariableLength(reader, out variableBytes);
            readBytes += variableBytes;
            byte[] data = reader.ReadBytes(dataLength);
            readBytes += dataLength;
            return new MidiMetaEvent(time, eventType, data);
        }

        private MidiEvent ParseNormalEvent(byte eventType, BinaryReader reader, 
            int time, ref int readBytes)
        {
            byte arg1 = 0;
            byte arg2 = 0;
            int numData = GetNumberOfDataBytes(eventType);
            if (numData > 0)
                arg1 = reader.ReadByte();
            if (numData > 1)
                arg2 = reader.ReadByte();
            readBytes += numData;
            return new MidiEvent(time, eventType, arg1, arg2);
        }

        private int GetNumberOfDataBytes(byte eventType)
        {
            if (eventType < 0x80 || eventType > 0xEF)
                throw new InvalidDataException("Event type " + eventType + " is not supported.");
            if (eventType >= 0xC0 && eventType <= 0xDF)
                return 1;
            return 2;
        }

        private int ParseVariableLength(BinaryReader reader, out int readBytes)
        {
            byte value;
            readBytes = 0;
            int sum = 0;
            do
            {
                readBytes++;
                value = reader.ReadByte();
                sum = (sum << 7) + (value & 0x7F);
            } while ((value & 0x80) == 0x80);
            return sum;
        }
    }
}
