using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Skinning
{
    public class SkinMeshContainer : MeshContainer
    {
        private string filePath;

        public SkinMeshContainer(string filePath, string name, MeshData meshData, 
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
            //this.textures = null;
            this.filePath = filePath;
        }
    }
}
