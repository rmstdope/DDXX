using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class VertexStreamAdapter : IVertexStream
    {
        private VertexStream vertexStream;

        public VertexStreamAdapter(VertexStream vertexStream)
        {
            this.vertexStream = vertexStream;
        }

        #region IVertexStream Members

        public int OffsetInBytes
        {
            get { return vertexStream.OffsetInBytes; }
        }

        public IVertexBuffer VertexBuffer
        {
            get { return new VertexBufferAdapter(vertexStream.VertexBuffer); }
        }

        public int VertexStride
        {
            get { return vertexStream.VertexStride; }
        }

#if (!XBOX)
        public void SetFrequency(int frequency)
        {
            vertexStream.SetFrequency(frequency);
        }

        public void SetFrequencyOfIndexData(int frequency)
        {
            vertexStream.SetFrequencyOfIndexData(frequency);
        }

        public void SetFrequencyOfInstanceData(int frequency)
        {
            vertexStream.SetFrequencyOfInstanceData(frequency);
        }
#endif

        public void SetSource(IVertexBuffer vb, int offsetInBytes, int vertexStride)
        {
            vertexStream.SetSource((vb as VertexBufferAdapter).DxVertexBuffer, offsetInBytes, vertexStride);
        }

        #endregion
    }
}
