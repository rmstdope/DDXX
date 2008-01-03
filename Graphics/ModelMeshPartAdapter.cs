using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class ModelMeshPartAdapter : IModelMeshPart
    {
        private ModelMeshPart part;

        public ModelMeshPartAdapter(ModelMeshPart part)
        {
            this.part = part;
            this.Tag = new MaterialHandler(this.Effect, new EffectConverter());
        }

        public ModelMeshPart DxModelMeshPart { get { return part; } }

        #region IModelMeshPart Members

        public int BaseVertex
        {
            get { return part.BaseVertex; }
        }

        public IEffect Effect
        {
            get
            {
                if (part.Effect is BasicEffect)
                    return new BasicEffectAdapter(part.Effect as BasicEffect);
                return new EffectAdapter(part.Effect);
            }
            set
            {
                part.Effect = (value as EffectAdapter).DxEffect;
                MaterialHandler.Effect = value;
            }
        }

        public int NumVertices
        {
            get { return part.NumVertices; }
        }

        public int PrimitiveCount
        {
            get { return part.PrimitiveCount; }
        }

        public int StartIndex
        {
            get { return part.StartIndex; }
        }

        public int StreamOffset
        {
            get { return part.StreamOffset; }
        }

        public object Tag
        {
            get
            {
                return part.Tag;
            }
            set
            {
                part.Tag = value;
            }
        }

        public IVertexDeclaration VertexDeclaration
        {
            get { return new VertexDeclarationAdapter(part.VertexDeclaration); }
        }

        public int VertexStride
        {
            get { return part.VertexStride; }
        }

        public IMaterialHandler MaterialHandler 
        {
            get { return Tag as IMaterialHandler; }
        }


        #endregion
    }
}
