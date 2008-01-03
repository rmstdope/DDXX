using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dope.DDXX.MidiExtractor
{
    public class MTrk : MidiChunk, IMTrk
    {
        private int length;
        private List<MidiEvent> events = new List<MidiEvent>();

        public MTrk()
            : base()
        {
        }


        public MTrk(BinaryReader reader, IEventParser eventParser)
            : base(reader)
        {
            ReadLength(reader);

            int pos = 0;
            int lastTime = 0;
            byte lastEventType = 0;
            int numBytes;
            while (pos < length)
            {
                MidiEvent midiEvent = eventParser.ParseEvent(reader, lastTime, lastEventType, out numBytes);
                pos += numBytes;
                lastTime = midiEvent.Time;
                lastEventType = midiEvent.EventType;
                events.Add(midiEvent);
            }
        }

        override protected byte[] ExpectedID
        {
            get { return new byte[] { 0x4D, 0x54, 0x72, 0x6B }; }
        }

        public List<MidiEvent> Events 
        {
            get { return events; } 
        }

        public int NumEvents
        {
            get { return events.Count; }
        }

        private void ReadLength(BinaryReader reader)
        {
            length = ReadUint32(reader);
        }

        public string Name
        {
            get 
            {
                string name = GetMetaString(3);
                if (name == null)
                    return "Unnamed";
                else 
                    return name;
            }
        }

        private string GetMetaString(int metaEventType)
        {
            foreach (MidiEvent e in events)
            {
                if (e is MidiMetaEvent)
                {
                    MidiMetaEvent me = e as MidiMetaEvent;
                    if (me.MetaEventType == metaEventType)
                    {
                        string retVal = "";
                        for (int i = 0; i < me.MetaData.Length; i++)
                            retVal += (char)me.MetaData[i];
                        return retVal;
                    }
                }
            }
            return null;
        }

        public string InstrumentName
        {
            get
            {
                string name = GetMetaString(4);
                if (name == null)
                    return "Unspecified";
                else
                    return name;
            }
        }

        public void WriteMetaData()
        {
            foreach (MidiEvent e in events)
            {
                if (e is MidiMetaEvent)
                {
                    MidiMetaEvent me = e as MidiMetaEvent;
                    string str = "";
                    for (int i = 0; i < me.MetaData.Length; i++)
                        str += (char)me.MetaData[i];
                    string type;
                    switch (me.MetaEventType)
                    {
                        case 0x02:
                            type = "Copyright";
                            break;
                        case 0x03:
                            type = "Track name";
                            break;
                        case 0x06:
                            type = "Marker";
                            break;
                        case 0x2F:
                            type = "End of track";
                            break;
                        case 0x51:
                            type = "Tempo (microsec per QN)";
                            str = ((me.MetaData[0] << 16) + (me.MetaData[1] << 8) + me.MetaData[2]).ToString();
                            break;
                        case 0x58:
                            type = "Time signature";
                            str = me.MetaData[0] + "/" + Math.Pow(2, me.MetaData[1]) + ", " + me.MetaData[2] +
                                " MIDI clocks per QN, " + me.MetaData[3] + " 32nd notes in a MIDI QN";
                            break;
                        default:
                            type = "Unknown(" + me.MetaEventType + ")";
                            break;
                    }
                    string realTime = me.RealTime.ToString();
                    string ticks = me.Time.ToString();
                    Console.WriteLine("(" + realTime + "/" + ticks + "): " + type + " = " + str);
                }
            }
        }


        public List<TimeInfo> ExtractTimeInfo(int division)
        {
            List<TimeInfo> infoList = new List<TimeInfo>();
            foreach (MidiEvent e in events)
            {
                MidiMetaEvent me = e as MidiMetaEvent;
                if (me != null && me.MetaEventType == 0x51)
                    infoList.Add(new TimeInfo(me.Time, BytesToUint24(me.MetaData) / division));
            }
            if (infoList.Count == 0 || infoList[0].TickNumber != 0)
            {
                infoList.Insert(0, new TimeInfo(0, 500000 / division));
            }
            return infoList;
        }

        public void CalcRealTime(List<TimeInfo> info)
        {
            List<TimeInfo>.Enumerator timeIterator = info.GetEnumerator();
            timeIterator.MoveNext();
            TimeInfo currentInfo = timeIterator.Current;
            TimeInfo nextInfo = null;
            if (timeIterator.MoveNext())
                nextInfo = timeIterator.Current;
            int sumMicroSeconds = 0;
            foreach (MidiEvent e in events)
            {
                while (nextInfo != null && timeIterator.Current.TickNumber < e.Time)
                {
                    // Next tick change occurs first
                    int ticks = nextInfo.TickNumber - currentInfo.TickNumber;
                    sumMicroSeconds += ticks * currentInfo.MicroSecondsPerTick;
                    currentInfo = nextInfo;
                    if (timeIterator.MoveNext())
                        nextInfo = timeIterator.Current;
                    else
                        nextInfo = null;
                }
                e.RealTime = ((float)(sumMicroSeconds + currentInfo.MicroSecondsPerTick * (e.Time - currentInfo.TickNumber))) / 1000000;
            }
        }

        public MidiEvent GetEvent(int index)
        {
            return events[index];
        }

        #region Code for test
        public void AddEvent(MidiEvent midiEvent)
        {
            events.Add(midiEvent);
        }
        #endregion
    }
}
