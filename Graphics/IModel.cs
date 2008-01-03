using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Animation;

namespace Dope.DDXX.Graphics
{
    public interface IModel
    {
        // Summary:
        //     Gets a collection of ModelBone objects which describe how each mesh in the
        //     Model.Meshes collection for this model relates to its parent mesh.
        //
        // Returns:
        //     A collection of ModelBone objects used by this model.
        ModelBoneCollection Bones { get; }
        //
        // Summary:
        //     Gets a collection of ModelMesh objects which compose the model. Each ModelMesh
        //     in a model may be moved independently and may be composed of multiple materials
        //     identified as ModelMeshPart objects.
        //
        // Returns:
        //     A collection of ModelMesh objects used by this model.
        ReadOnlyCollection<IModelMesh> Meshes { get; }
        //
        // Summary:
        //     Gets the root bone for this model.
        //
        // Returns:
        //     The root bone for this model.
        ModelBone Root { get; }
        //
        // Summary:
        //     Gets or sets an object identifying this model.
        //
        // Returns:
        //     An object identifying this model.
        object Tag { get; set; }

        // Summary:
        //     Copies a transform of each bone in a model relative to all parent bones of
        //     the bone into a given array.
        //
        // Parameters:
        //   destinationBoneTransforms:
        //     The array to receive bone transforms.
        void CopyAbsoluteBoneTransformsTo(Matrix[] destinationBoneTransforms);
        //
        // Summary:
        //     Copies an array of transforms into each bone in the model.
        //
        // Parameters:
        //   sourceBoneTransforms:
        //     An array containing new bone transforms.
        void CopyBoneTransformsFrom(Matrix[] sourceBoneTransforms);
        //
        // Summary:
        //     Copies each bone transform relative only to the parent bone of the model
        //     to a given array.
        //
        // Parameters:
        //   destinationBoneTransforms:
        //     The array to receive bone transforms.
        void CopyBoneTransformsTo(Matrix[] destinationBoneTransforms);

        IAnimationController AnimationController { get; }
    }
}
