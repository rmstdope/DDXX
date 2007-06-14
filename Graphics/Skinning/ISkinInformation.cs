using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public interface ISkinInformation : IDisposable
    {
        // Summary:
        //     Retrieves or sets the vertex declaration.
        VertexElement[] Declaration { get; set; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        bool Disposed { get; }
        //
        // Summary:
        //     Retrieves the maximum number of influences for any vertex in a mesh.
        int MaxVertexInfluences { get; }
        //
        // Summary:
        //     Retrieves or sets the minimum bone influence.
        float MinBoneInfluence { get; set; }
        //
        // Summary:
        //     Retrieves the number of bones in a mesh.
        int NumberBones { get; }
        //
        // Summary:
        //     Retrieves or sets the fixed function vertex value.
        VertexFormats VertexFormat { get; set; }
        // Summary:
        //     Clones a Microsoft.DirectX.Direct3D.SkinInformation object.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.SkinInformation object that contains the cloned
        //     object.
        SkinInformation Clone();
        //
        // Summary:
        //     Takes an existing mesh and returns a new mesh with per-vertex blend weights
        //     and a bone combination table. The table describes which bones affect which
        //     subsets of the mesh.
        //
        // Parameters:
        //   mesh:
        //     Source Microsoft.DirectX.Direct3D.Mesh.
        //
        //   options:
        //     A Microsoft.DirectX.Direct3D.MeshFlags object that specifies creation options
        //     for the new mesh.
        //
        //   adjacencyIn:
        //     A Microsoft.DirectX.GraphicsStream object that contains the input mesh adjacency
        //     information.
        //
        //   maxFaceInfluence:
        //     Integer that contains the maximum number of bone influences required per
        //     vertex for the current skinning method.
        //
        //   boneCombinationTable:
        //     Array of Microsoft.DirectX.Direct3D.BoneCombination objects.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the new mesh.
        IMesh ConvertToBlendedMesh(IMesh mesh, MeshFlags options, IGraphicsStream adjacencyIn, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable);
        //
        // Summary:
        //     Takes an existing mesh and returns a new mesh with per-vertex blend weights
        //     and a bone combination table. The table describes which bones affect which
        //     subsets of the mesh.
        //
        // Parameters:
        //   mesh:
        //     Source Microsoft.DirectX.Direct3D.Mesh.
        //
        //   options:
        //     A Microsoft.DirectX.Direct3D.MeshFlags object that specifies creation options
        //     for the new mesh.
        //
        //   adjacencyIn:
        //     Array that contains the input mesh adjacency information.
        //
        //   maxFaceInfluence:
        //     Integer that contains the maximum number of bone influences required per
        //     vertex for the current skinning method.
        //
        //   boneCombinationTable:
        //     Array of Microsoft.DirectX.Direct3D.BoneCombination objects.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the new mesh.
        IMesh ConvertToBlendedMesh(IMesh mesh, MeshFlags options, int[] adjacencyIn, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable);
        //
        // Summary:
        //     Takes an existing mesh and returns a new mesh with per-vertex blend weights
        //     and a bone combination table. The table describes which bones affect which
        //     subsets of the mesh.
        //
        // Parameters:
        //   mesh:
        //     Source Microsoft.DirectX.Direct3D.Mesh.
        //
        //   options:
        //     A Microsoft.DirectX.Direct3D.MeshFlags object that specifies creation options
        //     for the new mesh.
        //
        //   adjacencyIn:
        //     A Microsoft.DirectX.GraphicsStream object that contains the input mesh adjacency
        //     information.
        //
        //   maxFaceInfluence:
        //     Integer that contains the maximum number of bone influences required per
        //     vertex for the current skinning method.
        //
        //   boneCombinationTable:
        //     Array of Microsoft.DirectX.Direct3D.BoneCombination objects.
        //
        //   adjacencyOut:
        //     Array that contains the output mesh adjacency information.
        //
        //   faceRemap:
        //     Array that contains a new index for each face.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream object that contains a new index for each
        //     vertex.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the new mesh.
        IMesh ConvertToBlendedMesh(IMesh mesh, MeshFlags options, IGraphicsStream adjacencyIn, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap);
        //
        // Summary:
        //     Takes an existing mesh and returns a new mesh with per-vertex blend weights
        //     and a bone combination table. The table describes which bones affect which
        //     subsets of the mesh.
        //
        // Parameters:
        //   mesh:
        //     Source Microsoft.DirectX.Direct3D.Mesh.
        //
        //   options:
        //     A Microsoft.DirectX.Direct3D.MeshFlags object that specifies creation options
        //     for the new mesh.
        //
        //   adjacencyIn:
        //     Array that contains the input mesh adjacency information.
        //
        //   maxFaceInfluence:
        //     Integer that contains the maximum number of bone influences required per
        //     vertex for the current skinning method.
        //
        //   boneCombinationTable:
        //     Array of Microsoft.DirectX.Direct3D.BoneCombination objects.
        //
        //   adjacencyOut:
        //     Array that contains the output mesh adjacency information.
        //
        //   faceRemap:
        //     Array that contains a new index for each face.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream object that contains a new index for each
        //     vertex.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the new mesh.
        IMesh ConvertToBlendedMesh(IMesh mesh, MeshFlags options, int[] adjacencyIn, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap);
        //
        // Summary:
        //     Takes an existing mesh and returns a new mesh with per-vertex blend weights,
        //     indices, and a bone combination table. The table describes which bone palettes
        //     affect which subsets of the mesh.
        //
        // Parameters:
        //   mesh:
        //     Source Microsoft.DirectX.Direct3D.Mesh.
        //
        //   options:
        //     A Microsoft.DirectX.Direct3D.MeshFlags object that specifies creation options
        //     for the new mesh.
        //
        //   adjacencyIn:
        //     A Microsoft.DirectX.GraphicsStream object that contains the input mesh adjacency
        //     information.
        //
        //   paletteSize:
        //     Value that contains the number of bone matrices available for matrix palette
        //     skinning.
        //
        //   maxFaceInfluence:
        //     Value that contains the maximum number of bone influences required per vertex
        //     for the current skinning method.
        //
        //   boneCombinationTable:
        //     Array of Microsoft.DirectX.Direct3D.BoneCombination objects.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the new mesh.
        IMesh ConvertToIndexedBlendedMesh(IMesh mesh, MeshFlags options, IGraphicsStream adjacencyIn, int paletteSize, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable);
        //
        // Summary:
        //     Takes an existing mesh and returns a new mesh with per-vertex blend weights,
        //     indices, and a bone combination table. The table describes which bone palettes
        //     affect which subsets of the mesh.
        //
        // Parameters:
        //   mesh:
        //     Source Microsoft.DirectX.Direct3D.Mesh.
        //
        //   options:
        //     A Microsoft.DirectX.Direct3D.MeshFlags object that specifies creation options
        //     for the new mesh.
        //
        //   adjacencyIn:
        //     Array that contains the input mesh adjacency information.
        //
        //   paletteSize:
        //     Value that contains the number of bone matrices available for matrix palette
        //     skinning.
        //
        //   maxFaceInfluence:
        //     Value that contains the maximum number of bone influences required per vertex
        //     for the current skinning method.
        //
        //   boneCombinationTable:
        //     Array of Microsoft.DirectX.Direct3D.BoneCombination objects.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the new mesh.
        IMesh ConvertToIndexedBlendedMesh(IMesh mesh, MeshFlags options, int[] adjacencyIn, int paletteSize, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable);
        //
        // Summary:
        //     Takes an existing mesh and returns a new mesh with per-vertex blend weights,
        //     indices, and a bone combination table. The table describes which bone palettes
        //     affect which subsets of the mesh.
        //
        // Parameters:
        //   mesh:
        //     Source Microsoft.DirectX.Direct3D.Mesh.
        //
        //   options:
        //     A Microsoft.DirectX.Direct3D.MeshFlags object that specifies creation options
        //     for the new mesh.
        //
        //   adjacencyIn:
        //     A Microsoft.DirectX.GraphicsStream object that contains the input mesh adjacency
        //     information.
        //
        //   paletteSize:
        //     Value that contains the number of bone matrices available for matrix palette
        //     skinning.
        //
        //   maxFaceInfluence:
        //     Value that contains the maximum number of bone influences required per vertex
        //     for the current skinning method.
        //
        //   boneCombinationTable:
        //     Array of Microsoft.DirectX.Direct3D.BoneCombination objects.
        //
        //   adjacencyOut:
        //     Array that contains the output mesh adjacency information.
        //
        //   faceRemap:
        //     Array that contains a new index for each face.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream object that contains a new index for each
        //     vertex.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the new mesh.
        IMesh ConvertToIndexedBlendedMesh(IMesh mesh, MeshFlags options, IGraphicsStream adjacencyIn, int paletteSize, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap);
        //
        // Summary:
        //     Takes an existing mesh and returns a new mesh with per-vertex blend weights,
        //     indices, and a bone combination table. The table describes which bone palettes
        //     affect which subsets of the mesh.
        //
        // Parameters:
        //   mesh:
        //     Source Microsoft.DirectX.Direct3D.Mesh.
        //
        //   options:
        //     A Microsoft.DirectX.Direct3D.MeshFlags object that specifies creation options
        //     for the new mesh.
        //
        //   adjacencyIn:
        //     Array that contains the input mesh adjacency information.
        //
        //   paletteSize:
        //     Value that contains the number of bone matrices available for matrix palette
        //     skinning.
        //
        //   maxFaceInfluence:
        //     Value that contains the maximum number of bone influences required per vertex
        //     for the current skinning method.
        //
        //   boneCombinationTable:
        //     Array of Microsoft.DirectX.Direct3D.BoneCombination objects.
        //
        //   adjacencyOut:
        //     Array that contains the output mesh adjacency information.
        //
        //   faceRemap:
        //     Array that contains a new index for each face.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream object that contains a new index for each
        //     vertex.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the new mesh.
        IMesh ConvertToIndexedBlendedMesh(IMesh mesh, MeshFlags options, int[] adjacencyIn, int paletteSize, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap);
        //
        // Summary:
        //     Retrieves the vertices and weights that a bone influences.
        //
        // Parameters:
        //   bone:
        //     Integer that represents the bone number.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.BoneInfluences object that contains the bone
        //     influences.
        BoneInfluences GetBoneInfluence(int bone);
        //
        // Summary:
        //     Retrieves the bone name from the bone index.
        //
        // Parameters:
        //   bone:
        //     Value that represents the bone number of the bone name to retrieve.
        //
        // Returns:
        //     String that contains the bone name.
        string GetBoneName(int bone);
        //
        // Summary:
        //     Retrieves the bone offset matrix.
        //
        // Parameters:
        //   bone:
        //     Value that represents the bone number of the bone offset matrix to retrieve.
        //
        // Returns:
        //     A Microsoft.DirectX.Matrix object that represents the bone offset matrix.
        Matrix GetBoneOffsetMatrix(int bone);
        //
        // Summary:
        //     Retrieves the maximum face influences in a triangle mesh with the specified
        //     index buffer.
        //
        // Parameters:
        //   indexBuffer:
        //     An Microsoft.DirectX.Direct3D.IndexBuffer object that contains the mesh index
        //     data.
        //
        //   numFaces:
        //     Value that represents the number of faces in the mesh.
        //
        // Returns:
        //     Value that contains the maximum face influences.
        int GetMaxFaceInfluences(IndexBuffer indexBuffer, int numFaces);
        //
        // Summary:
        //     Retrieves the number of influences for a bone.
        //
        // Parameters:
        //   bone:
        //     Value that represents the bone number for which to retrieve the number of
        //     influences.
        //
        // Returns:
        //     Value that contains the number of influences for the bone.
        int GetNumBoneInfluences(int bone);
        //
        // Summary:
        //     Updates bone influence information to match vertices after they are reordered.
        //
        // Parameters:
        //   vertRemap:
        //     Array of integers that represent the vertices to remap.
        void Remap(int[] vertRemap);
        //
        // Summary:
        //     Sets the influence value for a bone.
        //
        // Parameters:
        //   bone:
        //     Value that represents the bone number.
        //
        //   numInfluences:
        //     Value that represents the number of influences.
        //
        //   vertices:
        //     Array of integers that represent the vertices influenced by the bone.
        //
        //   weights:
        //     Array of floating-point values that represent the weights influenced by the
        //     bone.
        void SetBoneInfluence(int bone, int numInfluences, int[] vertices, float[] weights);
        //
        // Summary:
        //     Sets the bone name associated with a bone number.
        //
        // Parameters:
        //   bone:
        //     Value that represents the bone number.
        //
        //   name:
        //     String that contains the bone name.
        void SetBoneName(int bone, string name);
        //
        // Summary:
        //     Sets the bone offset matrix.
        //
        // Parameters:
        //   bone:
        //     Value that represents the bone number of the bone offset matrix.
        //
        //   boneTransform:
        //     A Microsoft.DirectX.Matrix object that represents the bone offset matrix.
        void SetBoneOffsetMatrix(int bone, Matrix boneTransform);
        //
        // Summary:
        //     Applies software skinning to target vertices that are based on the current
        //     matrices.
        //
        // Parameters:
        //   boneTransforms:
        //     Array of Microsoft.DirectX.Matrix objects that represent bone transforms.
        //
        //   boneInvTransforms:
        //     Array of Microsoft.DirectX.Matrix objects that represent the inverse transpose
        //     of the bone transform matrix.
        //
        //   verticesSource:
        //     An System.Array of source vertices.
        //
        // Returns:
        //     An System.Array of destination vertices.
        Array UpdateSkinnedMesh(Matrix[] boneTransforms, Matrix[] boneInvTransforms, Array verticesSource);
        //
        // Summary:
        //     Applies software skinning to target vertices that are based on the current
        //     matrices.
        //
        // Parameters:
        //   boneTransforms:
        //     Array of Microsoft.DirectX.Matrix objects that represent bone transforms.
        //
        //   boneInvTransforms:
        //     Array of Microsoft.DirectX.Matrix objects that represent the inverse transpose
        //     of the bone transform matrix.
        //
        //   sourceVertices:
        //     A Microsoft.DirectX.GraphicsStream object that contains the source vertices.
        //
        //   destinationVertices:
        //     A Microsoft.DirectX.GraphicsStream object that contains destination vertices.
        void UpdateSkinnedMesh(Matrix[] boneTransforms, Matrix[] boneInvTransforms, IGraphicsStream sourceVertices, IGraphicsStream destinationVertices);
    }
}
