using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Animation;

namespace Dope.DDXX.Graphics
{
    public class CustomModel
    {
        private IAnimationController animationController;
        private ReadOnlyCollection<CustomModelMesh> meshes;

        public CustomModel(CustomModelMesh mesh)
        {
            List<CustomModelMesh> list = new List<CustomModelMesh>(1);
            list.Add(mesh);
            meshes = new ReadOnlyCollection<CustomModelMesh>(list);
            this.animationController = null;
        }

        public CustomModel(CustomModelMesh mesh, IAnimationController animationController)
        {
            List<CustomModelMesh> list = new List<CustomModelMesh>(1);
            list.Add(mesh);
            meshes = new ReadOnlyCollection<CustomModelMesh>(list);
            this.animationController = animationController;
        }

        public CustomModel(CustomModelMesh[] meshArray)
        {
            List<CustomModelMesh> list = new List<CustomModelMesh>(meshArray);
            meshes = new ReadOnlyCollection<CustomModelMesh>(list);
            this.animationController = null;
        }

        public CustomModel(CustomModelMesh[] meshArray, IAnimationController animationController)
        {
            List<CustomModelMesh> list = new List<CustomModelMesh>(meshArray);
            meshes = new ReadOnlyCollection<CustomModelMesh>(list);
            this.animationController = animationController;
        }

        public IAnimationController AnimationController { get { return animationController; } }
        //public ModelBoneCollection Bones { get { return baseModel.Bones; } }
        public ReadOnlyCollection<CustomModelMesh> Meshes { get { return meshes; } }
        //public ModelBone Root { get { return baseModel.Root; } }
        //public object Tag { get { return baseModel.Tag; } set { baseModel.Tag = value; } }
        //public void CopyAbsoluteBoneTransformsTo(Matrix[] destinationBoneTransforms)
        //{
        //    baseModel.CopyAbsoluteBoneTransformsTo(destinationBoneTransforms);
        //}
        //public void CopyBoneTransformsFrom(Matrix[] sourceBoneTransforms)
        //{
        //    baseModel.CopyBoneTransformsFrom(sourceBoneTransforms);
        //}
        //public void CopyBoneTransformsTo(Matrix[] destinationBoneTransforms)
        //{
        //    baseModel.CopyBoneTransformsTo(destinationBoneTransforms);
        //}
        public void Draw(Matrix world, Matrix view, Matrix projection)
        {
            /// TODO
        }
    }
}
