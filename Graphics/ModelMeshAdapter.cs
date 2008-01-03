using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public class ModelMeshAdapter : IModelMesh
    {
        private ModelMesh modelMesh;

        public ModelMeshAdapter(ModelMesh modelMesh)
        {
            this.modelMesh = modelMesh;
        }

        public ModelMesh DxModelMesh { get { return modelMesh; } }

        #region IModelMesh Members

        public BoundingSphere BoundingSphere
        {
            get { return modelMesh.BoundingSphere; }
        }

        public ModelEffectCollectionAdapter Effects
        {
            get { return new ModelEffectCollectionAdapter(modelMesh.Effects); }
        }

        public IIndexBuffer IndexBuffer
        {
            get { return new IndexBufferAdapter(modelMesh.IndexBuffer); }
        }

        public ReadOnlyCollection<IModelMeshPart> MeshParts
        {
            get 
            {
                List<IModelMeshPart> list = new List<IModelMeshPart>();
                foreach (ModelMeshPart part in modelMesh.MeshParts)
                    list.Add(new ModelMeshPartAdapter(part));
                return new ReadOnlyCollection<IModelMeshPart>(list);
            }
        }

        public string Name
        {
            get { return modelMesh.Name; }
        }

        public ModelBone ParentBone
        {
            get { return modelMesh.ParentBone; }
        }

        public object Tag
        {
            get
            {
                return modelMesh.Tag;
            }
            set
            {
                modelMesh.Tag = value;
            }
        }

        public IVertexBuffer VertexBuffer
        {
            get { return new VertexBufferAdapter(modelMesh.VertexBuffer); }
        }

        public void Draw()
        {
            modelMesh.Draw();
        }

        public void Draw(SaveStateMode saveStateMode)
        {
            modelMesh.Draw(saveStateMode);
        }

        #endregion
    }
}
