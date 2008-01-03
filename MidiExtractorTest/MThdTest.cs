using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.IO;

namespace Dope.DDXX.MidiExtractor
{
    [TestFixture]
    public class MThdTest
    {
        private byte[] standardHeader;

        [SetUp]
        public void SetUp()
        {
            standardHeader = new byte[] { 
                0x4D, 0x54, 0x68, 0x64, // MThd ID
                0x00, 0x00, 0x00, 0x06, // Length of the MThd chunk
                0x00, 0x01,             // Format
                0x00, 0x01,             // NumTracks
                0x00, 0x60,             // Division
            };
        }

        private MThd CreateHeader()
        {
            MemoryStream stream = new MemoryStream(standardHeader);
            BinaryReader reader = new BinaryReader(stream);
            MThd header = new MThd(reader);
            return header;
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestInvalidID1()
        {
            standardHeader[0] = 0;
            CreateHeader();
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestInvalidID2()
        {
            standardHeader[1] = 0;
            CreateHeader();
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestInvalidID3()
        {
            standardHeader[2] = 0;
            CreateHeader();
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestInvalidID4()
        {
            standardHeader[3] = 0;
            CreateHeader();
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestInvalidLength()
        {
            standardHeader[7] = 0;
            CreateHeader();
        }

        [Test]
        [ExpectedException(typeof(InvalidDataException))]
        public void TestInvalidFormat()
        {
            standardHeader[9] = 0;
            CreateHeader();
        }

        [Test]
        public void TestOneTrack()
        {
            MThd header = CreateHeader();
            Assert.AreEqual(1, header.NumTracks);
        }

        [Test]
        public void TestTwoTracks()
        {
            standardHeader[11] = 2;
            MThd header = CreateHeader();
            Assert.AreEqual(2, header.NumTracks);
        }

        [Test]
        public void Test96Ppqn()
        {
            MThd header = CreateHeader();
            Assert.AreEqual(96, header.Division);
        }

        [Test]
        public void Test97Ppqn()
        {
            standardHeader[13] = 0x61;
            MThd header = CreateHeader();
            Assert.AreEqual(97, header.Division);
        }
    }
}

// If positive:
// PPQN
// If negative:
// 