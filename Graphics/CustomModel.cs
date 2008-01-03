using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Animation;

namespace Dope.DDXX.Graphics
{
    public class CustomModel : IModel
    {
        private ReadOnlyCollection<IModelMesh> meshes;

        public CustomModel(IModelMesh mesh)
        {
            List<IModelMesh> list = new List<IModelMesh>();
            list.Add(mesh);
            meshes = new ReadOnlyCollection<IModelMesh>(list);
        }

        public ModelBoneCollection Bones
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ReadOnlyCollection<IModelMesh> Meshes
        {
            get { return meshes; }
        }

        public ModelBone Root
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

        public void CopyAbsoluteBoneTransformsTo(Matrix[] destinationBoneTransforms)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyBoneTransformsFrom(Matrix[] sourceBoneTransforms)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyBoneTransformsTo(Matrix[] destinationBoneTransforms)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IAnimationController AnimationController
        {
            get
            {
                return null;
            }
        }

    }
}
