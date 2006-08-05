using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Direct3D
{
    public class MeshAdapter : IMesh
    {
        private Mesh mesh;

        public MeshAdapter(Device device, float width, float height, float depth)
        {
            mesh = Mesh.Box(device, width, height, depth);
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

        #endregion
    }
}
