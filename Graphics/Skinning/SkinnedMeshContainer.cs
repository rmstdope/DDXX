using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public class SkinnedMeshContainer : MeshContainer
    {
        private Matrix[] restMatrices;
        private BoneCombination[] bones;
        private IFrame[] frames;

        public SkinnedMeshContainer(string name, MeshData meshData, 
            ExtendedMaterial[] materials, EffectInstance[] effectInstances, 
            GraphicsStream adjacency, SkinInformation skinInfo) 
            : base()
        {
            this.Name = name;
            this.MeshData = meshData;
            this.SetMaterials(materials);
            this.SetEffectInstances(effectInstances);
            this.SetAdjacency(adjacency);
            this.SkinInformation = skinInfo;
        }

        public Matrix[] RestMatrices
        {
            get { return restMatrices; }
            set { restMatrices = value; }
        }

        public BoneCombination[] Bones
        {
            get { return bones; }
            set { bones = value; }
        }

        public IFrame[] Frames
        {
            get { return frames; }
            set { frames = value; }
        }

    }
}
