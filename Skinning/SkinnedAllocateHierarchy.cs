using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Skinning
{
    public class SkinnedAllocateHierarchy : AllocateHierarchy
    {
        public override Frame CreateFrame(string name)
        {
            return new SkinnedFrame(name);
        }

        public override MeshContainer CreateMeshContainer(string name, MeshData meshData, ExtendedMaterial[] materials, EffectInstance[] effectInstances, Microsoft.DirectX.GraphicsStream adjacency, SkinInformation skinInfo)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
