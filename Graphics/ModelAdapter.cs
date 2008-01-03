using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Animation;

namespace Dope.DDXX.Graphics
{
    public class ModelAdapter : IModel
    {
        private Model model;

        public ModelAdapter(Model model)
        {
            this.model = model;
        }

        #region IModel Members

        public ModelBoneCollection Bones
        {
            get { return model.Bones; }
        }

        public ReadOnlyCollection<IModelMesh> Meshes
        {
            get 
            {
                List<IModelMesh> list = new List<IModelMesh>();
                foreach (ModelMesh mesh in model.Meshes)
                    list.Add(new ModelMeshAdapter(mesh));
                return new ReadOnlyCollection<IModelMesh>(list);
            }
        }

        public ModelBone Root
        {
            get { return model.Root; }
        }

        public object Tag
        {
            get
            {
                return model.Tag;
            }
            set
            {
                model.Tag = value;
            }
        }

        public void CopyAbsoluteBoneTransformsTo(Matrix[] destinationBoneTransforms)
        {
            model.CopyAbsoluteBoneTransformsTo(destinationBoneTransforms);
        }

        public void CopyBoneTransformsFrom(Matrix[] sourceBoneTransforms)
        {
            model.CopyBoneTransformsFrom(sourceBoneTransforms);
        }

        public void CopyBoneTransformsTo(Matrix[] destinationBoneTransforms)
        {
            model.CopyBoneTransformsTo(destinationBoneTransforms);
        }

        public IAnimationController AnimationController
        {
            get { return Tag as IAnimationController; }
        }

        #endregion
    }
}
