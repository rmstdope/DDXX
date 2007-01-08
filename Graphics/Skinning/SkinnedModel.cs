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
            ScanForMeshContainers(rootFrame.FrameHierarchy, 0, textureFactory);
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

        private void ScanForMeshContainers(IFrame frame, int startIndex, ITextureFactory textureFactory)
        {
            if (frame.MeshContainer != null)
            {
                ModelMaterial[] materials = CreateModelMaterials(textureFactory, frame.MeshContainer.GetMaterials());
                materials.CopyTo(Materials, startIndex);
                startIndex += materials.Length;
            }
            if (frame.FrameSibling != null)
                ScanForMeshContainers(frame.FrameSibling, startIndex, textureFactory);
            if (frame.FrameFirstChild != null)
                ScanForMeshContainers(frame.FrameFirstChild, startIndex, textureFactory);
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
            DrawMeshContainer(rootFrame.FrameHierarchy, 0, effectHandler, ambient, world, view, projection);
        }

        private void DrawMeshContainer(IFrame frame, int materialIndex, IEffectHandler effectHandler, ColorValue ambient, Matrix world, Matrix view, Matrix projection)
        {
            if (frame.MeshContainer != null)
            {
                for (int j = 0; j < frame.MeshContainer.GetMaterials().Length; j++)
                {
                    effectHandler.SetMaterialConstants(ambient, Materials[materialIndex], j);
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
            if (frame.FrameSibling != null)
            {
                DrawMeshContainer(frame.FrameSibling, materialIndex, effectHandler, ambient, world, view, projection);
            }
            if (frame.FrameFirstChild != null)
            {
                DrawMeshContainer(frame.FrameFirstChild, materialIndex, effectHandler, ambient, world, view, projection);
            }
        }
    }
}
