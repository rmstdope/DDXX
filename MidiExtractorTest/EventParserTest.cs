using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace Dope.DDXX.MidiExtractor
{
    [TestFixture]
    public class EventParserTest
    {
        private IEventParser parser;

        [SetUp]
        public void SetUp()
        {
            parser = new EventParser();
        }

        [Test]
        public void TestParseTimeLastZeroDeltaZero()
        {
            MidiEvent e = CreateEventWithLength(new byte[] { 0 }, 0);
            Assert.AreEqual(0, e.Time);
        }

        [Test]
        public void TestParseTimeLastTenDeltaZero()
        {
            MidiEvent e = CreateEventWithLength(new byte[] { 0 }, 10);
            Assert.AreEqual(10, e.Time);
        }

        [Test]
        public void TestParseTimeLastZeroDelta0x7F()
        {
            MidiEvent e = CreateEventWithLength(new byte[] { 0x7F }, 0);
            Assert.AreEqual(0x7F, e.Time);
        }

        [Test]
        public void TestParseTimeLast0x10Delta0x40()
        {
            MidiEvent e = CreateEventWithLength(new byte[] { 0x40 }, 0x10);
            Assert.AreEqual(0x40 + 0x10, e.Time);
        }

        [Test]
        public void TestParseTimeLength8100()
        {
            MidiEvent e = CreateEventWithLength(new byte[] { 0x81, 0x00 }, 0);
            Assert.AreEqual(0x80, e.Time);
        }

        [Test]
        public void TestParseTimeLengthC000()
        {
            MidiEvent e = CreateEventWithLength(new byte[] { 0xC0, 0x00 }, 0);
            Assert.AreEqual(0x2000, e.Time);
        }

        [Test]
        public void TestParseTimeLengthFF7F()
        {
            MidiEvent e = CreateEventWithLength(new byte[] { 0xFF, 0x7F }, 0);
            Assert.AreEqual(0x3FFF, e.Time);
        }

        [Test]
        public void TestParseTimeLength818000()
        {
            MidiEvent e = CreateEventWithLength(new byte[] { 0x81, 0x80, 0x00 }, 0);
            Assert.AreEqual(0x4000, e.Time);
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestParseEventType0x7F()
        {
            MidiEvent e = CreateEventWithData(new byte[] { 0x7F });
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestParseEventType0xF0()
        {
            MidiEvent e = CreateEventWithData(new byte[] { 0xF0 });
        }

        [Test]
        public void TestParseEventType0x80To0xBF()
        {
            for (byte i = 0x80; i < 0xC0; i++)
            {
                MidiEvent e = CreateEventWithData(new byte[] { i, (byte)(i + 1), (byte)(i + 2) });
                Assert.AreEqual(i, e.EventType);
                Assert.AreEqual((byte)(i + 1), e.Arg1);
                Assert.AreEqual((byte)(i + 2), e.Arg2);
            }
        }

        [Test]
        public void TestParseEventType0xC0To0xDF()
        {
            for (byte i = 0xC0; i < 0xE0; i++)
            {
                MidiEvent e = CreateEventWithData(new byte[] { i, (byte)(i + 1) });
                Assert.AreEqual(i, e.EventType);
                Assert.AreEqual((byte)(i + 1), e.Arg1);
                Assert.AreEqual(0, e.Arg2);
            }
        }

        [Test]
        public void TestParseEventType0xE0To0xEF()
        {
            for (byte i = 0xE0; i < 0xF0; i++)
            {
                MidiEvent e = CreateEventWithData(new byte[] { i, (byte)(i + 1), (byte)(i + 2) });
                Assert.AreEqual(i, e.EventType);
                Assert.AreEqual((byte)(i + 1), e.Arg1);
                Assert.AreEqual((byte)(i + 2), e.Arg2);
            }
        }

        [Test]
        public void TestParseMetaEvent3()
        {
            MidiEvent e = CreateEventWithData(new byte[] { 0xFF, 3, 1, 0x4D });
            Assert.IsInstanceOfType(typeof(MidiMetaEvent), e);
            MidiMetaEvent me = e as MidiMetaEvent;
            Assert.AreEqual(3, me.MetaEventType);
            Assert.AreEqual(new byte[] { 0x4D }, me.MetaData);
        }

        [Test]
        public void TestParseMetaEvent4()
        {
            MidiEvent e = CreateEventWithData(new byte[] { 0xFF, 4, 4, 1, 2, 3, 4 });
            Assert.IsInstanceOfType(typeof(MidiMetaEvent), e);
            MidiMetaEvent me = e as MidiMetaEvent;
            Assert.AreEqual(4, me.MetaEventType);
            Assert.AreEqual(new byte[] { 1, 2, 3, 4 }, me.MetaData);
        }

        [Test]
        public void TestRunningStatus0x80()
        {
            MidiEvent e = CreateEventWithData(new byte[] { 1, 2 }, 0x80);
            Assert.AreEqual(0x80, e.EventType);
            Assert.AreEqual(1, e.Arg1);
            Assert.AreEqual(2, e.Arg2);
        }

        [Test]
        public void TestRunningStatus0xC0()
        {
            MidiEvent e = CreateEventWithData(new byte[] { 3 }, 0xC0);
            Assert.AreEqual(0xC0, e.EventType);
            Assert.AreEqual(3, e.Arg1);
            Assert.AreEqual(0, e.Arg2);
        }

        private MidiEvent CreateEventWithData(byte[] eventData)
        {
            return CreateEventWithData(eventData, 0);
        }

        private MidiEvent CreateEventWithData(byte[] eventData, byte lastEventType)
        {
            int readBytes;
            byte[] data = new byte[eventData.Length + 1];
            eventData.CopyTo(data, 1);
            data[0] = 0;
            MemoryStream stream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(stream);
            MidiEvent e = parser.ParseEvent(reader, 0, lastEventType, out readBytes);
            Assert.AreEqual(-1, reader.PeekChar());
            Assert.AreEqual(data.Length, readBytes);
            return e;
        }

        private MidiEvent CreateEventWithLength(byte[] lengthData, int deltaTime)
        {
            int readBytes;
            byte[] data = new byte[lengthData.Length + 3];
            lengthData.CopyTo(data, 0);
            data[lengthData.Length + 0] = 0x90;
            data[lengthData.Length + 1] = 0x80;
            data[lengthData.Length + 2] = 10;
            MemoryStream stream = new MemoryStream(data);
            BinaryReader reader = new BinaryReader(stream);
            MidiEvent e = parser.ParseEvent(reader, deltaTime, 0, out readBytes);
            Assert.AreEqual(-1, reader.PeekChar());
            Assert.AreEqual(data.Length, readBytes);
            return e;
        }

    }
}
