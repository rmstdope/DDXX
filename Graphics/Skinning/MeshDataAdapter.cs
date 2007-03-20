using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    public struct MeshDataAdapter
    {
        private IMesh mesh;
        private PatchMesh patchMesh;
        private ProgressiveMesh progressiveMesh;

        public IMesh Mesh
        {
            get
            {
                return mesh;
            }
            set
            {
                mesh = value;
            }
        }

        public PatchMesh PatchMesh
        {
            get
            {
                return patchMesh;
            }
            set
            {
                patchMesh = value;
            }
        }

        public ProgressiveMesh ProgressiveMesh
        {
            get
            {
                return progressiveMesh;
            }
            set
            {
                progressiveMesh = value;
            }
        }

    }
}
