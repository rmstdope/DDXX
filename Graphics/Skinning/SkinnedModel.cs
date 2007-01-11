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
                for (int j = 0; j < frame.MeshContainer.GetMaterials().Length; j++)
                {
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
        }
    }
}
