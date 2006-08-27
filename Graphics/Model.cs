using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public class Model// : MeshContainer
    {
        private IMesh mesh;
        ExtendedMaterial[] materials;

        public ExtendedMaterial[] Materials
        {
            get { return materials; }
            set { materials = value; }
        }

        public IMesh IMesh
        {
            get { return mesh; }
            set { mesh = value; }
        }

        public Model(IMesh mesh, ExtendedMaterial[] materials)
        {
            IMesh = mesh;
            Materials = materials;
        }
    }
}
