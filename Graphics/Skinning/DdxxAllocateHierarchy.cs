using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    public class DdxxAllocateHierarchy : AllocateHierarchy
    {
        public override Frame CreateFrame(string name)
        {
            return new SkinnedFrame(name);
        }

        public override MeshContainer CreateMeshContainer(string name, MeshData meshData, ExtendedMaterial[] materials, EffectInstance[] effectInstances, GraphicsStream adjacency, SkinInformation skinInfo)
        {
            return new SkinnedMeshContainer(name, meshData, materials, effectInstances, adjacency, skinInfo);
        }
    }
}
