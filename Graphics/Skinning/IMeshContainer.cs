using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public interface IMeshContainer
    {
        // Summary:
        //     Retrieves or sets a Microsoft.DirectX.Direct3D.MeshData object that contains
        //     the type of data in the mesh.
        MeshDataAdapter MeshData { get; set; }
        //
        // Summary:
        //     Retrieves or sets the mesh name.
        string Name { get; set; }
        //
        // Summary:
        //     Retrieves the next mesh container.
        IMeshContainer NextContainer { get; }
        //
        // Summary:
        //     Retrieves or sets a Microsoft.DirectX.Direct3D.SkinInformation object that
        //     contains information about animating bones in a skin mesh.
        ISkinInformation SkinInformation { get; set; }

        // Summary:
        //     Retrieves an array that contains the mesh adjacency information.
        //
        // Returns:
        //     Array of integers that represent the adjacency information.
        int[] GetAdjacency();
        //
        // Summary:
        //     Retrieves a Microsoft.DirectX.GraphicsStream object that contains the mesh
        //     adjacency information.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that represents the adjacency information.
        IGraphicsStream GetAdjacencyStream();
        //
        // Summary:
        //     Retrieves an array of Microsoft.DirectX.Direct3D.EffectInstance objects that
        //     contain the effects for the mesh.
        //
        // Returns:
        //     Returns an array of Microsoft.DirectX.Direct3D.EffectInstance objects.
        EffectInstance[] GetEffectInstances();
        //
        // Summary:
        //     Retrieves an array of mesh materials.
        //
        // Returns:
        //     Array of Microsoft.DirectX.Direct3D.ExtendedMaterial objects that contain
        //     the mesh materials.
        ExtendedMaterial[] GetMaterials();
        //
        // Summary:
        //     Sets the mesh adjacency information.
        //
        // Parameters:
        //   adj:
        //     A Microsoft.DirectX.GraphicsStream object that represents the adjacency information.
        void SetAdjacency(IGraphicsStream adj);
        //
        // Summary:
        //     Sets the mesh adjacency information.
        //
        // Parameters:
        //   adj:
        //     Array of integers that represent the adjacency information.
        void SetAdjacency(int[] adj);
        //
        // Summary:
        //     Sets an array of Microsoft.DirectX.Direct3D.EffectInstance objects that contain
        //     the effects for the mesh.
        //
        // Parameters:
        //   effects:
        //     An array of Microsoft.DirectX.Direct3D.EffectInstance objects.
        void SetEffectInstances(EffectInstance[] effects);
        //
        // Summary:
        //     Sets an array of mesh materials.
        //
        // Parameters:
        //   mtrl:
        //     Array of Microsoft.DirectX.Direct3D.ExtendedMaterial objects that contain
        //     the mesh materials to set.
        void SetMaterials(ExtendedMaterial[] mtrl);

        // Special methods for DDXX
        Matrix[] RestMatrices { get; set; }
        BoneCombination[] Bones { get; set; }
        IFrame[] Frames { get; set; }
    }
}
