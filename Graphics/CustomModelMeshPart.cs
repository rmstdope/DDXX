using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class CustomModelMeshPart : IModelMeshPart
    {
        private IEffect effect;
        private int baseVertex;
        private int numVertices;
        private int startIndex;
        private int primitiveCount;
        private IMaterialHandler materialHandler;

        public CustomModelMeshPart(IEffect effect, int baseVertex, int numVertices, int startIndex, int primitiveCount)
        {
            this.effect = effect;
            this.baseVertex = baseVertex;
            this.numVertices = numVertices;
            this.startIndex = startIndex;
            this.primitiveCount = primitiveCount;
            this.materialHandler = new MaterialHandler(effect, new EffectConverter());
        }

        #region IModelMeshPart Members

        public int BaseVertex
        {
            get { return baseVertex; }
        }

        public IEffect Effect
        {
            get
            {
                return effect;
            }
            set
            {
                effect = value;
            }
        }

        public int NumVertices
        {
            get { return numVertices; }
        }

        public int PrimitiveCount
        {
            get { return primitiveCount; }
        }

        public int StartIndex
        {
            get { return startIndex; }
        }

        public int StreamOffset
        {
            get { throw new Exception("The method or operation is not implemented."); }
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

        public IVertexDeclaration VertexDeclaration
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int VertexStride
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IMaterialHandler MaterialHandler
        {
            get { return materialHandler; }
        }

        #endregion
    }
}
