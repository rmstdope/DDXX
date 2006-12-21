using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    public class MeshDataAdapter : IMeshData
    {
        private MeshData meshData;

        public MeshDataAdapter(MeshData meshData)
        {
            this.meshData = meshData;
        }

        public MeshData DXMeshData
        {
            get { return meshData; }
        }

        #region IMeshData Members

        public IMesh Mesh
        {
            get
            {
                return new MeshAdapter(meshData.Mesh);
            }
            set
            {
                meshData.Mesh = ((MeshAdapter)value).DXMesh;
            }
        }

        public PatchMesh PatchMesh
        {
            get
            {
                return meshData.PatchMesh;
            }
            set
            {
                meshData.PatchMesh = value;
            }
        }

        public ProgressiveMesh ProgressiveMesh
        {
            get
            {
                return meshData.ProgressiveMesh;
            }
            set
            {
                meshData.ProgressiveMesh = value;
            }
        }

        #endregion
    }
}
