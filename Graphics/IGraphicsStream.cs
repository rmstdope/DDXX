using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public interface IGraphicsStream : IDisposable
    {
        // Summary:
        //     Retrieves a value that indicates whether the current stream supports reading.
        bool CanRead { get; }
        //
        // Summary:
        //     Retrieves a value that indicates whether the current stream supports seeking.
        bool CanSeek { get; }
        //
        // Summary:
        //     Retrieves a value that indicates whether the current stream supports writing.
        bool CanWrite { get; }
        //
        // Summary:
        //     Retrieves the length of the stream in bytes.
        long Length { get; }
        //
        // Summary:
        //     Retrieves or sets the position within the current stream.
        long Position { get; set; }
        // Summary:
        //     Closes the current stream and releases any resources associated with it.
        void Close();
        //
        // Summary:
        //     Reads from the current stream and advances the position within it by the
        //     number of bytes read.
        //
        // Parameters:
        //   unicode:
        //     Set to true if the graphics stream is Unicode. Set to false if the graphics
        //     stream is ASCII.
        //
        // Returns:
        //     A System.String that contains the data to read from the stream buffer.
        string Read(bool unicode);
        //
        // Summary:
        //     Reads from the current stream and advances the position within it by the
        //     number of bytes read.
        //
        // Parameters:
        //   returnType:
        //     Value that indicates the System.Type of the returned array.
        //
        // Returns:
        //     A System.ValueType of type Microsoft.DirectX.GraphicsStream.Read() that contains
        //     the data read from the stream buffer.
        ValueType Read(Type returnType);
        //
        // Summary:
        //     Reads from the current stream and advances the position within it by the
        //     number of bytes read.
        //
        // Parameters:
        //   returnType:
        //     Value that indicates the System.Type of the returned array.
        //
        //   ranks:
        //     An array of System.Int32 values that represent the size of each dimension
        //     of the returning array.
        //
        // Returns:
        //     An System.Array of type Microsoft.DirectX.GraphicsStream.Read() that contains
        //     the data read from the stream buffer.
        Array Read(Type returnType, params int[] ranks);
        //
        // Summary:
        //     Reads a sequence of bytes from the current stream and advances the position
        //     within the stream by the number of bytes read.
        //
        // Parameters:
        //   buffer:
        //     Reference to a System.Byte array that is used as the read buffer.
        //
        //   offset:
        //     Value that specifies the offset, from the start of the buffer, of the data
        //     read from Microsoft.DirectX.GraphicsStream.Read().
        //
        //   count:
        //     Value that specifies the number of bytes to read.
        //
        // Returns:
        //     Integer that represents the number of bytes read into Microsoft.DirectX.GraphicsStream.Read()
        //     from the graphics stream.
        int Read(byte[] buffer, int offset, int count);
        //
        // Summary:
        //     Sets the position within the current stream.
        //
        // Parameters:
        //   newposition:
        //     Value that represents the new position within the stream buffer.
        //
        //   origin:
        //     Member of the System.IO.SeekOrigin enumeration that specifies where to begin
        //     seeking.
        //
        // Returns:
        //     New position within the stream buffer.
        long Seek(long newposition, SeekOrigin origin);
        //
        // Summary:
        //     Resizing the graphics stream is not supported.
        //
        // Parameters:
        //   newLength:
        void SetLength(long newLength);
        //
        // Summary:
        //     Writes to the current stream and advances the current position within it
        //     by the number of bytes written.
        //
        // Parameters:
        //   value:
        //     Reference to an System.Array that contains the data to write into the stream
        //     buffer.
        void Write(Array value);
        //
        // Summary:
        //     Writes to the current stream and advances the current position within it
        //     by the number of bytes written.
        //
        // Parameters:
        //   value:
        //     Reference to a System.String that contains the data to write into the stream
        //     buffer.
        void Write(string value);
        //
        // Summary:
        //     Writes to the current stream and advances the current position within it
        //     by the number of bytes written.
        //
        // Parameters:
        //   value:
        //     Value that specifies the System.ValueType of the data to write into the stream
        //     buffer.
        void Write(ValueType value);
        //
        // Summary:
        //     Writes to the current stream and advances the current position within it
        //     by the number of bytes written.
        //
        // Parameters:
        //   value:
        //     Reference to a System.String that contains the data to write into the stream
        //     buffer.
        //
        //   isUnicodeString:
        //     Set to true if the graphics stream is Unicode. Set to false if the graphics
        //     stream is ASCII.
        void Write(string value, bool isUnicodeString);
        //
        // Summary:
        //     Writes to the current stream and advances the current position within it
        //     by the number of bytes written.
        //
        // Parameters:
        //   buffer:
        //     Reference to a System.Byte array that is used as the write buffer.
        //
        //   offset:
        //     Value that specifies the offset of the data to write from Microsoft.DirectX.GraphicsStream.Write().
        //
        //   count:
        //     Value that specifies the number of bytes to write.
        void Write(byte[] buffer, int offset, int count);
    }
}
