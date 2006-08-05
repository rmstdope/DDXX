using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Direct3D
{
    public interface IMesh
    {
        //
        // Summary:
        //     Computes the tangent vectors for the texture coordinates given in a texture
        //     stage.
        //
        // Parameters:
        //   texStage:
        //     Index that represents the Microsoft.DirectX.Direct3D.TextureStateManager
        //     in Microsoft.DirectX.Direct3D.TextureStateManagerCollection.
        //
        //   tangentIndex:
        //     Index that provides the usage index for the tangent data. The vertex declaration
        //     implies the usage; this index modifies it.
        //
        //   binormIndex:
        //     Index that provides the usage index for the binormal data. The vertex declaration
        //     implies the usage; this index modifies it.
        //
        //   wrap:
        //     Value that is set to 0 to specify no wrapping, or to 1 to specify wrapping
        //     in the u and v directions.
        void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap);
        //
        // Summary:
        //     Computes the tangent vectors for the texture coordinates given in a texture
        //     stage.
        //
        // Parameters:
        //   texStage:
        //     Index that represents the Microsoft.DirectX.Direct3D.TextureStateManager
        //     in Microsoft.DirectX.Direct3D.TextureStateManagerCollection.
        //
        //   tangentIndex:
        //     Index that provides the usage index for the tangent data. The vertex declaration
        //     implies the usage; this index modifies it.
        //
        //   binormIndex:
        //     Index that provides the usage index for the binormal data. The vertex declaration
        //     implies the usage; this index modifies it.
        //
        //   wrap:
        //     Value that is set to 0 to specify no wrapping, or to 1 to specify wrapping
        //     in the u and v directions.
        //
        //   adjacency:
        //     A Microsoft.DirectX.GraphicsStream object filled with an array of three System.Int32
        //     values per face to be filled with adjacent face indices.
        void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap, GraphicsStream adjacency);
        //
        // Summary:
        //     Computes the tangent vectors for the texture coordinates given in a texture
        //     stage.
        //
        // Parameters:
        //   texStage:
        //     Index that represents the Microsoft.DirectX.Direct3D.TextureStateManager
        //     in Microsoft.DirectX.Direct3D.TextureStateManagerCollection.
        //
        //   tangentIndex:
        //     Index that provides the usage index for the tangent data. The vertex declaration
        //     implies the usage; this index modifies it.
        //
        //   binormIndex:
        //     Index that provides the usage index for the binormal data. The vertex declaration
        //     implies the usage; this index modifies it.
        //
        //   wrap:
        //     Value that is set to 0 to specify no wrapping, or to 1 to specify wrapping
        //     in the u and v directions.
        //
        //   adjacency:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the mesh. The size of this array must be at least 3 * Microsoft.DirectX.Direct3D.BaseMesh.NumberFaces.
        void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap, int[] adjacency);
        //
        // Summary:
        //     Performs tangent frame computations on a mesh. Tangent, binormal, and optionally
        //     normal vectors are generated. Singularities are handled as required by grouping
        //     edges and splitting vertices.
        //
        // Parameters:
        //   options:
        //     Combination of one or more Microsoft.DirectX.Direct3D.TangentOptions values
        //     that specify tangent frame computation options. If zero, the following options
        //     will be specified: & ( GenerateInPlace | ( !( WeightByArea | WeightEqual
        //     | OrthogonalizeFromU | OrthogonalizeFromV | WrapUV | DontNormalizePartials
        //     | WindClockwise | CalculateNormals ) ).
        void ComputeTangentFrame(TangentOptions options);
        //
        // Summary:
        //     Determines whether a ray intersects with a mesh.
        //
        // Parameters:
        //   rayPos:
        //     A Microsoft.DirectX.Vector3 structure that specifies the origin coordinate
        //     of the ray.
        //
        //   rayDir:
        //     A Microsoft.DirectX.Vector3 structure that specifies the direction of the
        //     ray.
        //
        // Returns:
        //     Value that is true if the ray intersects a triangular face on the mesh; otherwise,
        //     the value is false.
        bool Intersect(Vector3 rayPos, Vector3 rayDir);
        //
        // Summary:
        //     Determines whether a ray intersects with a mesh.
        //
        // Parameters:
        //   rayPos:
        //     A Microsoft.DirectX.Vector3 structure that specifies the origin coordinate
        //     of the ray.
        //
        //   rayDir:
        //     A Microsoft.DirectX.Vector3 structure that specifies the direction of the
        //     ray.
        //
        //   closestHit:
        //     An Microsoft.DirectX.Direct3D.IntersectInformation object that describes
        //     the closest intersection between the array and the mesh.
        //
        // Returns:
        //     Value that is true if the ray intersects a triangular face on the mesh; otherwise,
        //     the value is false.
        bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit);
        //
        // Summary:
        //     Determines whether a ray intersects with a mesh.
        //
        // Parameters:
        //   rayPos:
        //     A Microsoft.DirectX.Vector3 structure that specifies the origin coordinate
        //     of the ray.
        //
        //   rayDir:
        //     A Microsoft.DirectX.Vector3 structure that specifies the direction of the
        //     ray.
        //
        //   allHits:
        //     Array of Microsoft.DirectX.Direct3D.IntersectInformation structures that
        //     describe all intersections of the ray and the mesh.
        //
        // Returns:
        //     Value that is true if the ray intersects a triangular face on the mesh; otherwise,
        //     the value is false.
        bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation[] allHits);
        //
        // Summary:
        //     Determines whether a ray intersects with a mesh.
        //
        // Parameters:
        //   rayPos:
        //     A Microsoft.DirectX.Vector3 structure that specifies the origin coordinate
        //     of the ray.
        //
        //   rayDir:
        //     A Microsoft.DirectX.Vector3 structure that specifies the direction of the
        //     ray.
        //
        //   closestHit:
        //     An Microsoft.DirectX.Direct3D.IntersectInformation object that describes
        //     the closest intersection between the array and the mesh.
        //
        //   allHits:
        //     Array of Microsoft.DirectX.Direct3D.IntersectInformation structures that
        //     describe all intersections of the ray and the mesh.
        //
        // Returns:
        //     Value that is true if the ray intersects a triangular face on the mesh; otherwise,
        //     the value is false.
        bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit, out IntersectInformation[] allHits);
        //
        // Summary:
        //     Intersects the specified ray with a given mesh subset.
        //
        // Parameters:
        //   attributeId:
        //     Attribute identifier of the subset to intersect with.
        //
        //   rayPos:
        //     A Microsoft.DirectX.Vector3 structure that specifies the origin coordinate
        //     of the ray.
        //
        //   rayDir:
        //     A Microsoft.DirectX.Vector3 structure that specifies the direction of the
        //     ray.
        //
        // Returns:
        //     Value that is true if the ray intersects a triangular face on the mesh; otherwise,
        //     the value is false.
        bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir);
        //
        // Summary:
        //     Intersects the specified ray with a given mesh subset.
        //
        // Parameters:
        //   attributeId:
        //     Attribute identifier of the subset to intersect with.
        //
        //   rayPos:
        //     A Microsoft.DirectX.Vector3 structure that specifies the origin coordinate
        //     of the ray.
        //
        //   rayDir:
        //     A Microsoft.DirectX.Vector3 structure that specifies the direction of the
        //     ray.
        //
        //   closestHit:
        //     An Microsoft.DirectX.Direct3D.IntersectInformation object that describes
        //     the closest intersection between the array and the mesh.
        //
        // Returns:
        //     Value that is true if the ray intersects a triangular face on the mesh; otherwise,
        //     the value is false.
        bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit);
        //
        // Summary:
        //     Intersects the specified ray with a given mesh subset.
        //
        // Parameters:
        //   attributeId:
        //     Attribute identifier of the subset to intersect with.
        //
        //   rayPos:
        //     A Microsoft.DirectX.Vector3 structure that specifies the origin coordinate
        //     of the ray.
        //
        //   rayDir:
        //     A Microsoft.DirectX.Vector3 structure that specifies the direction of the
        //     ray.
        //
        //   allHits:
        //     Array of Microsoft.DirectX.Direct3D.IntersectInformation structures that
        //     describe all intersections of the ray and the mesh.
        //
        // Returns:
        //     Value that is true if the ray intersects a triangular face on the mesh; otherwise,
        //     the value is false.
        bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation[] allHits);
        //
        // Summary:
        //     Intersects the specified ray with a given mesh subset.
        //
        // Parameters:
        //   attributeId:
        //     Attribute identifier of the subset to intersect with.
        //
        //   rayPos:
        //     A Microsoft.DirectX.Vector3 structure that specifies the origin coordinate
        //     of the ray.
        //
        //   rayDir:
        //     A Microsoft.DirectX.Vector3 structure that specifies the direction of the
        //     ray.
        //
        //   closestHit:
        //     An Microsoft.DirectX.Direct3D.IntersectInformation object that describes
        //     the closest intersection between the array and the mesh.
        //
        //   allHits:
        //     Array of Microsoft.DirectX.Direct3D.IntersectInformation structures that
        //     describe all intersections of the ray and the mesh.
        //
        // Returns:
        //     Value that is true if the ray intersects a triangular face on the mesh; otherwise,
        //     the value is false.
        bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit, out IntersectInformation[] allHits);
        //
        // Summary:
        //     Locks an attribute buffer and obtains its memory.
        //
        // Parameters:
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags that describe the type
        //     of lock to perform. For this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        GraphicsStream LockAttributeBuffer(LockFlags flags);
        //
        // Summary:
        //     Locks an attribute buffer and obtains its memory.
        //
        // Parameters:
        //   flags:
        //     Zero or more Microsoft.DirectX.Direct3D.LockFlags that describe the type
        //     of lock to perform. For this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly.
        //
        // Returns:
        //     Returns an array that contains three System.Int32 values for each face in
        //     the mesh.
        int[] LockAttributeBufferArray(LockFlags flags);
        //
        // Summary:
        //     Controls the reordering of mesh faces and vertices to optimize performance
        //     and generate an output mesh.
        //
        // Parameters:
        //   flags:
        //     Type of optimization to perform; can be set to one or more Microsoft.DirectX.Direct3D.MeshFlags
        //     flags (except Microsoft.DirectX.Direct3D.MeshFlags.Use32Bit, Microsoft.DirectX.Direct3D.MeshFlags.IbWriteOnly,
        //     and Microsoft.DirectX.Direct3D.MeshFlags.WriteOnly).
        //
        //   adjacencyIn:
        //     A Microsoft.DirectX.GraphicsStream containing three System.Int32 values per
        //     face that specify the three neighbors for each face in the source mesh. If
        //     the edge has no adjacent faces, the value is 0.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the optimized mesh.
        Mesh Optimize(MeshFlags flags, GraphicsStream adjacencyIn);
        //
        // Summary:
        //     Controls the reordering of mesh faces and vertices to optimize performance
        //     and generate an output mesh.
        //
        // Parameters:
        //   flags:
        //     Type of optimization to perform; can be set to one or more Microsoft.DirectX.Direct3D.MeshFlags
        //     flags (except Microsoft.DirectX.Direct3D.MeshFlags.Use32Bit, Microsoft.DirectX.Direct3D.MeshFlags.IbWriteOnly,
        //     and Microsoft.DirectX.Direct3D.MeshFlags.WriteOnly).
        //
        //   adjacencyIn:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the source mesh. If the edge has no adjacent faces, the
        //     value is 0.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the optimized mesh.
        Mesh Optimize(MeshFlags flags, int[] adjacencyIn);
        //
        // Summary:
        //     Controls the reordering of mesh faces and vertices to optimize performance
        //     and generate an output mesh.
        //
        // Parameters:
        //   flags:
        //     Type of optimization to perform; can be set to one or more Microsoft.DirectX.Direct3D.MeshFlags
        //     flags (except Microsoft.DirectX.Direct3D.MeshFlags.Use32Bit, Microsoft.DirectX.Direct3D.MeshFlags.IbWriteOnly,
        //     and Microsoft.DirectX.Direct3D.MeshFlags.WriteOnly).
        //
        //   adjacencyIn:
        //     A Microsoft.DirectX.GraphicsStream containing three System.Int32 values per
        //     face that specify the three neighbors for each face in the source mesh. If
        //     the edge has no adjacent faces, the value is 0.
        //
        //   adjacencyOut:
        //     [in, out] Array for the face adjacency array of the optimized mesh. The face
        //     adjacency is stored as an array of arrays. The innermost array is three indices
        //     of adjacent triangles, and the outermost array is one set of face adjacencies
        //     per triangle in the mesh.
        //
        //   faceRemap:
        //     [in, out] Destination buffer that contains the new index for each face.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream that contains the new index for each vertex.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the optimized mesh.
        Mesh Optimize(MeshFlags flags, GraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap);
        //
        // Summary:
        //     Controls the reordering of mesh faces and vertices to optimize performance
        //     and generate an output mesh.
        //
        // Parameters:
        //   flags:
        //     Type of optimization to perform; can be set to one or more Microsoft.DirectX.Direct3D.MeshFlags
        //     flags (except Microsoft.DirectX.Direct3D.MeshFlags.Use32Bit, Microsoft.DirectX.Direct3D.MeshFlags.IbWriteOnly,
        //     and Microsoft.DirectX.Direct3D.MeshFlags.WriteOnly).
        //
        //   adjacencyIn:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the source mesh. If the edge has no adjacent faces, the
        //     value is 0.
        //
        //   adjacencyOut:
        //     [in, out] Array for the face adjacency array of the optimized mesh. The face
        //     adjacency is stored as an array of arrays. The innermost array is three indices
        //     of adjacent triangles, and the outermost array is one set of face adjacencies
        //     per triangle in the mesh.
        //
        //   faceRemap:
        //     [in, out] Destination buffer that contains the new index for each face.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream that contains the new index for each vertex.
        //
        // Returns:
        //     A Microsoft.DirectX.Direct3D.Mesh object that represents the optimized mesh.
        Mesh Optimize(MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap);
        //
        // Summary:
        //     Controls the reordering of mesh faces and vertices to optimize performance.
        //
        // Parameters:
        //   flags:
        //     Type of optimization to perform; can be set to one or more of the Microsoft.DirectX.Direct3D.MeshFlagsOptimize*
        //     flags.
        //
        //   adjacencyIn:
        //     A Microsoft.DirectX.GraphicsStream of three System.Int32 values per face
        //     that specify the three neighbors for each face in the source mesh. If the
        //     edge has no adjacent faces, the value is 0.
        void OptimizeInPlace(MeshFlags flags, GraphicsStream adjacencyIn);
        //
        // Summary:
        //     Controls the reordering of mesh faces and vertices to optimize performance.
        //
        // Parameters:
        //   flags:
        //     Type of optimization to perform; can be set to one or more of the Microsoft.DirectX.Direct3D.MeshFlagsOptimize*
        //     flags.
        //
        //   adjacencyIn:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the source mesh. If the edge has no adjacent faces, the
        //     value is 0.
        void OptimizeInPlace(MeshFlags flags, int[] adjacencyIn);
        //
        // Summary:
        //     Controls the reordering of mesh faces and vertices to optimize performance.
        //
        // Parameters:
        //   flags:
        //     Type of optimization to perform; can be set to one or more of the Microsoft.DirectX.Direct3D.MeshFlagsOptimize*
        //     flags.
        //
        //   adjacencyIn:
        //     A Microsoft.DirectX.GraphicsStream of three System.Int32 values per face
        //     that specify the three neighbors for each face in the source mesh. If the
        //     edge has no adjacent faces, the value is 0.
        //
        //   adjacencyOut:
        //     [in, out] Array for the face adjacency array of the optimized mesh. The face
        //     adjacency is stored as an array of arrays. The innermost array is three indices
        //     of adjacent triangles, and the outermost array is one set of face adjacencies
        //     per triangle in the mesh.
        //
        //   faceRemap:
        //     [in, out] Destination buffer that contains the new index for each face.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream that contains the new index for each vertex.
        void OptimizeInPlace(MeshFlags flags, GraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap);
        //
        // Summary:
        //     Controls the reordering of mesh faces and vertices to optimize performance.
        //
        // Parameters:
        //   flags:
        //     Type of optimization to perform; can be set to one or more of the Microsoft.DirectX.Direct3D.MeshFlagsOptimize*
        //     flags.
        //
        //   adjacencyIn:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the source mesh. If the edge has no adjacent faces, the
        //     value is 0.
        //
        //   adjacencyOut:
        //     [in, out] Array for the face adjacency array of the optimized mesh. The face
        //     adjacency is stored as an array of arrays. The innermost array is three indices
        //     of adjacent triangles, and the outermost array is one set of face adjacencies
        //     per triangle in the mesh.
        //
        //   faceRemap:
        //     [in, out] Destination buffer that contains the new index for each face.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream that contains the new index for each vertex.
        void OptimizeInPlace(MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap);
        //
        // Summary:
        //     Saves the mesh to the specified stream object.
        //
        // Parameters:
        //   stream:
        //     The System.IO.Stream in which to save the .x file.
        //
        //   adjacency:
        //     A Microsoft.DirectX.GraphicsStream of three System.Int32 values per face
        //     that specify the three neighbors for each face in the mesh.
        //
        //   materials:
        //     Array of Microsoft.DirectX.Direct3D.ExtendedMaterial structures that contain
        //     material information to save in the DirectX (.x) file.
        //
        //   format:
        //     An Microsoft.DirectX.Direct3D.XFileFormat that indicates the format to use
        //     when saving the .x file. See Remarks.
        void Save(Stream stream, GraphicsStream adjacency, ExtendedMaterial[] materials, XFileFormat format);
        //
        // Summary:
        //     Saves the mesh to the specified stream object.
        //
        // Parameters:
        //   stream:
        //     The System.IO.Stream in which to save the .x file.
        //
        //   adjacency:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the mesh.
        //
        //   materials:
        //     Array of Microsoft.DirectX.Direct3D.ExtendedMaterial structures that contain
        //     material information to save in the DirectX (.x) file.
        //
        //   format:
        //     An Microsoft.DirectX.Direct3D.XFileFormat that indicates the format to use
        //     when saving the .x file. See Remarks.
        void Save(Stream stream, int[] adjacency, ExtendedMaterial[] materials, XFileFormat format);
        //
        // Summary:
        //     Saves the mesh to the specified stream object.
        //
        // Parameters:
        //   filename:
        //     String that specifies the file name.
        //
        //   adjacency:
        //     A Microsoft.DirectX.GraphicsStream of three System.Int32 values per face
        //     that specify the three neighbors for each face in the mesh.
        //
        //   materials:
        //     Array of Microsoft.DirectX.Direct3D.ExtendedMaterial structures that contain
        //     material information to save in the DirectX (.x) file.
        //
        //   format:
        //     An Microsoft.DirectX.Direct3D.XFileFormat that indicates the format to use
        //     when saving the .x file. See Remarks.
        void Save(string filename, GraphicsStream adjacency, ExtendedMaterial[] materials, XFileFormat format);
        //
        // Summary:
        //     Saves the mesh to the specified stream object.
        //
        // Parameters:
        //   filename:
        //     String that specifies the file name.
        //
        //   adjacency:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the mesh.
        //
        //   materials:
        //     Array of Microsoft.DirectX.Direct3D.ExtendedMaterial structures that contain
        //     material information to save in the DirectX (.x) file.
        //
        //   format:
        //     An Microsoft.DirectX.Direct3D.XFileFormat that indicates the format to use
        //     when saving the .x file. See Remarks.
        void Save(string filename, int[] adjacency, ExtendedMaterial[] materials, XFileFormat format);
        //
        // Summary:
        //     Saves the mesh to the specified stream object.
        //
        // Parameters:
        //   stream:
        //     The System.IO.Stream in which to save the .x file.
        //
        //   adjacency:
        //     A Microsoft.DirectX.GraphicsStream of three System.Int32 values per face
        //     that specify the three neighbors for each face in the mesh.
        //
        //   materials:
        //     Array of Microsoft.DirectX.Direct3D.ExtendedMaterial structures that contain
        //     material information to save in the DirectX (.x) file.
        //
        //   effects:
        //     Array of Microsoft.DirectX.Direct3D.EffectInstance structures, one per attribute
        //     group in the mesh. An effect instance is a particular instance of state information
        //     used to initialize an effect.
        //
        //   format:
        //     An Microsoft.DirectX.Direct3D.XFileFormat that indicates the format to use
        //     when saving the .x file. See Remarks.
        void Save(Stream stream, GraphicsStream adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format);
        //
        // Summary:
        //     Saves the mesh to the specified stream object.
        //
        // Parameters:
        //   stream:
        //     The System.IO.Stream in which to save the .x file.
        //
        //   adjacency:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the mesh.
        //
        //   materials:
        //     Array of Microsoft.DirectX.Direct3D.ExtendedMaterial structures that contain
        //     material information to save in the DirectX (.x) file.
        //
        //   effects:
        //     Array of Microsoft.DirectX.Direct3D.EffectInstance structures, one per attribute
        //     group in the mesh. An effect instance is a particular instance of state information
        //     used to initialize an effect.
        //
        //   format:
        //     An Microsoft.DirectX.Direct3D.XFileFormat that indicates the format to use
        //     when saving the .x file. See Remarks.
        void Save(Stream stream, int[] adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format);
        //
        // Summary:
        //     Saves the mesh to the specified stream object.
        //
        // Parameters:
        //   filename:
        //     String that specifies the file name.
        //
        //   adjacency:
        //     A Microsoft.DirectX.GraphicsStream of three System.Int32 values per face
        //     that specify the three neighbors for each face in the mesh.
        //
        //   materials:
        //     Array of Microsoft.DirectX.Direct3D.ExtendedMaterial structures that contain
        //     material information to save in the DirectX (.x) file.
        //
        //   effects:
        //     Array of Microsoft.DirectX.Direct3D.EffectInstance structures, one per attribute
        //     group in the mesh. An effect instance is a particular instance of state information
        //     used to initialize an effect.
        //
        //   format:
        //     An Microsoft.DirectX.Direct3D.XFileFormat that indicates the format to use
        //     when saving the .x file. See Remarks.
        void Save(string filename, GraphicsStream adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format);
        //
        // Summary:
        //     Saves the mesh to the specified stream object.
        //
        // Parameters:
        //   filename:
        //     String that specifies the file name.
        //
        //   adjacency:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the mesh.
        //
        //   materials:
        //     Array of Microsoft.DirectX.Direct3D.ExtendedMaterial structures that contain
        //     material information to save in the DirectX (.x) file.
        //
        //   effects:
        //     Array of Microsoft.DirectX.Direct3D.EffectInstance structures, one per attribute
        //     group in the mesh. An effect instance is a particular instance of state information
        //     used to initialize an effect.
        //
        //   format:
        //     An Microsoft.DirectX.Direct3D.XFileFormat that indicates the format to use
        //     when saving the .x file. See Remarks.
        void Save(string filename, int[] adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format);
        //
        // Summary:
        //     Sets a mesh's attribute table and specifies the number of entries stored
        //     in it.
        //
        // Parameters:
        //   table:
        //     Array of Microsoft.DirectX.Direct3D.AttributeRange structures that represent
        //     the entries in the mesh attribute table.
        void SetAttributeTable(AttributeRange[] table);
        //
        // Summary:
        //     Unlocks an attribute buffer.
        void UnlockAttributeBuffer();
        //
        // Summary:
        //     Unlocks an attribute buffer.
        //
        // Parameters:
        //   dataAttribute:
        //     Array of pointers that represents the attribute buffer to unlock.
        void UnlockAttributeBuffer(int[] dataAttribute);
        //
        // Summary:
        //     Validates a mesh.
        //
        // Parameters:
        //   adjacency:
        //     A Microsoft.DirectX.GraphicsStream containing three System.Int32 values per
        //     face that specify the three neighbors for each face in the mesh to be tested.
        void Validate(GraphicsStream adjacency);
        //
        // Summary:
        //     Validates a mesh.
        //
        // Parameters:
        //   adjacency:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the mesh to be tested.
        void Validate(int[] adjacency);
        //
        // Summary:
        //     Validates a mesh.
        //
        // Parameters:
        //   adjacency:
        //     A Microsoft.DirectX.GraphicsStream containing three System.Int32 values per
        //     face that specify the three neighbors for each face in the mesh to be tested.
        //
        //   errorsAndWarnings:
        //     Returns a buffer that contains a string of errors and warnings, which explain
        //     the problems found in the mesh.
        void Validate(GraphicsStream adjacency, out string errorsAndWarnings);
        //
        // Summary:
        //     Validates a mesh.
        //
        // Parameters:
        //   adjacency:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the mesh to be tested.
        //
        //   errorsAndWarnings:
        //     Returns a buffer that contains a string of errors and warnings, which explain
        //     the problems found in the mesh.
        void Validate(int[] adjacency, out string errorsAndWarnings);
        //
        // Summary:
        //     Welds together replicated vertices that have equal attributes.
        //
        // Parameters:
        //   flags:
        //     One or more flags from Microsoft.DirectX.Direct3D.WeldEpsilonsFlags.
        //
        //   epsilons:
        //     A Microsoft.DirectX.Direct3D.WeldEpsilons structure that specifies the epsilon
        //     values to use for this method.
        //
        //   adjacencyIn:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the source mesh. If the edge has no adjacent faces, the
        //     value is 0. If this parameter is omitted, Microsoft.DirectX.Direct3D.BaseMesh.GenerateAdjacency(System.Single,System.Int32[])
        //     is called to create logical adjacency information.
        void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn);
        //
        // Summary:
        //     Welds together replicated vertices that have equal attributes.
        //
        // Parameters:
        //   flags:
        //     One or more flags from Microsoft.DirectX.Direct3D.WeldEpsilonsFlags.
        //
        //   epsilons:
        //     A Microsoft.DirectX.Direct3D.WeldEpsilons structure that specifies the epsilon
        //     values to use for this method.
        //
        //   adjacencyIn:
        //     A Microsoft.DirectX.GraphicsStream containing three System.Int32 values per
        //     face that specify the three neighbors for each face in the source mesh. If
        //     the edge has no adjacent faces, the value is 0. If this parameter is omitted,
        //     Microsoft.DirectX.Direct3D.BaseMesh.GenerateAdjacency(System.Single,System.Int32[])
        //     is called to create logical adjacency information.
        //
        //   adjacencyOut:
        //     [in, out] A Microsoft.DirectX.GraphicsStream that contains the face adjacency
        //     array of the optimized mesh. The face adjacency is stored as an array of
        //     arrays. The innermost array is three indices of adjacent triangles, and the
        //     outer array is one set of face adjacencies per triangle in the mesh.
        void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, GraphicsStream adjacencyIn, GraphicsStream adjacencyOut);
        //
        // Summary:
        //     Welds together replicated vertices that have equal attributes.
        //
        // Parameters:
        //   flags:
        //     One or more flags from Microsoft.DirectX.Direct3D.WeldEpsilonsFlags.
        //
        //   epsilons:
        //     A Microsoft.DirectX.Direct3D.WeldEpsilons structure that specifies the epsilon
        //     values to use for this method.
        //
        //   adjacencyIn:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the source mesh. If the edge has no adjacent faces, the
        //     value is 0. If this parameter is omitted, Microsoft.DirectX.Direct3D.BaseMesh.GenerateAdjacency(System.Single,System.Int32[])
        //     is called to create logical adjacency information.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream object that contains the new index for
        //     each vertex.
        void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn, out GraphicsStream vertexRemap);
        //
        // Summary:
        //     Welds together replicated vertices that have equal attributes.
        //
        // Parameters:
        //   flags:
        //     One or more flags from Microsoft.DirectX.Direct3D.WeldEpsilonsFlags.
        //
        //   epsilons:
        //     A Microsoft.DirectX.Direct3D.WeldEpsilons structure that specifies the epsilon
        //     values to use for this method.
        //
        //   adjacencyIn:
        //     A Microsoft.DirectX.GraphicsStream containing three System.Int32 values per
        //     face that specify the three neighbors for each face in the source mesh. If
        //     the edge has no adjacent faces, the value is 0. If this parameter is omitted,
        //     Microsoft.DirectX.Direct3D.BaseMesh.GenerateAdjacency(System.Single,System.Int32[])
        //     is called to create logical adjacency information.
        //
        //   adjacencyOut:
        //     [in, out] A Microsoft.DirectX.GraphicsStream that contains the face adjacency
        //     array of the optimized mesh. The face adjacency is stored as an array of
        //     arrays. The innermost array is three indices of adjacent triangles, and the
        //     outer array is one set of face adjacencies per triangle in the mesh.
        //
        //   faceRemap:
        //     Array that contains the new index for each face.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream object that contains the new index for
        //     each vertex.
        void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, GraphicsStream adjacencyIn, GraphicsStream adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap);
        //
        // Summary:
        //     Welds together replicated vertices that have equal attributes.
        //
        // Parameters:
        //   flags:
        //     One or more flags from Microsoft.DirectX.Direct3D.WeldEpsilonsFlags.
        //
        //   epsilons:
        //     A Microsoft.DirectX.Direct3D.WeldEpsilons structure that specifies the epsilon
        //     values to use for this method.
        //
        //   adjacencyIn:
        //     Array of three System.Int32 values per face that specify the three neighbors
        //     for each face in the source mesh. If the edge has no adjacent faces, the
        //     value is 0. If this parameter is omitted, Microsoft.DirectX.Direct3D.BaseMesh.GenerateAdjacency(System.Single,System.Int32[])
        //     is called to create logical adjacency information.
        //
        //   adjacencyOut:
        //     [in, out] Face adjacency array of the optimized mesh. The face adjacency
        //     is stored as an array of arrays. The innermost array is three indices of
        //     adjacent triangles, and the outermost array is one set of face adjacencies
        //     per triangle in the mesh.
        //
        //   faceRemap:
        //     Array that contains the new index for each face.
        //
        //   vertexRemap:
        //     A Microsoft.DirectX.GraphicsStream object that contains the new index for
        //     each vertex.
        void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap);
    }
}
