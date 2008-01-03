using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface IModelMesh
    {
        // Summary:
        //     Gets the Framework.BoundingSphere that contains this mesh.
        //
        // Returns:
        //     The Framework.BoundingSphere that contains this mesh.
        BoundingSphere BoundingSphere { get; }
        //
        // Summary:
        //     Gets a collection of effects associated with this mesh.
        //
        // Returns:
        //     A collection of effects associated with this mesh.
        ModelEffectCollectionAdapter Effects { get; }
        //
        // Summary:
        //     Gets the index buffer for this mesh.
        //
        // Returns:
        //     The index buffer for this mesh.
        IIndexBuffer IndexBuffer { get; }
        //
        // Summary:
        //     Gets the ModelMeshPart objects that make up this mesh. Each part of a mesh
        //     is composed of a set of primitives that share the same material.
        //
        // Returns:
        //     The ModelMeshPart objects that make up this mesh.
        ReadOnlyCollection<IModelMeshPart> MeshParts { get; }
        //
        // Summary:
        //     Gets the name of this mesh.
        //
        // Returns:
        //     The name of this mesh.
        string Name { get; }
        //
        // Summary:
        //     Gets the parent bone for this mesh. The parent bone of a mesh contains a
        //     transformation matrix that describes how the mesh is located relative to
        //     any parent meshes in a model.
        //
        // Returns:
        //     The parent bone for this mesh.
        ModelBone ParentBone { get; }
        //
        // Summary:
        //     Gets or sets an object identifying this mesh.
        //
        // Returns:
        //     An object identifying this mesh.
        object Tag { get; set; }
        //
        // Summary:
        //     Gets the vertex buffer used to render this mesh.
        //
        // Returns:
        //     The vertex buffer used to render this mesh.
        IVertexBuffer VertexBuffer { get; }
        // Summary:
        //     Draws all the ModelMeshPart objects in this mesh, using their current ModelMeshPart.Effect
        //     settings.
        void Draw();
        //
        // Summary:
        //     Draws all the ModelMeshPart objects in this mesh, using their current ModelMeshPart.Effect
        //     settings, and specifying options for saving effect state.
        //
        // Parameters:
        //   saveStateMode:
        //     The save state options to pass to each ModelMeshPart.Effect.
        void Draw(SaveStateMode saveStateMode);
    }
}
