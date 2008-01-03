using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Dope.DDXX.MidiExtractor
{
    public abstract class MidiChunk
    {
        private byte[] id;

        abstract protected byte[] ExpectedID { get; }

        protected MidiChunk()
        {
        }

        protected MidiChunk(BinaryReader reader)
        {
            ReadID(reader);
            VerifyID();
        }

        protected static int ReadUint16(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(2);
            return (bytes[0] << 8) + bytes[1];
        }

        protected static int ReadUint32(BinaryReader reader)
        {
            byte[] bytes = reader.ReadBytes(4);
            return (bytes[0] << 24) + (bytes[1] << 16) + (bytes[2] << 8) + bytes[3];
        }

        protected static int BytesToUint24(byte[] bytes)
        {
            return (bytes[0] << 16) + (bytes[1] << 8) + bytes[2];
        }

        private void ReadID(BinaryReader reader)
        {
            id = reader.ReadBytes(4);
        }

        private void VerifyID()
        {
            for (int i = 0; i < 4; i++)
                if (id[i] != ExpectedID[i])
                    throw new InvalidDataException("Header bytes is incorrect for chunk.");
        }

    }
}
