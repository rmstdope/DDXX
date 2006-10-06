using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public class GraphicsStreamAdapter : IGraphicsStream
    {
        private GraphicsStream graphicsStream;

        public GraphicsStreamAdapter(GraphicsStream graphicsStream)
        {
            this.graphicsStream = graphicsStream;
        }

        internal GraphicsStream DXGraphicsStream
        { 
            get { return graphicsStream; } 
        }

        #region IGraphicsStream Members

        public bool CanRead
        {
            get { return graphicsStream.CanRead; }
        }

        public bool CanSeek
        {
            get { return graphicsStream.CanSeek; }
        }

        public bool CanWrite
        {
            get { return graphicsStream.CanWrite; }
        }

        public long Length
        {
            get { return graphicsStream.Length; }
        }

        public long Position
        {
            get
            {
                return graphicsStream.Position;
            }
            set
            {
                graphicsStream.Position = value;
            }
        }

        public void Close()
        {
            graphicsStream.Close();
        }

        public void Dispose()
        {
            graphicsStream.Dispose();
        }

        public string Read(bool unicode)
        {
            return graphicsStream.Read(unicode);
        }

        public ValueType Read(Type returnType)
        {
            return graphicsStream.Read(returnType);
        }

        public Array Read(Type returnType, params int[] ranks)
        {
            return graphicsStream.Read(returnType, ranks);
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return graphicsStream.Read(buffer, offset, count);
        }

        public long Seek(long newposition, SeekOrigin origin)
        {
            return graphicsStream.Seek(newposition, origin);
        }

        public void SetLength(long newLength)
        {
            graphicsStream.SetLength(newLength);
        }

        public void Write(Array value)
        {
            graphicsStream.Write(value);
        }

        public void Write(string value)
        {
            graphicsStream.Write(value);
        }

        public void Write(ValueType value)
        {
            graphicsStream.Write(value);
        }

        public void Write(string value, bool isUnicodeString)
        {
            graphicsStream.Write(value, isUnicodeString);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            graphicsStream.Write(buffer, offset, count);
        }

        #endregion
    }
}
