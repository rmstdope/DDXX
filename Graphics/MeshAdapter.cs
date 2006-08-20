using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class MeshAdapter : IMesh
    {
        private Mesh mesh;

        public MeshAdapter(Mesh mesh)
        {
            this.mesh = mesh;
        }

        public static MeshAdapter Box(Device device, float width, float height, float depth)
        {
            return new MeshAdapter(Mesh.Box(device, width, height, depth));
        }
        public static MeshAdapter Box(Device device, float width, float height, float depth, out GraphicsStream adjacency)
        {
            return new MeshAdapter(Mesh.Box(device, width, height, depth, out adjacency));
        }
        public static MeshAdapter Clean(CleanType cleanType, Mesh mesh, GraphicsStream adjacency, GraphicsStream adjacencyOut)
        {
            return new MeshAdapter(Mesh.Clean(cleanType, mesh, adjacency, adjacencyOut));
        }
        public static MeshAdapter Clean(CleanType cleanType, Mesh mesh, int[] adjacency, out int[] adjacencyOut)
        {
            return new MeshAdapter(Mesh.Clean(cleanType, mesh, adjacency, out adjacencyOut));
        }
        public static MeshAdapter Clean(CleanType cleanType, Mesh mesh, GraphicsStream adjacency, GraphicsStream adjacencyOut, out string errorsAndWarnings)
        {
            return new MeshAdapter(Mesh.Clean(cleanType, mesh, adjacency, adjacencyOut, out errorsAndWarnings));
        }
        public static MeshAdapter Clean(CleanType cleanType, Mesh mesh, int[] adjacency, out int[] adjacencyOut, out string errorsAndWarnings)
        {
            return new MeshAdapter(Mesh.Clean(cleanType, mesh, adjacency, out adjacencyOut, out errorsAndWarnings));
        }
        public static MeshAdapter Cylinder(Device device, float radius1, float radius2, float length, int slices, int stacks)
        {
            return new MeshAdapter(Mesh.Cylinder(device, radius1, radius2, length, slices, stacks));
        }
        public static MeshAdapter Cylinder(Device device, float radius1, float radius2, float length, int slices, int stacks, out GraphicsStream adjacency)
        {
            return new MeshAdapter(Mesh.Cylinder(device, radius1, radius2, length, slices, stacks, out adjacency));
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device)
        {
            return new MeshAdapter(Mesh.FromFile(filename, options, device));
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromFile(filename, options, device, out effects));
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out ExtendedMaterial[] materials)
        {
            return new MeshAdapter(Mesh.FromFile(filename, options, device, out materials));
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out GraphicsStream adjacency)
        {
            return new MeshAdapter(Mesh.FromFile(filename, options, device, out adjacency));
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromFile(filename, options, device, out materials, out effects));
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out GraphicsStream adjacency, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromFile(filename, options, device, out adjacency, out effects));
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out GraphicsStream adjacency, out ExtendedMaterial[] materials)
        {
            return new MeshAdapter(Mesh.FromFile(filename, options, device, out adjacency, out materials));
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out GraphicsStream adjacency, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromFile(filename, options, device, out adjacency, out materials, out effects));
        }
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device)
        {
            return new MeshAdapter(Mesh.FromStream(stream, options, device));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device));
        }
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromStream(stream, options, device, out effects));
        }
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device, out ExtendedMaterial[] materials)
        {
            return new MeshAdapter(Mesh.FromStream(stream, options, device, out materials));
        }
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device, out GraphicsStream adjacency)
        {
            return new MeshAdapter(Mesh.FromStream(stream, options, device, out adjacency));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out effects));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out ExtendedMaterial[] materials)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out materials));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out GraphicsStream adjacency)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out adjacency));
        }
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromStream(stream, options, device, out materials, out effects));
        }
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device, out GraphicsStream adjacency, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromStream(stream, options, device, out adjacency, out effects));
        }
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device, out GraphicsStream adjacency, out ExtendedMaterial[] materials)
        {
            return new MeshAdapter(Mesh.FromStream(stream, options, device, out adjacency, out materials));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out materials, out effects));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out GraphicsStream adjacency, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out adjacency, out effects));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out GraphicsStream adjacency, out ExtendedMaterial[] materials)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out adjacency, out materials));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out GraphicsStream adjacency, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out adjacency, out materials, out effects));
        }
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device)
        {
            return new MeshAdapter(Mesh.FromX(xofObjMesh, options, device));
        }
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromX(xofObjMesh, options, device, out effects));
        }
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device, out ExtendedMaterial[] materials)
        {
            return new MeshAdapter(Mesh.FromX(xofObjMesh, options, device, out materials));
        }
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device, out GraphicsStream adjacency)
        {
            return new MeshAdapter(Mesh.FromX(xofObjMesh, options, device, out adjacency));
        }
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromX(xofObjMesh, options, device, out materials, out effects));
        }
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device, out GraphicsStream adjacency, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromX(xofObjMesh, options, device, out adjacency, out effects));
        }
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device, out GraphicsStream adjacency, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromX(xofObjMesh, options, device, out adjacency, out materials, out effects));
        }
        public static MeshAdapter Polygon(Device device, float length, int sides)
        {
            return new MeshAdapter(Mesh.Polygon(device, length, sides));
        }
        public static MeshAdapter Polygon(Device device, float length, int sides, out GraphicsStream adjacency)
        {
            return new MeshAdapter(Mesh.Polygon(device, length, sides, out adjacency));
        }
        public static MeshAdapter Sphere(Device device, float radius, int slices, int stacks)
        {
            return new MeshAdapter(Mesh.Sphere(device, radius, slices, stacks));
        }
        public static MeshAdapter Sphere(Device device, float radius, int slices, int stacks, out GraphicsStream adjacency)
        {
            return new MeshAdapter(Mesh.Sphere(device, radius, slices, stacks, out adjacency));
        }
        public static MeshAdapter Teapot(Device device)
        {
            return new MeshAdapter(Mesh.Teapot(device));
        }
        public static MeshAdapter Teapot(Device device, out GraphicsStream adjacency)
        {
            return new MeshAdapter(Mesh.Teapot(device, out adjacency));
        }
        public static MeshAdapter Torus(Device device, float innerRadius, float outerRadius, int sides, int rings)
        {
            return new MeshAdapter(Mesh.Torus(device, innerRadius, outerRadius, sides, rings));
        }
        public static MeshAdapter Torus(Device device, float innerRadius, float outerRadius, int sides, int rings, out GraphicsStream adjacency)
        {
            return new MeshAdapter(Mesh.Torus(device, innerRadius, outerRadius, sides, rings, out adjacency));
        }

        #region IMesh Members

        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap)
        {
            mesh.ComputeTangent(texStage, tangentIndex, binormIndex, wrap);
        }

        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap, GraphicsStream adjacency)
        {
            mesh.ComputeTangent(texStage, tangentIndex, binormIndex, wrap, adjacency);
        }

        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap, int[] adjacency)
        {
            mesh.ComputeTangent(texStage, tangentIndex, binormIndex, wrap, adjacency);
        }

        public void ComputeTangentFrame(TangentOptions options)
        {
            mesh.ComputeTangentFrame(options);
        }

        public bool Intersect(Vector3 rayPos, Vector3 rayDir)
        {
            return mesh.Intersect(rayPos, rayDir);
        }

        public bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit)
        {
            return mesh.Intersect(rayPos, rayDir, out closestHit);
        }

        public bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation[] allHits)
        {
            return mesh.Intersect(rayPos, rayDir, out allHits);
        }

        public bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit, out IntersectInformation[] allHits)
        {
            return mesh.Intersect(rayPos, rayDir, out closestHit, out allHits);
        }

        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir)
        {
            return mesh.IntersectSubset(attributeId, rayPos, rayDir);
        }

        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit)
        {
            return mesh.IntersectSubset(attributeId, rayPos, rayDir, out closestHit);
        }

        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation[] allHits)
        {
            return mesh.IntersectSubset(attributeId, rayPos, rayDir, out allHits);
        }

        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit, out IntersectInformation[] allHits)
        {
            return mesh.IntersectSubset(attributeId, rayPos, rayDir, out closestHit, out allHits);
        }

        public GraphicsStream LockAttributeBuffer(LockFlags flags)
        {
            return mesh.LockAttributeBuffer(flags);
        }

        public int[] LockAttributeBufferArray(LockFlags flags)
        {
            return mesh.LockAttributeBufferArray(flags);
        }

        public Mesh Optimize(MeshFlags flags, GraphicsStream adjacencyIn)
        {
            return mesh.Optimize(flags, adjacencyIn);
        }

        public Mesh Optimize(MeshFlags flags, int[] adjacencyIn)
        {
            return mesh.Optimize(flags, adjacencyIn);
        }

        public Mesh Optimize(MeshFlags flags, GraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap)
        {
            return mesh.Optimize(flags, adjacencyIn, out adjacencyOut, out faceRemap, out vertexRemap);
        }

        public Mesh Optimize(MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap)
        {
            return mesh.Optimize(flags, adjacencyIn, out adjacencyOut, out faceRemap, out vertexRemap);
        }

        public void OptimizeInPlace(MeshFlags flags, GraphicsStream adjacencyIn)
        {
            mesh.OptimizeInPlace(flags, adjacencyIn);
        }

        public void OptimizeInPlace(MeshFlags flags, int[] adjacencyIn)
        {
            mesh.OptimizeInPlace(flags, adjacencyIn);
        }

        public void OptimizeInPlace(MeshFlags flags, GraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap)
        {
            mesh.OptimizeInPlace(flags, adjacencyIn, out adjacencyOut, out faceRemap, out vertexRemap);
        }

        public void OptimizeInPlace(MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap)
        {
            mesh.OptimizeInPlace(flags, adjacencyIn, out adjacencyOut, out faceRemap, out vertexRemap);
        }

        public void Save(Stream stream, GraphicsStream adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            mesh.Save(stream, adjacency, materials, format);
        }

        public void Save(Stream stream, int[] adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            mesh.Save(stream, adjacency, materials, format);
        }

        public void Save(string filename, GraphicsStream adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            mesh.Save(filename, adjacency, materials, format);
        }

        public void Save(string filename, int[] adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            mesh.Save(filename, adjacency, materials, format);
        }

        public void Save(Stream stream, GraphicsStream adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            mesh.Save(stream, adjacency, materials, effects, format);
        }

        public void Save(Stream stream, int[] adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            mesh.Save(stream, adjacency, materials, effects, format);
        }

        public void Save(string filename, GraphicsStream adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            mesh.Save(filename, adjacency, materials, effects, format);
        }

        public void Save(string filename, int[] adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            mesh.Save(filename, adjacency, materials, effects, format);
        }

        public void SetAttributeTable(AttributeRange[] table)
        {
            mesh.SetAttributeTable(table);
        }

        public void UnlockAttributeBuffer()
        {
            mesh.UnlockAttributeBuffer();
        }

        public void UnlockAttributeBuffer(int[] dataAttribute)
        {
            mesh.UnlockAttributeBuffer(dataAttribute);
        }

        public void Validate(GraphicsStream adjacency)
        {
            mesh.Validate(adjacency);
        }

        public void Validate(int[] adjacency)
        {
            mesh.Validate(adjacency);
        }

        public void Validate(GraphicsStream adjacency, out string errorsAndWarnings)
        {
            mesh.Validate(adjacency, out errorsAndWarnings);
        }

        public void Validate(int[] adjacency, out string errorsAndWarnings)
        {
            mesh.Validate(adjacency, out errorsAndWarnings);
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn)
        {
            mesh.WeldVertices(flags, epsilons, adjacencyIn);
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, GraphicsStream adjacencyIn, GraphicsStream adjacencyOut)
        {
            mesh.WeldVertices(flags, epsilons, adjacencyIn, adjacencyOut);
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn, out GraphicsStream vertexRemap)
        {
            mesh.WeldVertices(flags, epsilons, adjacencyIn, out vertexRemap);
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, GraphicsStream adjacencyIn, GraphicsStream adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap)
        {
            mesh.WeldVertices(flags, epsilons, adjacencyIn, adjacencyOut, out faceRemap, out vertexRemap);
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap)
        {
            mesh.WeldVertices(flags, epsilons, adjacencyIn, out adjacencyOut, out faceRemap, out vertexRemap);
        }

        public VertexElement[] Declaration
        {
            get { return mesh.Declaration; }
        }

        public Device Device
        {
            get { return mesh.Device; }
        }

        public bool Disposed
        {
            get { return mesh.Disposed; }
        }

        public void Dispose()
        {
            mesh.Dispose();
        }

        public IndexBuffer IndexBuffer
        {
            get { return mesh.IndexBuffer; }
        }

        public int NumberAttributes
        {
            get { return mesh.NumberAttributes; }
        }

        public int NumberBytesPerVertex
        {
            get { return mesh.NumberBytesPerVertex; }
        }

        public int NumberFaces
        {
            get { return mesh.NumberFaces; }
        }

        public int NumberVertices
        {
            get { return mesh.NumberVertices; }
        }

        public MeshOptions Options
        {
            get { return mesh.Options; }
        }

        public VertexBuffer VertexBuffer
        {
            get { return mesh.VertexBuffer; }
        }

        public VertexFormats VertexFormat
        {
            get { return mesh.VertexFormat; }
        }

        public IMesh Clone(MeshFlags options, GraphicsStream declaration, IDevice device)
        {
            return new MeshAdapter(mesh.Clone(options, declaration, ((DeviceAdapter)device).DXDevice));
        }

        public IMesh Clone(MeshFlags options, VertexElement[] declaration, IDevice device)
        {
            return new MeshAdapter(mesh.Clone(options, declaration, ((DeviceAdapter)device).DXDevice));
        }

        public IMesh Clone(MeshFlags options, VertexFormats vertexFormat, IDevice device)
        {
            return new MeshAdapter(mesh.Clone(options, vertexFormat, ((DeviceAdapter)device).DXDevice));
        }

        public void ComputeNormals()
        {
            mesh.ComputeNormals();
        }

        public void ComputeNormals(GraphicsStream adjacency)
        {
            mesh.ComputeNormals(adjacency);
        }

        public void ComputeNormals(int[] adjacency)
        {
            mesh.ComputeNormals(adjacency);
        }

        public int[] ConvertAdjacencyToPointReps(GraphicsStream adjacency)
        {
            return mesh.ConvertAdjacencyToPointReps(adjacency);
        }

        public int[] ConvertAdjacencyToPointReps(int[] adjaceny)
        {
            return mesh.ConvertAdjacencyToPointReps(adjaceny);
        }

        public int[] ConvertPointRepsToAdjacency(GraphicsStream pointReps)
        {
            return mesh.ConvertPointRepsToAdjacency(pointReps);
        }

        public int[] ConvertPointRepsToAdjacency(int[] pointReps)
        {
            return mesh.ConvertPointRepsToAdjacency(pointReps);
        }

        public void DrawSubset(int attributeID)
        {
            mesh.DrawSubset(attributeID);
        }

        public void GenerateAdjacency(float epsilon, int[] adjacency)
        {
            mesh.GenerateAdjacency(epsilon, adjacency);
        }

        public AttributeRange[] GetAttributeTable()
        {
            return mesh.GetAttributeTable();
        }

        public GraphicsStream LockIndexBuffer(LockFlags flags)
        {
            return mesh.LockIndexBuffer(flags);
        }

        public Array LockIndexBuffer(Type typeIndex, LockFlags flags, params int[] ranks)
        {
            return mesh.LockIndexBuffer(typeIndex, flags, ranks);
        }

        public GraphicsStream LockVertexBuffer(LockFlags flags)
        {
            return mesh.LockVertexBuffer(flags);
        }

        public Array LockVertexBuffer(Type typeVertex, LockFlags flags, params int[] ranks)
        {
            return mesh.LockVertexBuffer(typeVertex, flags, ranks);
        }

        public void SetIndexBufferData(object data, LockFlags flags)
        {
            mesh.SetIndexBufferData(data, flags);
        }

        public void SetVertexBufferData(object data, LockFlags flags)
        {
            
            mesh.SetVertexBufferData(data, flags);
        }

        public void UnlockIndexBuffer()
        {
            mesh.UnlockIndexBuffer();
        }

        public void UnlockVertexBuffer()
        {
            mesh.UnlockVertexBuffer();
        }

        public void UpdateSemantics(GraphicsStream declaration)
        {
            mesh.UpdateSemantics(declaration);
        }

        public void UpdateSemantics(VertexElement[] declaration)
        {
            mesh.UpdateSemantics(declaration);
        }

        #endregion
    }
}
