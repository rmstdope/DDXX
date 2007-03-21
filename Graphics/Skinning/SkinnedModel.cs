using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    public class SkinnedModel : ModelBase
    {
        private IAnimationRootFrame rootFrame;
        private const int MAX_NUM_BONES = 60;

        public SkinnedModel(IAnimationRootFrame rootFrame, ITextureFactory textureFactory)
        {
            this.rootFrame = rootFrame;
            int numMaterials = CountMaterials(rootFrame.FrameHierarchy);
            Materials = new ModelMaterial[numMaterials];
            int startIndex = 0;
            ScanForMeshContainers(rootFrame.FrameHierarchy, ref startIndex, textureFactory);
        }

        private int CountMaterials(IFrame frame)
        {
            int numMaterials = 0;
            if (frame.MeshContainer != null)
                numMaterials += frame.MeshContainer.GetMaterials().Length;
            if (frame.FrameSibling != null)
                numMaterials += CountMaterials(frame.FrameSibling);
            if (frame.FrameFirstChild != null)
                numMaterials += CountMaterials(frame.FrameFirstChild);
            return numMaterials;
        }

        private void ScanForMeshContainers(IFrame frame, ref int startIndex, ITextureFactory textureFactory)
        {
            if (frame.FrameSibling != null)
                ScanForMeshContainers(frame.FrameSibling, ref startIndex, textureFactory);
            if (frame.FrameFirstChild != null)
                ScanForMeshContainers(frame.FrameFirstChild, ref startIndex, textureFactory);
            if (frame.MeshContainer != null)
            {
                ModelMaterial[] materials = CreateModelMaterials(textureFactory, frame.MeshContainer.GetMaterials());
                materials.CopyTo(Materials, startIndex);
                startIndex += materials.Length;
                if (frame.MeshContainer.SkinInformation != null)
                {
                    int numBones = Math.Min(MAX_NUM_BONES, frame.MeshContainer.SkinInformation.NumberBones);
                    int influences = 0;
                    BoneCombination[] bones = null;
                    MeshDataAdapter data = new MeshDataAdapter();
                    data.Mesh = frame.MeshContainer.SkinInformation.ConvertToIndexedBlendedMesh(
                        frame.MeshContainer.MeshData.Mesh, MeshFlags.Managed | MeshFlags.OptimizeVertexCache,
                        frame.MeshContainer.GetAdjacencyStream(), numBones, out influences,
                        out bones);
                    frame.MeshContainer.MeshData = data;
                    frame.MeshContainer.Bones = bones;

                    Matrix[] offsetMatrices = new Matrix[numBones];
                    for (int i = 0; i < numBones; i++)
                        offsetMatrices[i] = frame.MeshContainer.SkinInformation.GetBoneOffsetMatrix(i);
                    frame.MeshContainer.RestMatrices = offsetMatrices;

                    IFrame[] frameMatrix = new IFrame[numBones];
                    for (int i = 0; i < numBones; i++)
                    {
                        IFrame foundFrame = frame.Find(rootFrame.FrameHierarchy,
                            frame.MeshContainer.SkinInformation.GetBoneName(i));
                        if (foundFrame == null)
                            throw new InvalidOperationException("Could not find valid bone.");
                        frameMatrix[i] = foundFrame;
                    }
                    frame.MeshContainer.Frames = frameMatrix;
                }
            }
        }

        public override IMesh Mesh
        {
            get { throw new DDXXException("Depricated! This should be removed!"); }
            set { throw new DDXXException("Depricated! This should be removed!"); }
        }

        public override void DrawSubset(int subset)
        {
            throw new DDXXException("Depricated! This should be removed!");
        }

        public override bool IsSkinned()
        {
            return true;
        }

        public override void Draw(IEffectHandler effectHandler, ColorValue ambient, Matrix world, Matrix view, Matrix projection)
        {
            int materialIndex = 0;
            DrawMeshContainer(rootFrame.FrameHierarchy, Matrix.Identity, ref materialIndex, effectHandler, ambient, world, view, projection);
        }

        private void DrawMeshContainer(IFrame frame, Matrix parentMatrix, ref int materialIndex, IEffectHandler effectHandler, ColorValue ambient, Matrix world, Matrix view, Matrix projection)
        {
            if (frame.FrameSibling != null)
            {
                DrawMeshContainer(frame.FrameSibling, parentMatrix, ref materialIndex, effectHandler, ambient, world, view, projection);
            }
            Matrix matrix = frame.TransformationMatrix * parentMatrix;
            if (frame.FrameFirstChild != null)
            {
                DrawMeshContainer(frame.FrameFirstChild, matrix, ref materialIndex, effectHandler, ambient, world, view, projection);
            }
            if (frame.MeshContainer != null)
            {
                effectHandler.SetNodeConstants(matrix * world, view, projection);
                BoneCombination[] bones = frame.MeshContainer.Bones;
                for (int j = 0; j < frame.MeshContainer.GetMaterials().Length; j++)
                {
                    if (frame.MeshContainer.SkinInformation != null)
                    {
                        Matrix[] skinMatrices = new Matrix[frame.MeshContainer.SkinInformation.NumberBones];
                        for (int i = 0; i < frame.MeshContainer.SkinInformation.NumberBones; i++)
                        {
                            int index = bones[j].BoneId[i];
                            if (index != -1)
                            {
                                skinMatrices[i] = frame.MeshContainer.RestMatrices[index] * 
                                    frame.MeshContainer.Frames[index].CombinedTransformationMatrix;
                            }
                        }
                        effectHandler.SetBones(skinMatrices);
                    }
                    effectHandler.SetMaterialConstants(ambient, Materials[materialIndex + j], j + materialIndex);
                    int passes = effectHandler.Effect.Begin(FX.None);
                    for (int i = 0; i < passes; i++)
                    {
                        effectHandler.Effect.BeginPass(i);
                        frame.MeshContainer.MeshData.Mesh.DrawSubset(j);
                        effectHandler.Effect.EndPass();
                    }
                    effectHandler.Effect.End();
                }
                materialIndex += frame.MeshContainer.GetMaterials().Length;
            }
        }

        public override void Step()
        {
            if (rootFrame.AnimationController != null)
                rootFrame.AnimationController.AdvanceTime(Time.DeltaTime);

            UpdateFrameMatrices(rootFrame.FrameHierarchy, Matrix.Identity);
        }

        private void UpdateFrameMatrices(IFrame frame, Matrix parentMatrix)
        {
            frame.CombinedTransformationMatrix = frame.TransformationMatrix * parentMatrix;

            if (frame.FrameSibling != null)
            {
                UpdateFrameMatrices(frame.FrameSibling, parentMatrix);
            }

            if (frame.FrameFirstChild != null)
            {
                UpdateFrameMatrices(frame.FrameFirstChild, frame.CombinedTransformationMatrix);
            }
        }
    }
}
