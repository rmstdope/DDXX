using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics.Skinning
{
    public class SkinnedMeshContainer : MeshContainer
    {
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
    }
}
