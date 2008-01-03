using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace Dope.DDXX.MidiExtractor
{
    [TestFixture]
    public class MidiExtractorTest
    {
        private MidiExtractor extractor;
        private MThd header;
        private List<IMTrk> tracks;

        [SetUp]
        public void SetUp()
        {
            extractor = new MidiExtractor();
        }

        [Test]
        public void TestHeaderNoTracks()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x68, 0x64, // MThd ID
                0x00, 0x00, 0x00, 0x06, // Length of the MThd chunk is always 6.
                0x00, 0x01,             // Format
                0x00, 0x00,             // NumTracks
                0xE7, 0x28,             // Division
            };
            MemoryStream stream = new MemoryStream(data);
            extractor.Parse(stream, null, out header, out tracks);
            Assert.AreEqual(0, tracks.Count);
        }

        [Test]
        public void TestHeaderOneTrack()
        {
            byte[] data = new byte[] { 
                0x4D, 0x54, 0x68, 0x64, // MThd ID
                0x00, 0x00, 0x00, 0x06, // Length of the MThd chunk is always 6.
                0x00, 0x01,             // Format
                0x00, 0x01,             // NumTracks
                0xE7, 0x28,             // Division
                0x4D, 0x54, 0x72, 0x6B, // MTrk ID
                0x00, 0x00, 0x00, 0x00  // Length of the MTrk chunk
            };
            MemoryStream stream = new MemoryStream(data);
            extractor.Parse(stream, null, out header, out tracks);
            Assert.AreEqual(1, tracks.Count);
        }

        [Test]
        public void TestHeaderThreeTracks()
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
            MemoryStream stream = new MemoryStream(data);
            extractor.Parse(stream, null, out header, out tracks);
            Assert.AreEqual(3, tracks.Count);
        }

    }
}
