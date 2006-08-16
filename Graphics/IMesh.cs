using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Graphics
{
    public interface IMesh
    {
        // Summary:
        //     Retrieves a declaration that describes the vertices in a mesh.
        VertexElement[] Declaration { get; }
        //
        // Summary:
        //     Retrieves the device associated with a mesh.
        Device Device { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the object is disposed.
        bool Disposed { get; }
        //
        // Summary:
        //     Retrieves the data in an index buffer.
        IndexBuffer IndexBuffer { get; }
        //
        // Summary:
        //     Retrieves the number of entries stored in an attribute table for a mesh.
        int NumberAttributes { get; }
        //
        // Summary:
        //     Retrieves the number of bytes per vertex.
        int NumberBytesPerVertex { get; }
        //
        // Summary:
        //     Retrieves the number of faces in a mesh.
        int NumberFaces { get; }
        //
        // Summary:
        //     Retrieves the number of vertices in a mesh.
        int NumberVertices { get; }
        //
        // Summary:
        //     Retrieves the mesh options enabled for the current mesh at creation time.
        MeshOptions Options { get; }
        //
        // Summary:
        //     Retrieves the vertex buffer of a mesh.
        VertexBuffer VertexBuffer { get; }
        //
        // Summary:
        //     Retrieves the vertex format that describes the contents of vertices.
        VertexFormats VertexFormat { get; }
        // Summary:
        //     Clones, or copies, a mesh object.
        //
        // Parameters:
        //   options:
        //     Mesh creation options, as specified with one or more Microsoft.DirectX.Direct3D.MeshFlags
        //     flags (excepting the Microsoft.DirectX.Direct3D.MeshFlags and Microsoft.DirectX.Direct3D.MeshFlags
        //     flags, which should not be used for this purpose).
        //
        //   declaration:
        //     A Microsoft.DirectX.GraphicsStream object that contains the mesh data to
        //     duplicate.
        //
        //   device:
        //     The Microsoft.DirectX.Direct3D.Device object associated with the mesh.
        //
        // Returns:
        //     Cloned mesh.
        Mesh Clone(MeshFlags options, GraphicsStream declaration, Device device);
        //
        // Summary:
        //     Clones, or copies, a mesh object.
        //
        // Parameters:
        //   options:
        //     Mesh creation options, as specified with one or more Microsoft.DirectX.Direct3D.MeshFlags
        //     flags (excepting the Microsoft.DirectX.Direct3D.MeshFlags and Microsoft.DirectX.Direct3D.MeshFlags
        //     flags, which should not be used for this purpose).
        //
        //   declaration:
        //     Vertex data, as defined with an array of Microsoft.DirectX.Direct3D.VertexElement
        //     structures.
        //
        //   device:
        //     The Microsoft.DirectX.Direct3D.Device object associated with the mesh.
        //
        // Returns:
        //     Cloned mesh.
        Mesh Clone(MeshFlags options, VertexElement[] declaration, Device device);
        //
        // Summary:
        //     Clones, or copies, a mesh object.
        //
        // Parameters:
        //   options:
        //     Mesh creation options, as specified with one or more Microsoft.DirectX.Direct3D.MeshFlags
        //     flags (excepting the Microsoft.DirectX.Direct3D.MeshFlags and Microsoft.DirectX.Direct3D.MeshFlags
        //     flags, which should not be used for this purpose).
        //
        //   vertexFormat:
        //     Mesh vertex format, as specified with one or more Microsoft.DirectX.Direct3D.VertexFormats
        //     flags.
        //
        //   device:
        //     The Microsoft.DirectX.Direct3D.Device object associated with the mesh.
        //
        // Returns:
        //     Cloned mesh.
        Mesh Clone(MeshFlags options, VertexFormats vertexFormat, Device device);
        //
        // Summary:
        //     Computes normals for each vertex in a mesh.
        void ComputeNormals();
        //
        // Summary:
        //     Computes normals for each vertex in a mesh.
        //
        // Parameters:
        //   adjacency:
        //     A Microsoft.DirectX.GraphicsStream that contains three System.Int32 values
        //     per face, which specify the three neighbors for each face in the mesh.
        void ComputeNormals(GraphicsStream adjacency);
        //
        // Summary:
        //     Computes normals for each vertex in a mesh.
        //
        // Parameters:
        //   adjacency:
        //     Array of three System.Int32 values per face, which specify the three neighbors
        //     for each face in the mesh.
        void ComputeNormals(int[] adjacency);
        //
        // Summary:
        //     Converts mesh adjacency information to an array of point representatives.
        //
        // Parameters:
        //   adjacency:
        //     A Microsoft.DirectX.GraphicsStream that contains three System.Int32 values
        //     per face, which specify the three neighbors for each face in the mesh.
        //
        // Returns:
        //     Array that contains one System.Int32 per vertex of the mesh that is filled
        //     with point representative data.
        int[] ConvertAdjacencyToPointReps(GraphicsStream adjacency);
        //
        // Summary:
        //     Converts mesh adjacency information to an array of point representatives.
        //
        // Parameters:
        //   adjaceny:
        //     Array of three System.Int32 values per face, which specify the three neighbors
        //     for each face in the mesh.
        //
        // Returns:
        //     Array that contains one System.Int32 per vertex of the mesh that is filled
        //     with point representative data.
        int[] ConvertAdjacencyToPointReps(int[] adjaceny);
        //
        // Summary:
        //     Converts point representative data to mesh adjacency information.
        //
        // Parameters:
        //   pointReps:
        //     A Microsoft.DirectX.GraphicsStream that contains one System.Int32 per vertex
        //     of the mesh that represents point representative data. This parameter is
        //     optional; supplying a value of 0 causes it to be interpreted as an "identity"
        //     array.
        //
        // Returns:
        //     Array of three System.Int32 values per face, which specify the three neighbors
        //     for each face in the mesh.
        int[] ConvertPointRepsToAdjacency(GraphicsStream pointReps);
        //
        // Summary:
        //     Converts point representative data to mesh adjacency information.
        //
        // Parameters:
        //   pointReps:
        //     Array that contains one System.Int32 per vertex of the mesh that contains
        //     point representative data. This parameter is optional; supplying a value
        //     of 0 causes it to be interpreted as an "identity" array.
        //
        // Returns:
        //     Array of three System.Int32 values per face, which specify the three neighbors
        //     for each face in the mesh.
        int[] ConvertPointRepsToAdjacency(int[] pointReps);
        //
        // Summary:
        //     Draws a subset of a mesh.
        //
        // Parameters:
        //   attributeID:
        //     An System.Int32 value that specifies which subset of the mesh to draw. This
        //     value is used to differentiate faces in a mesh as belonging to one or more
        //     attribute groups.
        void DrawSubset(int attributeID);
        //
        // Summary:
        //     Generates adjacency information based on mesh indices.
        //
        // Parameters:
        //   epsilon:
        //     Specifies that vertices that differ in position by less than epsilon should
        //     be treated as coincident.
        //
        //   adjacency:
        //     Array of three System.Int32 values per face to be filled with adjacent face
        //     indices. The size of this array must be at least 3 * Microsoft.DirectX.Direct3D.BaseMesh.NumberFaces.
        void GenerateAdjacency(float epsilon, int[] adjacency);
        //
        // Summary:
        //     Retrieves an attribute table for a mesh.
        //
        // Returns:
        //     Array of Microsoft.DirectX.Direct3D.AttributeRange structures, which represent
        //     each entry in the attribute table.
        AttributeRange[] GetAttributeTable();
        //
        // Summary:
        //     Locks an index buffer and obtains the index buffer data.
        //
        // Parameters:
        //   flags:
        //     Combination of zero or more Microsoft.DirectX.Direct3D.LockFlags that describe
        //     the type of lock to perform. For this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that represents the locked index
        //     buffer.
        GraphicsStream LockIndexBuffer(LockFlags flags);
        //
        // Summary:
        //     Locks an index buffer and obtains the index buffer data.
        //
        // Parameters:
        //   typeIndex:
        //     A System.Type object that indicates the type of array data to return. This
        //     can be a value type or any type that contains only value types.
        //
        //   flags:
        //     Combination of zero or more Microsoft.DirectX.Direct3D.LockFlags that describe
        //     the type of lock to perform. For this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly. For a description of the
        //     flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returning System.Array.
        //
        // Returns:
        //     An System.Array that represents the locked index buffer.
        Array LockIndexBuffer(Type typeIndex, LockFlags flags, params int[] ranks);
        //
        // Summary:
        //     Locks a vertex buffer and obtains a pointer to the vertex buffer memory.
        //
        // Parameters:
        //   flags:
        //     Combination of zero or more Microsoft.DirectX.Direct3D.LockFlags that describe
        //     the type of lock to perform. For this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     Microsoft.DirectX.Direct3D.LockFlags.ReadOnly, and Microsoft.DirectX.Direct3D.LockFlags.
        //     For a description of the flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        // Returns:
        //     A Microsoft.DirectX.GraphicsStream object that represents the locked vertex
        //     buffer.
        GraphicsStream LockVertexBuffer(LockFlags flags);
        //
        // Summary:
        //     Locks a vertex buffer and obtains a pointer to the vertex buffer memory.
        //
        // Parameters:
        //   typeVertex:
        //     A System.Type object that indicates the type of array data to return. This
        //     can be a value type or any type that contains only value types.
        //
        //   flags:
        //     Combination of zero or more Microsoft.DirectX.Direct3D.LockFlags that describe
        //     the type of lock to perform. For this method, the valid flags are Microsoft.DirectX.Direct3D.LockFlags.Discard,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate, Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock,
        //     Microsoft.DirectX.Direct3D.LockFlags.ReadOnly, and Microsoft.DirectX.Direct3D.LockFlags.
        //     For a description of the flags, see Microsoft.DirectX.Direct3D.LockFlags.
        //
        //   ranks:
        //     Array of one to three System.Int32 values that indicate the dimensions of
        //     the returning System.Array.
        //
        // Returns:
        //     An System.Array that represents the locked vertex buffer.
        Array LockVertexBuffer(Type typeVertex, LockFlags flags, params int[] ranks);
        //
        // Summary:
        //     Sets index buffer data.
        //
        // Parameters:
        //   data:
        //     An System.Object that contains the data to copy into the index buffer. This
        //     can be any value type or array of value types. The System.Int32 and System.Int16
        //     values are commonly used.
        //
        //   flags:
        //     Combination of zero or more Microsoft.DirectX.Direct3D.LockFlags that describe
        //     the type of lock to perform when setting the data. For this method, the valid
        //     flags are Microsoft.DirectX.Direct3D.LockFlags.Discard, Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock, and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly.
        //     For a description of the flags, see Microsoft.DirectX.Direct3D.LockFlags.
        void SetIndexBufferData(object data, LockFlags flags);
        //
        // Summary:
        //     Sets vertex buffer data.
        //
        // Parameters:
        //   data:
        //     An System.Object that contains the data to copy into the vertex buffer. This
        //     can be any value type or array of value types. The System.Int32 and System.Int16
        //     values are commonly used.
        //
        //   flags:
        //     Combination of zero or more Microsoft.DirectX.Direct3D.LockFlags that describe
        //     the type of lock to perform when setting the data. For this method, the valid
        //     flags are Microsoft.DirectX.Direct3D.LockFlags.Discard, Microsoft.DirectX.Direct3D.LockFlags.NoDirtyUpdate,
        //     Microsoft.DirectX.Direct3D.LockFlags.NoSystemLock, and Microsoft.DirectX.Direct3D.LockFlags.ReadOnly.
        //     For a description of the flags, see Microsoft.DirectX.Direct3D.LockFlags.
        void SetVertexBufferData(object data, LockFlags flags);
        //
        // Summary:
        //     Unlocks an index buffer.
        void UnlockIndexBuffer();
        //
        // Summary:
        //     Unlocks a vertex buffer.
        void UnlockVertexBuffer();
        //
        // Summary:
        //     Allows the user to change the mesh declaration without changing the data
        //     layout of the vertex buffer.  The call is valid only if the old and new declaration
        //     formats have the same vertex size.
        //
        // Parameters:
        //   declaration:
        //     A Microsoft.DirectX.GraphicsStream containing Microsoft.DirectX.Direct3D.VertexElement
        //     structures that describe the vertex format of the mesh vertices.
        void UpdateSemantics(GraphicsStream declaration);
        //
        // Summary:
        //     Allows the user to change the mesh declaration without changing the data
        //     layout of the vertex buffer.  The call is valid only if the old and new declaration
        //     formats have the same vertex size.
        //
        // Parameters:
        //   declaration:
        //     Array of Microsoft.DirectX.Direct3D.VertexElement structures that describe
        //     the vertex format of the mesh vertices.
        void UpdateSemantics(VertexElement[] declaration);

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
