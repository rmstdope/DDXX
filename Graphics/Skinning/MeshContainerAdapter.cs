using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    public class MeshContainerAdapter : IMeshContainer
    {
        private MeshContainer container;

        public MeshContainerAdapter(MeshContainer container)
        {
            this.container = container;
        }

        public MeshContainer DXMeshContainer
        {
            get { return container; }
        }

        #region IMeshContainer Members

        public MeshDataAdapter MeshData
        {
            get
            {
                MeshDataAdapter meshDataAdapter = new MeshDataAdapter();
                if (container.MeshData.Mesh != null)
                    meshDataAdapter.Mesh = new MeshAdapter(container.MeshData.Mesh);
                meshDataAdapter.PatchMesh = container.MeshData.PatchMesh;
                meshDataAdapter.ProgressiveMesh = container.MeshData.ProgressiveMesh;
                return meshDataAdapter;
            }
            set
            {
                MeshData data = new MeshData();
                data.Mesh = ((MeshAdapter)value.Mesh).DXMesh;
                if (value.PatchMesh != null)
                    data.PatchMesh = value.PatchMesh;
                if (value.ProgressiveMesh != null)
                    data.ProgressiveMesh = value.ProgressiveMesh;
                container.MeshData = data;
            }
        }

        public string Name
        {
            get
            {
                return container.Name;
            }
            set
            {
                container.Name = value;
            }
        }

        public IMeshContainer NextContainer
        {
            get { return new MeshContainerAdapter(container.NextContainer); }
        }

        public ISkinInformation SkinInformation
        {
            get
            {
                return new SkinInformationAdapter(container.SkinInformation);
            }
            set
            {
                container.SkinInformation = (value as SkinInformationAdapter).DXSkinInformation;
            }
        }

        public int[] GetAdjacency()
        {
            return container.GetAdjacency();
        }

        public IGraphicsStream GetAdjacencyStream()
        {
            return new GraphicsStreamAdapter(container.GetAdjacencyStream());
        }

        public EffectInstance[] GetEffectInstances()
        {
            return container.GetEffectInstances();
        }

        public ExtendedMaterial[] GetMaterials()
        {
            return container.GetMaterials();
        }

        public void SetAdjacency(IGraphicsStream adj)
        {
            container.SetAdjacency(((GraphicsStreamAdapter)adj).DXGraphicsStream);
        }

        public void SetAdjacency(int[] adj)
        {
            container.SetAdjacency(adj);
        }

        public void SetEffectInstances(EffectInstance[] effects)
        {
            container.SetEffectInstances(effects);
        }

        public void SetMaterials(ExtendedMaterial[] mtrl)
        {
            container.SetMaterials(mtrl);
        }

        #endregion
    }
}
