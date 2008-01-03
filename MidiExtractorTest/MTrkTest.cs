using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;

namespace Dope.DDXX.MidiExtractor
{
    [TestFixture]
    public class MTrkTest : IEventParser
    {
        private byte[] standardHeader;
        private BinaryReader reader;
        private MidiEvent midiEvent;
        private int expectedTime;
        private int bytesPerRead;
        private byte expectedLastEvent;

        [SetUp]
        public void SetUp()
        {
            expectedTime = 0;
            expectedLastEvent = 0;
            standardHeader = new byte[] { 
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 0x00  // Length of the MTrk chunk
            };
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestInvalidHeader1()
        {
            standardHeader[0] = 0;
            CreateTrack(standardHeader);
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestInvalidHeader2()
        {
            standardHeader[1] = 0;
            CreateTrack(standardHeader);
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestInvalidHeader3()
        {
            standardHeader[2] = 0;
            CreateTrack(standardHeader);
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestInvalidHeader4()
        {
            standardHeader[3] = 0;
            CreateTrack(standardHeader);
        }

        [Test]
        public void TestEmptyTrack()
        {
            MTrk track = CreateTrack(standardHeader);
            Assert.AreEqual(0, track.NumEvents);
        }

        [Test]
        public void TestTrackWithOneEvent()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 1  // Length of the MTrk chunk
            };
            midiEvent = new MidiEvent(1, 2, 3, 4);
            bytesPerRead = 1;
            MTrk track = CreateTrack(data);
            Assert.AreEqual(1, track.NumEvents);
        }

        [Test]
        public void TestTrackWithTwoEvent()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 6  // Length of the MTrk chunk
            };
            midiEvent = new MidiEvent(0, 1, 2, 3);
            bytesPerRead = 3;
            MTrk track = CreateTrack(data);
            Assert.AreEqual(2, track.NumEvents);
        }

        [Test]
        public void TestTrackWithHugeEvent()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x10, 0x00, 0x00, 0x00  // Length of the MTrk chunk
            };
            midiEvent = new MidiEvent(0, 1, 2, 3);
            bytesPerRead = 0x10000000;
            MTrk track = CreateTrack(data);
            Assert.AreEqual(1, track.NumEvents);
        }

        [Test]
        public void TestNameNoMeta()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 0x01  // Length of the MTrk chunk
            };
            midiEvent = new MidiEvent(0, 0x80, 0, 0);
            bytesPerRead = 1;
            MTrk track = CreateTrack(data);
            Assert.AreEqual("Unnamed", track.Name);
        }

        [Test]
        public void TestNameWithMetaab()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 0x01  // Length of the MTrk chunk
            };
            midiEvent = new MidiMetaEvent(0, 3, (byte)'a', (byte)'b');
            bytesPerRead = 1;
            MTrk track = CreateTrack(data);
            Assert.AreEqual("ab", track.Name);
        }

        [Test]
        public void TestInstrumentNameNoMeta()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 0x01  // Length of the MTrk chunk
            };
            midiEvent = new MidiEvent(0, 0x80, 0, 0);
            bytesPerRead = 1;
            MTrk track = CreateTrack(data);
            Assert.AreEqual("Unspecified", track.InstrumentName);
        }

        [Test]
        public void TestInstrumentNameMetaXY()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 0x01  // Length of the MTrk chunk
            };
            midiEvent = new MidiMetaEvent(0, 4, (byte)'X', (byte)'Y');
            bytesPerRead = 1;
            MTrk track = CreateTrack(data);
            Assert.AreEqual("XY", track.InstrumentName);
        }

        [Test]
        public void TestNameWithMetaX()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 0x01  // Length of the MTrk chunk
            };
            midiEvent = new MidiMetaEvent(0, 3, (byte)'X');
            bytesPerRead = 1;
            MTrk track = CreateTrack(data);
            Assert.AreEqual("X", track.Name);
        }

        private MTrk CreateTrack(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            reader = new BinaryReader(stream);
            MTrk track = new MTrk(reader, this);
            return track;
        }

        [Test]
        public void TestExtractTimeInfoNoMeta()
        {
            MTrk track = new MTrk();
            List<TimeInfo> info = track.ExtractTimeInfo(24);
            Assert.AreEqual(1, info.Count);
            Assert.AreEqual(0, info[0].TickNumber);
            Assert.AreEqual(500000 / 24, info[0].MicroSecondsPerTick);
        }

        [Test]
        public void TestExtractTimeInfoOtherMeta()
        {
            MTrk track = new MTrk();
            track.AddEvent(new MidiMetaEvent(0, 3));
            List<TimeInfo> info = track.ExtractTimeInfo(500);
            Assert.AreEqual(1, info.Count);
            Assert.AreEqual(0, info[0].TickNumber);
            Assert.AreEqual(500000 / 500, info[0].MicroSecondsPerTick);
        }

        [Test]
        public void TestExtractTimeInfoTempoAt0()
        {
            MTrk track = new MTrk();
            track.AddEvent(new MidiMetaEvent(0, 0x51, 0, 0, 24));
            List<TimeInfo> info = track.ExtractTimeInfo(24);
            Assert.AreEqual(1, info.Count);
            Assert.AreEqual(0, info[0].TickNumber);
            Assert.AreEqual(24 / 24, info[0].MicroSecondsPerTick);
        }

        [Test]
        public void TestExtractTimeInfoTempoAt0WithOtherEvents()
        {
            MTrk track = new MTrk();
            track.AddEvent(new MidiEvent(0, 0x80, 0, 0));
            track.AddEvent(new MidiMetaEvent(0, 0x51, 0, 0, 24));
            track.AddEvent(new MidiEvent(1, 0x80, 0, 0));
            List<TimeInfo> info = track.ExtractTimeInfo(24);
            Assert.AreEqual(1, info.Count);
            Assert.AreEqual(0, info[0].TickNumber);
            Assert.AreEqual(24 / 24, info[0].MicroSecondsPerTick);
        }

        [Test]
        public void TestExtractTimeInfoTempoAt1()
        {
            MTrk track = new MTrk();
            track.AddEvent(new MidiMetaEvent(1, 0x51, 0, 0, 24));
            List<TimeInfo> info = track.ExtractTimeInfo(24);
            Assert.AreEqual(2, info.Count);
            Assert.AreEqual(0, info[0].TickNumber);
            Assert.AreEqual(500000 / 24, info[0].MicroSecondsPerTick);
            Assert.AreEqual(1, info[1].TickNumber);
            Assert.AreEqual(24 / 24, info[1].MicroSecondsPerTick);
        }

        [Test]
        public void TestExtractTimeInfoMultipleTempos()
        {
            MTrk track = new MTrk();
            track.AddEvent(new MidiMetaEvent(1, 0x51, 0, 0, 24));
            track.AddEvent(new MidiMetaEvent(2, 0x51, 0, 0, 48));
            List<TimeInfo> info = track.ExtractTimeInfo(24);
            Assert.AreEqual(3, info.Count);
            Assert.AreEqual(0, info[0].TickNumber);
            Assert.AreEqual(500000 / 24, info[0].MicroSecondsPerTick);
            Assert.AreEqual(1, info[1].TickNumber);
            Assert.AreEqual(24 / 24, info[1].MicroSecondsPerTick);
            Assert.AreEqual(2, info[2].TickNumber);
            Assert.AreEqual(48 / 24, info[2].MicroSecondsPerTick);
        }

        [Test]
        public void TestCalcRealTimeOneTempoEventAt0()
        {
            MTrk track = new MTrk();
            track.AddEvent(new MidiEvent(0, 0x80, 0, 0));
            List<TimeInfo> info = new List<TimeInfo>();
            info.Add(new TimeInfo(0, 1000));
            track.CalcRealTime(info);
            Assert.AreEqual(0.0f, track.GetEvent(0).RealTime);
        }

        [Test]
        public void TestCalcRealTimeOneTempoEventAt10()
        {
            MTrk track = new MTrk();
            track.AddEvent(new MidiEvent(10, 0x80, 0, 0));
            List<TimeInfo> info = new List<TimeInfo>();
            info.Add(new TimeInfo(0, 1000));
            track.CalcRealTime(info);
            Assert.AreEqual(0.01f, track.GetEvent(0).RealTime);
        }

        [Test]
        public void TestCalcRealTimeOneTempoTwoEvents()
        {
            MTrk track = new MTrk();
            track.AddEvent(new MidiEvent(0, 0x80, 0, 0));
            track.AddEvent(new MidiEvent(10, 0x80, 0, 0));
            List<TimeInfo> info = new List<TimeInfo>();
            info.Add(new TimeInfo(0, 1000));
            track.CalcRealTime(info);
            Assert.AreEqual(0.0f, track.GetEvent(0).RealTime);
            Assert.AreEqual(0.01f, track.GetEvent(1).RealTime);
        }

        [Test]
        public void TestCalcRealTimeThreeTemposOneEvent()
        {
            MTrk track = new MTrk();
            track.AddEvent(new MidiEvent(3, 0x80, 0, 0));
            List<TimeInfo> info = new List<TimeInfo>();
            info.Add(new TimeInfo(0, 1000));
            info.Add(new TimeInfo(1, 2000));
            info.Add(new TimeInfo(2, 3000));
            track.CalcRealTime(info);
            Assert.AreEqual(0.006f, track.GetEvent(0).RealTime);
        }

        [Test]
        public void TestCalcRealTimeTwoTemposTwoEvents()
        {
            MTrk track = new MTrk();
            track.AddEvent(new MidiEvent(5, 0x80, 0, 0));
            track.AddEvent(new MidiEvent(10, 0x80, 0, 0));
            List<TimeInfo> info = new List<TimeInfo>();
            info.Add(new TimeInfo(0, 1000));
            info.Add(new TimeInfo(5, 2000));
            track.CalcRealTime(info);
            Assert.AreEqual(0.005f, track.GetEvent(0).RealTime);
            Assert.AreEqual(0.015f, track.GetEvent(1).RealTime);
        }

        #region IEventParser Members

        public MidiEvent ParseEvent(BinaryReader reader, int time, byte lastEventType, out int readBytes)
        {
            Assert.AreEqual(this.reader, reader);
            Assert.AreEqual(expectedTime, time);
            Assert.AreEqual(expectedLastEvent, lastEventType);
            expectedLastEvent = midiEvent.EventType;
            expectedTime += midiEvent.Time;
            readBytes = bytesPerRead;
            return midiEvent;
        }

        #endregion
    }
}
