using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace Dope.DDXX.MidiProcessorLib
{
    [TestFixture]
    public class MidiImporterTest
    {
        public MidiImporter importer;
        byte[] threeTrackData = new byte[] { 
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

        [SetUp]
        public void SetUp()
        {
            importer = new MidiImporter();
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
            MidiSource midi = importer.ImportFromStream(stream);
            Assert.AreEqual(0, midi.Tracks.Count);
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
            MidiSource midi = importer.ImportFromStream(stream);
            Assert.AreEqual(1, midi.Tracks.Count);
        }

        [Test]
        public void TestHeaderThreeTracks()
        {
            MemoryStream stream = new MemoryStream(threeTrackData);
            MidiSource midi = importer.ImportFromStream(stream);
            Assert.AreEqual(3, midi.Tracks.Count);
        }

        [Test]
        public void TestHeader()
        {
            MemoryStream stream = new MemoryStream(threeTrackData);
            MidiSource midi = importer.ImportFromStream(stream);
            Assert.AreEqual(3, midi.Header.NumTracks);
            int expectedDivision = (threeTrackData[12] << 8) + threeTrackData[13];
            Assert.AreEqual(expectedDivision, midi.Header.Division);
        }
    }
}
