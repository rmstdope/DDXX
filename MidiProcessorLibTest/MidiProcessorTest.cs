using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.MidiProcessorLib;
using Dope.DDXX.MidiExtractor;
using System.IO;

namespace Dope.DDXX.MidiProcessorLibTest
{
    [TestFixture]
    public class MidiProcessorTest
    {
        [Test]
        public void TestProcess()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x68, 0x64, // MThd ID
                0x00, 0x00, 0x00, 0x06, // Length of the MThd chunk is always 6.
                0x00, 0x01,             // Format
                0x00, 0x03,             // NumTracks
                0xE7, 0x28,             // Division
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 0x00,  // Length of the MTrk chunk
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 0x00,  // Length of the MTrk chunk
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 0x00,  // Length of the MTrk chunk
            };
            MidiExtractor.MidiExtractor extractor = new MidiExtractor.MidiExtractor();
            MemoryStream stream = new MemoryStream(data);
            MThd header;
            List<IMTrk> tracks;
            extractor.Parse(stream, null, out header, out tracks);
            MidiSource midiSource = new MidiSource(header, tracks);
            MidiProcessor processor = new MidiProcessor();
            CompiledMidi compiledMidi = processor.Process(midiSource);
            Assert.AreEqual(3, compiledMidi.Tracks.Count);
        }
    }
}
