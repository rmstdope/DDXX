using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    public class SkinInformationAdapter : ISkinInformation
    {
        private SkinInformation skinInformation;

        public SkinInformationAdapter(SkinInformation skin)
        {
            skinInformation = skin;
        }

        #region ISkinInformation Members

        public VertexElement[] Declaration
        {
            get
            {
                return skinInformation.Declaration;
            }
            set
            {
                skinInformation.Declaration = value;
            }
        }

        public bool Disposed
        {
            get { return skinInformation.Disposed; }
        }

        public int MaxVertexInfluences
        {
            get { return skinInformation.MaxVertexInfluences; }
        }

        public float MinBoneInfluence
        {
            get
            {
                return skinInformation.MinBoneInfluence;
            }
            set
            {
                skinInformation.MinBoneInfluence = value;
            }
        }

        public int NumberBones
        {
            get { return skinInformation.NumberBones; }
        }

        public VertexFormats VertexFormat
        {
            get
            {
                return skinInformation.VertexFormat;
            }
            set
            {
                skinInformation.VertexFormat = value;
            }
        }

        public SkinInformation Clone()
        {
            return skinInformation.Clone();
        }

        public IMesh ConvertToBlendedMesh(IMesh mesh, MeshFlags options, IGraphicsStream adjacencyIn, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable)
        {
            return new MeshAdapter(skinInformation.ConvertToBlendedMesh(((MeshAdapter)mesh).DXMesh, options, ((GraphicsStreamAdapter)adjacencyIn).DXGraphicsStream, out maxFaceInfluence, out boneCombinationTable));
        }

        public IMesh ConvertToBlendedMesh(IMesh mesh, MeshFlags options, int[] adjacencyIn, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable)
        {
            return new MeshAdapter(skinInformation.ConvertToBlendedMesh(((MeshAdapter)mesh).DXMesh, options, adjacencyIn, out maxFaceInfluence, out boneCombinationTable));
        }

        public IMesh ConvertToBlendedMesh(IMesh mesh, MeshFlags options, IGraphicsStream adjacencyIn, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            GraphicsStream stream;
            IMesh newMesh = new MeshAdapter(skinInformation.ConvertToBlendedMesh(((MeshAdapter)mesh).DXMesh, options, ((GraphicsStreamAdapter)adjacencyIn).DXGraphicsStream, out maxFaceInfluence, out boneCombinationTable, out adjacencyOut, out faceRemap, out stream));
            vertexRemap = new GraphicsStreamAdapter(stream);
            return newMesh;
        }

        public IMesh ConvertToBlendedMesh(IMesh mesh, MeshFlags options, int[] adjacencyIn, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            GraphicsStream stream;
            IMesh newMesh = new MeshAdapter(skinInformation.ConvertToBlendedMesh(((MeshAdapter)mesh).DXMesh, options, adjacencyIn, out maxFaceInfluence, out boneCombinationTable, out adjacencyOut, out faceRemap, out stream));
            vertexRemap = new GraphicsStreamAdapter(stream);
            return newMesh;
        }

        public IMesh ConvertToIndexedBlendedMesh(IMesh mesh, MeshFlags options, IGraphicsStream adjacencyIn, int paletteSize, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable)
        {
            return new MeshAdapter(skinInformation.ConvertToIndexedBlendedMesh(((MeshAdapter)mesh).DXMesh, options, ((GraphicsStreamAdapter)adjacencyIn).DXGraphicsStream, paletteSize, out maxFaceInfluence, out boneCombinationTable));
        }

        public IMesh ConvertToIndexedBlendedMesh(IMesh mesh, MeshFlags options, int[] adjacencyIn, int paletteSize, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable)
        {
            return new MeshAdapter(skinInformation.ConvertToIndexedBlendedMesh(((MeshAdapter)mesh).DXMesh, options, adjacencyIn, paletteSize, out maxFaceInfluence, out boneCombinationTable));
        }

        public IMesh ConvertToIndexedBlendedMesh(IMesh mesh, MeshFlags options, IGraphicsStream adjacencyIn, int paletteSize, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            GraphicsStream stream;
            IMesh newMesh = new MeshAdapter(skinInformation.ConvertToIndexedBlendedMesh(((MeshAdapter)mesh).DXMesh, options, ((GraphicsStreamAdapter)adjacencyIn).DXGraphicsStream, paletteSize, out maxFaceInfluence, out boneCombinationTable, out adjacencyOut, out faceRemap, out stream));
            vertexRemap = new GraphicsStreamAdapter(stream);
            return newMesh;
        }

        public IMesh ConvertToIndexedBlendedMesh(IMesh mesh, MeshFlags options, int[] adjacencyIn, int paletteSize, out int maxFaceInfluence, out BoneCombination[] boneCombinationTable, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            GraphicsStream stream;
            IMesh newMesh = new MeshAdapter(skinInformation.ConvertToIndexedBlendedMesh(((MeshAdapter)mesh).DXMesh, options, adjacencyIn, paletteSize, out maxFaceInfluence, out boneCombinationTable, out adjacencyOut, out faceRemap, out stream));
            vertexRemap = new GraphicsStreamAdapter(stream);
            return newMesh;
        }

        public BoneInfluences GetBoneInfluence(int bone)
        {
            return skinInformation.GetBoneInfluence(bone);
        }

        public string GetBoneName(int bone)
        {
            return skinInformation.GetBoneName(bone);
        }

        public Matrix GetBoneOffsetMatrix(int bone)
        {
            return GetBoneOffsetMatrix(bone);
        }

        public int GetMaxFaceInfluences(IndexBuffer indexBuffer, int numFaces)
        {
            return skinInformation.GetMaxFaceInfluences(indexBuffer, numFaces);
        }

        public int GetNumBoneInfluences(int bone)
        {
            return GetNumBoneInfluences(bone);
        }

        public void Remap(int[] vertRemap)
        {
            skinInformation.Remap(vertRemap);
        }

        public void SetBoneInfluence(int bone, int numInfluences, int[] vertices, float[] weights)
        {
            skinInformation.SetBoneInfluence(bone, numInfluences, vertices, weights);
        }

        public void SetBoneName(int bone, string name)
        {
            skinInformation.SetBoneName(bone, name);
        }

        public void SetBoneOffsetMatrix(int bone, Matrix boneTransform)
        {
            skinInformation.SetBoneOffsetMatrix(bone, boneTransform);
        }

        public Array UpdateSkinnedMesh(Matrix[] boneTransforms, Matrix[] boneInvTransforms, Array verticesSource)
        {
            return skinInformation.UpdateSkinnedMesh(boneTransforms, boneInvTransforms, verticesSource);
        }

        public void UpdateSkinnedMesh(Matrix[] boneTransforms, Matrix[] boneInvTransforms, IGraphicsStream sourceVertices, IGraphicsStream destinationVertices)
        {
            skinInformation.UpdateSkinnedMesh(boneTransforms, boneInvTransforms, ((GraphicsStreamAdapter)sourceVertices).DXGraphicsStream, ((GraphicsStreamAdapter)destinationVertices).DXGraphicsStream);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            skinInformation.Dispose();
        }

        #endregion
    }
}
