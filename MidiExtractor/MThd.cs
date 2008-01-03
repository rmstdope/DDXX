using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dope.DDXX.MidiExtractor
{
    public class MThd : MidiChunk
    {
        private int length;
        private int format;
        private int numTracks;
        private int division;

        public MThd(BinaryReader reader)
            : base(reader)
        {
            ReadLength(reader);
            VerifyLength();
            ReadFormat(reader);
            VerifyFormat();
            ReadNumTracks(reader);
            ReadDivision(reader);
            VerifyDivision();
        }

        override protected byte[] ExpectedID
        {
            get { return new byte[] { 0x4D, 0x54, 0x68, 0x64 }; }
        }

        private void VerifyDivision()
        {
        }

        private void ReadDivision(BinaryReader reader)
        {
            division = ReadUint16(reader);
        }

        private void ReadNumTracks(BinaryReader reader)
        {
            numTracks = ReadUint16(reader);
        }

        private void VerifyFormat()
        {
            if (1 != format)
                throw new InvalidDataException("Only format 1 is supported.");
        }

        private void ReadFormat(BinaryReader reader)
        {
            format = ReadUint16(reader);
        }

        //private void VerifyID()
        //{
        //    byte[] correctID = new byte[] { 0x4D, 0x54, 0x68, 0x64 };
        //    for (int i = 0; i < 4; i++)
        //        if (id[i] != correctID[i])
        //            throw new InvalidDataException("Header bytes for MThd must be {0x4D, 0x54, 0x68, 0x64}.");
        //}

        private void VerifyLength()
        {
            if (6 != length)
                throw new InvalidDataException("Length for MThd chunk must be 6.");
        }

        private void ReadLength(BinaryReader reader)
        {
            length = ReadUint32(reader);
        }

        public int NumTracks
        {
            get
            {
                return numTracks;
            }
        }

        public int Division
        {
            get
            {
                return division;
            }
        }
    }
}
