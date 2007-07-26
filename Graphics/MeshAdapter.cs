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

        public Mesh DXMesh
        {
            get { return mesh; }
        }

        public static MeshAdapter Box(Device device, float width, float height, float depth)
        {
            return new MeshAdapter(Mesh.Box(device, width, height, depth));
        }
        public static MeshAdapter Box(Device device, float width, float height, float depth, out IGraphicsStream adjacency)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.Box(device, width, height, depth, out realAdjacency));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter Cylinder(Device device, float radius1, float radius2, float length, int slices, int stacks)
        {
            return new MeshAdapter(Mesh.Cylinder(device, radius1, radius2, length, slices, stacks));
        }
        public static MeshAdapter Cylinder(Device device, float radius1, float radius2, float length, int slices, int stacks, out IGraphicsStream adjacency)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.Cylinder(device, radius1, radius2, length, slices, stacks, out realAdjacency));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
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
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out IGraphicsStream adjacency)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromFile(filename, options, device, out realAdjacency));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromFile(filename, options, device, out materials, out effects));
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out IGraphicsStream adjacency, out EffectInstance[] effects)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromFile(filename, options, device, out realAdjacency, out effects));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out IGraphicsStream adjacency, out ExtendedMaterial[] materials)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromFile(filename, options, device, out realAdjacency, out materials));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromFile(string filename, MeshFlags options, Device device, out IGraphicsStream adjacency, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromFile(filename, options, device, out realAdjacency, out materials, out effects));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
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
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device, out IGraphicsStream adjacency)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromStream(stream, options, device, out realAdjacency));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out effects));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out ExtendedMaterial[] materials)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out materials));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out IGraphicsStream adjacency)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out realAdjacency));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromStream(stream, options, device, out materials, out effects));
        }
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device, out IGraphicsStream adjacency, out EffectInstance[] effects)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromStream(stream, options, device, out realAdjacency, out effects));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromStream(Stream stream, MeshFlags options, Device device, out IGraphicsStream adjacency, out ExtendedMaterial[] materials)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromStream(stream, options, device, out realAdjacency, out materials));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out materials, out effects));
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out IGraphicsStream adjacency, out EffectInstance[] effects)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out realAdjacency, out effects));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out IGraphicsStream adjacency, out ExtendedMaterial[] materials)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out realAdjacency, out materials));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromStream(Stream stream, int readBytes, MeshFlags options, Device device, out IGraphicsStream adjacency, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromStream(stream, readBytes, options, device, out realAdjacency, out materials, out effects));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
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
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device, out IGraphicsStream adjacency)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromX(xofObjMesh, options, device, out realAdjacency));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            return new MeshAdapter(Mesh.FromX(xofObjMesh, options, device, out materials, out effects));
        }
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device, out IGraphicsStream adjacency, out EffectInstance[] effects)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromX(xofObjMesh, options, device, out realAdjacency, out effects));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter FromX(XFileData xofObjMesh, MeshFlags options, Device device, out IGraphicsStream adjacency, out ExtendedMaterial[] materials, out EffectInstance[] effects)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.FromX(xofObjMesh, options, device, out realAdjacency, out materials, out effects));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter Polygon(Device device, float length, int sides)
        {
            return new MeshAdapter(Mesh.Polygon(device, length, sides));
        }
        public static MeshAdapter Polygon(Device device, float length, int sides, out IGraphicsStream adjacency)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.Polygon(device, length, sides, out realAdjacency));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter Sphere(Device device, float radius, int slices, int stacks)
        {
            return new MeshAdapter(Mesh.Sphere(device, radius, slices, stacks));
        }
        public static MeshAdapter Sphere(Device device, float radius, int slices, int stacks, out IGraphicsStream adjacency)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.Sphere(device, radius, slices, stacks, out realAdjacency));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter Teapot(Device device)
        {
            return new MeshAdapter(Mesh.Teapot(device));
        }
        public static MeshAdapter Teapot(Device device, out IGraphicsStream adjacency)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.Teapot(device, out realAdjacency));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }
        public static MeshAdapter Torus(Device device, float innerRadius, float outerRadius, int sides, int rings)
        {
            return new MeshAdapter(Mesh.Torus(device, innerRadius, outerRadius, sides, rings));
        }
        public static MeshAdapter Torus(Device device, float innerRadius, float outerRadius, int sides, int rings, out IGraphicsStream adjacency)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(Mesh.Torus(device, innerRadius, outerRadius, sides, rings, out realAdjacency));
            adjacency = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }

        #region Mesh Members

        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap)
        {
            mesh.ComputeTangent(texStage, tangentIndex, binormIndex, wrap);
        }

        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap, IGraphicsStream adjacency)
        {
            mesh.ComputeTangent(texStage, tangentIndex, binormIndex, wrap, ((GraphicsStreamAdapter)adjacency).DXGraphicsStream);
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

        public IGraphicsStream LockAttributeBuffer(LockFlags flags)
        {
            return new GraphicsStreamAdapter(mesh.LockAttributeBuffer(flags));
        }

        public int[] LockAttributeBufferArray(LockFlags flags)
        {
            return mesh.LockAttributeBufferArray(flags);
        }

        public IMesh Optimize(MeshFlags flags, IGraphicsStream adjacencyIn)
        {
            return new MeshAdapter(mesh.Optimize(flags, ((GraphicsStreamAdapter)adjacencyIn).DXGraphicsStream));
        }

        public IMesh Optimize(MeshFlags flags, int[] adjacencyIn)
        {
            return new MeshAdapter(mesh.Optimize(flags, adjacencyIn));
        }

        public IMesh Optimize(MeshFlags flags, IGraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(mesh.Optimize(flags, ((GraphicsStreamAdapter)adjacencyIn).DXGraphicsStream, out adjacencyOut, out faceRemap, out realAdjacency));
            vertexRemap = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }

        public IMesh Optimize(MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            GraphicsStream realAdjacency;
            MeshAdapter adapter = new MeshAdapter(mesh.Optimize(flags, adjacencyIn, out adjacencyOut, out faceRemap, out realAdjacency));
            vertexRemap = new GraphicsStreamAdapter(realAdjacency);
            return adapter;
        }

        public void OptimizeInPlace(MeshFlags flags, IGraphicsStream adjacencyIn)
        {
            mesh.OptimizeInPlace(flags, ((GraphicsStreamAdapter)adjacencyIn).DXGraphicsStream);
        }

        public void OptimizeInPlace(MeshFlags flags, int[] adjacencyIn)
        {
            mesh.OptimizeInPlace(flags, adjacencyIn);
        }

        public void OptimizeInPlace(MeshFlags flags, IGraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            GraphicsStream realAdjacency;
            mesh.OptimizeInPlace(flags, ((GraphicsStreamAdapter)adjacencyIn).DXGraphicsStream, out adjacencyOut, out faceRemap, out realAdjacency);
            vertexRemap = new GraphicsStreamAdapter(realAdjacency);
        }

        public void OptimizeInPlace(MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            GraphicsStream realAdjacency;
            mesh.OptimizeInPlace(flags, adjacencyIn, out adjacencyOut, out faceRemap, out realAdjacency);
            vertexRemap = new GraphicsStreamAdapter(realAdjacency);
        }

        public void Save(Stream stream, IGraphicsStream adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            mesh.Save(stream, ((GraphicsStreamAdapter)adjacency).DXGraphicsStream, materials, format);
        }

        public void Save(Stream stream, int[] adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            mesh.Save(stream, adjacency, materials, format);
        }

        public void Save(string filename, IGraphicsStream adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            mesh.Save(filename, ((GraphicsStreamAdapter)adjacency).DXGraphicsStream, materials, format);
        }

        public void Save(string filename, int[] adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            mesh.Save(filename, adjacency, materials, format);
        }

        public void Save(Stream stream, IGraphicsStream adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            mesh.Save(stream, ((GraphicsStreamAdapter)adjacency).DXGraphicsStream, materials, effects, format);
        }

        public void Save(Stream stream, int[] adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            mesh.Save(stream, adjacency, materials, effects, format);
        }

        public void Save(string filename, IGraphicsStream adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            mesh.Save(filename, ((GraphicsStreamAdapter)adjacency).DXGraphicsStream, materials, effects, format);
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

        public void Validate(IGraphicsStream adjacency)
        {
            mesh.Validate(((GraphicsStreamAdapter)adjacency).DXGraphicsStream);
        }

        public void Validate(int[] adjacency)
        {
            mesh.Validate(adjacency);
        }

        public void Validate(IGraphicsStream adjacency, out string errorsAndWarnings)
        {
            mesh.Validate(((GraphicsStreamAdapter)adjacency).DXGraphicsStream, out errorsAndWarnings);
        }

        public void Validate(int[] adjacency, out string errorsAndWarnings)
        {
            mesh.Validate(adjacency, out errorsAndWarnings);
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn)
        {
            mesh.WeldVertices(flags, epsilons, adjacencyIn);
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, IGraphicsStream adjacencyIn, IGraphicsStream adjacencyOut)
        {
            mesh.WeldVertices(flags, epsilons, ((GraphicsStreamAdapter)adjacencyIn).DXGraphicsStream, ((GraphicsStreamAdapter)adjacencyOut).DXGraphicsStream);
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn, out IGraphicsStream vertexRemap)
        {
            GraphicsStream realAdjacency;
            mesh.WeldVertices(flags, epsilons, adjacencyIn, out realAdjacency);
            vertexRemap = new GraphicsStreamAdapter(realAdjacency);
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, IGraphicsStream adjacencyIn, IGraphicsStream adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            GraphicsStream realAdjacency;
            mesh.WeldVertices(flags, epsilons, ((GraphicsStreamAdapter)adjacencyIn).DXGraphicsStream, ((GraphicsStreamAdapter)adjacencyOut).DXGraphicsStream, out faceRemap, out realAdjacency);
            vertexRemap = new GraphicsStreamAdapter(realAdjacency);
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            GraphicsStream realAdjacency;
            mesh.WeldVertices(flags, epsilons, adjacencyIn, out adjacencyOut, out faceRemap, out realAdjacency);
            vertexRemap = new GraphicsStreamAdapter(realAdjacency);
        }

        public VertexElement[] Declaration
        {
            get { return mesh.Declaration; }
        }

        public IDevice Device
        {
            get { return new DeviceAdapter(mesh.Device); }
        }

        public bool Disposed
        {
            get { return mesh.Disposed; }
        }

        public void Dispose()
        {
            mesh.Dispose();
        }

        public IIndexBuffer IndexBuffer
        {
            get { return new IndexBufferAdapter(mesh.IndexBuffer); }
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

        public IVertexBuffer VertexBuffer
        {
            get { return new VertexBufferAdapter(mesh.VertexBuffer); }
        }

        public VertexFormats VertexFormat
        {
            get { return mesh.VertexFormat; }
        }

        public IMesh Clean(CleanType cleanType, IGraphicsStream adjacency, IGraphicsStream adjacencyOut)
        {
            Mesh mesh = Mesh.Clean(cleanType, this.mesh, ((GraphicsStreamAdapter)adjacency).DXGraphicsStream, ((GraphicsStreamAdapter)adjacencyOut).DXGraphicsStream);
            if (mesh == this.mesh)
                return this;
            else
                return new MeshAdapter(mesh);
        }

        public IMesh Clean(CleanType cleanType, int[] adjacency, out int[] adjacencyOut)
        {
            Mesh mesh = Mesh.Clean(cleanType, this.mesh, adjacency, out adjacencyOut);
            if (mesh == this.mesh)
                return this;
            else
                return new MeshAdapter(mesh);
        }

        public IMesh Clean(CleanType cleanType, IGraphicsStream adjacency, IGraphicsStream adjacencyOut, out string errorsAndWarnings)
        {
            Mesh mesh = Mesh.Clean(cleanType, this.mesh, ((GraphicsStreamAdapter)adjacency).DXGraphicsStream, ((GraphicsStreamAdapter)adjacency).DXGraphicsStream, out errorsAndWarnings);
            if (mesh == this.mesh)
                return this;
            else
                return new MeshAdapter(mesh);
        }

        public IMesh Clean(CleanType cleanType, int[] adjacency, out int[] adjacencyOut, out string errorsAndWarnings)
        {
            Mesh mesh = Mesh.Clean(cleanType, this.mesh, adjacency, out adjacencyOut, out errorsAndWarnings);
            if (mesh == this.mesh)
                return this;
            else
                return new MeshAdapter(mesh);
        }

        public IMesh Clone(MeshFlags options, IGraphicsStream declaration, IDevice device)
        {
            return new MeshAdapter(mesh.Clone(options, ((GraphicsStreamAdapter)declaration).DXGraphicsStream, ((DeviceAdapter)device).DXDevice));
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

        public void ComputeNormals(IGraphicsStream adjacency)
        {
            mesh.ComputeNormals(((GraphicsStreamAdapter)adjacency).DXGraphicsStream);
        }

        public void ComputeNormals(int[] adjacency)
        {
            mesh.ComputeNormals(adjacency);
        }

        public int[] ConvertAdjacencyToPointReps(IGraphicsStream adjacency)
        {
            return mesh.ConvertAdjacencyToPointReps(((GraphicsStreamAdapter)adjacency).DXGraphicsStream);
        }

        public int[] ConvertAdjacencyToPointReps(int[] adjaceny)
        {
            return mesh.ConvertAdjacencyToPointReps(adjaceny);
        }

        public int[] ConvertPointRepsToAdjacency(IGraphicsStream pointReps)
        {
            return mesh.ConvertPointRepsToAdjacency(((GraphicsStreamAdapter)pointReps).DXGraphicsStream);
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

        public IGraphicsStream LockIndexBuffer(LockFlags flags)
        {
            return new GraphicsStreamAdapter(mesh.LockIndexBuffer(flags));
        }

        public Array LockIndexBuffer(Type typeIndex, LockFlags flags, params int[] ranks)
        {
            return mesh.LockIndexBuffer(typeIndex, flags, ranks);
        }

        public IGraphicsStream LockVertexBuffer(LockFlags flags)
        {
            return new GraphicsStreamAdapter(mesh.LockVertexBuffer(flags));
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

        public void UpdateSemantics(IGraphicsStream declaration)
        {
            mesh.UpdateSemantics(((GraphicsStreamAdapter)declaration).DXGraphicsStream);
        }

        public void UpdateSemantics(VertexElement[] declaration)
        {
            mesh.UpdateSemantics(declaration);
        }

        #endregion
    }
}
