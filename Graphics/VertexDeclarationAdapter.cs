using System;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class VertexDeclarationAdapter : IVertexDeclaration
    {
        private VertexDeclaration vertexDeclaration;

        public VertexDeclarationAdapter(VertexDeclaration vertexDeclaration)
        {
            this.vertexDeclaration = vertexDeclaration;
        }

        public VertexDeclaration DxVertexDeclaration { get { return vertexDeclaration; } }

        #region IVertexDeclaration Members

        public IGraphicsDevice GraphicsDevice
        {
            get { return new GraphicsDeviceAdapter(vertexDeclaration.GraphicsDevice); }
        }

        public bool IsDisposed
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public string Name
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public object Tag
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public VertexElement[] GetVertexElements()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetVertexStrideSize(int stream)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
