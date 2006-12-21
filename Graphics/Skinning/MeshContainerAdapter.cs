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

        public IMeshData MeshData
        {
            get
            {
                return new MeshDataAdapter(container.MeshData);
            }
            set
            {
                container.MeshData = ((MeshDataAdapter)value).DXMeshData;
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

        public SkinInformation SkinInformation
        {
            get
            {
                return container.SkinInformation;
            }
            set
            {
                container.SkinInformation = value;
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
